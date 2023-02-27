#include "esp32_general.h"
#include "M95_uart.h"
#include "uart_rs485.h"


#include <stdio.h>
#include <string.h>
#include <stdlib.h>
#include <time.h>
#include "freertos/FreeRTOS.h"
#include "freertos/task.h"
#include "esp_system.h"
#include "nvs_flash.h"
#include "driver/uart.h"
#include "freertos/queue.h"
#include "esp_log.h"
#include "sdkconfig.h"

#include "crc.h"
#include "ota_control.h"
#include "ota_headers.h"
#include "ota_esp32.h"
#include "ota_m95.h"
#include <cjson.h>

// Read packet timeout
#define SENSOR_TASK_STACK_SIZE    (4096)
#define MODEM_TASK_STACK_SIZE     (2048)
#define SENSOR_TASK_PRIO          (10)
#define MODEM_TASK_PRIO           (9)
#define TIME_SLEEP 30                 // Time in "min"
#define TAG "Saphi_sensor"           // Nombre del TAG

#define BUF_SIZE_MODEM (1024)
#define RD_BUF_SIZE (BUF_SIZE_MODEM)
cJSON *doc;
char * output;
uint32_t current_time=0;
char watchdog_en=1;
static QueueHandle_t uart_modem_queue;
int rxBytesModem;

int wake_count;

uint8_t rx_modem_ready;
uint8_t * p_RxModem;
void OTA_check(void);
bool state_blink_led = false;       // Blink indicador de funcionamiento
uint8_t* buffer_rs485;              // buffer para leer datos del RS485
char level_battery[8];
float factors[7] = {10.0 , 10.0 , 1.0 , 10.0 , 1.0 , 1.0 , 1.0};
 
struct datos_modem{
    char    IMEI[25];
    char    topic[50];
    char    ota[50];
    int     signal;
    float   battery;
    int     datos_sin_enviar;
    char    fecha[30];
    bool    b_topic;
    int     mode;       // 0 normal - 1 OTA
};

struct datos_modem m95 = {  .IMEI = "0", .topic="Agro_test/", .ota = "Agro_test/",
                            .signal = -99, .battery = 0.00, .datos_sin_enviar=0,
                            .fecha = "01/01/23,00:00:00", .b_topic = false, .mode = 0};

void power_off_gpio(){
    deactivate_pin(ESP_ERROR_PIN);
    deactivate_pin(ESP_LED_PIN);
    deactivate_pin(ESP_READY_PIN);
}

void m95_config(){

	// GPIO PIN CONFIGURATION
	//gpio_pad_select_gpio(STATUS_Pin);
	gpio_reset_pin(STATUS_Pin);
    //gpio_pad_select_gpio(PWRKEY_Pin);
	gpio_reset_pin(PWRKEY_Pin);
    gpio_set_direction(STATUS_Pin, GPIO_MODE_INPUT);
    gpio_set_direction(PWRKEY_Pin, GPIO_MODE_OUTPUT);
    ESP_ERROR_CHECK(gpio_pulldown_en(STATUS_Pin));


	// SERIAL PORT CONFIGURATION
	const int uart_m95      = UART_MODEM;
    uart_config_t uart_config_modem = {
		.baud_rate = BAUD_RATE_M95,
		.data_bits = UART_DATA_8_BITS,
		.parity = UART_PARITY_DISABLE,
		.stop_bits = UART_STOP_BITS_1,
		.flow_ctrl = UART_HW_FLOWCTRL_DISABLE,
		.source_clk = UART_SCLK_APB,
	};

    // Install UART driver (we don't need an event queue here)
    ESP_ERROR_CHECK(uart_driver_install(UART_MODEM, BUF_SIZE_MODEM * 2, BUF_SIZE_MODEM * 2, 20, &uart_modem_queue, 0));
    // Configure UART parameters
    ESP_ERROR_CHECK(uart_param_config(UART_MODEM, &uart_config_modem));
    // Set UART pins as per KConfig settings
    ESP_ERROR_CHECK(uart_set_pin(uart_m95, M95_TXD, M95_RXD, M95_RTS, M95_CTS));
    // Set read timeout of UART TOUT feature
    //ESP_ERROR_CHECK(uart_set_rx_timeout(uart_m95, ECHO_READ_TOUT));

	// Modem start: OFF
	deactivate_pin(PWRKEY_Pin);
}


static void MODEM_event_task(void *pvParameters){
    uart_event_t event;
    uint8_t* dtmp = (uint8_t*) malloc(RD_BUF_SIZE);
    p_RxModem = dtmp;
    int ring_buff_len;
    for(;;) {
        //Waiting for UART event.
        if(xQueueReceive(uart_modem_queue, (void * )&event, (TickType_t )portMAX_DELAY)) {
            if(rx_modem_ready == 0){
              bzero(dtmp, RD_BUF_SIZE);
            }
            switch(event.type) {
                case UART_DATA:
                    if(event.size >= 120){
                        vTaskDelay(100/portTICK_PERIOD_MS);
                        ESP_LOGI(TAG, "OVER [UART DATA]: %d", event.size);
                    }
                    if(rx_modem_ready == 0){
                      uart_get_buffered_data_len(UART_MODEM,(size_t *)&ring_buff_len);
                      rxBytesModem = uart_read_bytes(UART_MODEM,dtmp,ring_buff_len,0);
                      rx_modem_ready = 1;
                    }
                    break;
                default:
                    vTaskDelay(10);
                    break;
            }
        }
        
    }
    free(dtmp);
    dtmp = NULL;
    vTaskDelete(NULL);
}

// Tarea principal donde leemos y enviamos los datos a la database
static void rs485_task(){

    m95.signal= get_M95_signal();
    strcpy(m95.IMEI,get_M95_IMEI());
    strcat(m95.topic,m95.IMEI);
    strcpy(m95.ota,m95.topic);
    strcat(m95.topic,"/db");
    strcat(m95.ota,"/OTA");
    printf("Topic direction = %s\n",m95.topic);
    // Allocate buffers for UART
    buffer_rs485        = (uint8_t*) malloc(BUF_SIZE_RS485);
    char* msg_mqtt   = (char*) malloc(BUF_SIZE_MODEM);
    char* string_temp   = (char*) malloc (50); 
    activate_pin(ESP_LED_PIN);
    vTaskDelay(100/ portTICK_PERIOD_MS);

    // Obtenemos la fecha y el nivel de bateria
    strcpy(m95.fecha,get_m95_date());
    m95.battery = read_battery();
    strcpy(msg_mqtt,m95.fecha);
    strcat(msg_mqtt,"@");
    sprintf(string_temp,"%dU",m95.signal);
    strcat(msg_mqtt,string_temp);
    strcat(msg_mqtt,"@");
    sprintf(string_temp,"%.2fV",m95.battery);
    strcat(msg_mqtt,string_temp);
    strcat(msg_mqtt,"\r\n");
    deactivate_pin(ESP_LED_PIN);

    // Obtenemos los valores del sensor
    int param_initial = 0;
    int param_final = 6;
    size_t numero_param = param_final - param_initial + 1;
    float data1[numero_param];
    float data2[numero_param];

    ESP_LOGI(TAG, ": Activando modulo RS485 ...  \n");
    deactivate_pin(RS485_ENABLE_PIN);
    activate_pin(ESP_READY_PIN);
    vTaskDelay(3000/ portTICK_PERIOD_MS);
    
    // Lecutra SENSOR 1
    ESP_LOGI(TAG," Leyendo sensor 1");
    int result1 = get_sensor_n_data(1,data1,param_initial,numero_param,buffer_rs485);
    vTaskDelay(300 / portTICK_PERIOD_MS);
    // Lectura SENSOR 2
    ESP_LOGI(TAG," Leyendo sensor 2");
    int result2 = get_sensor_n_data(2,data2,param_initial,numero_param,buffer_rs485);
    vTaskDelay(300 / portTICK_PERIOD_MS);
    // Apagamos el modulo RS485
    deactivate_pin(ESP_READY_PIN);
    activate_pin(RS485_ENABLE_PIN);

    if(result1 != 1){
        for(int i =0; i<numero_param; i++){
            sprintf(string_temp,"#%.1f",-99.9);
            strcat(msg_mqtt,string_temp);
        }
    }
    else{
        for(int i =0; i<numero_param; i++){
            sprintf(string_temp,"#%.1f",(float)(data1[i]/factors[i]));
            strcat(msg_mqtt,string_temp);
        }
    }
    strcat(msg_mqtt,"\r\n");
    if(result2 != 1){
        for(int i =0; i<numero_param; i++){
            sprintf(string_temp,"#%.1f",-99.9);
            strcat(msg_mqtt,string_temp);
        }
    }
    else{
        for(int i =0; i<numero_param; i++){
            sprintf(string_temp,"#%.1f",(float)(data2[i]/factors[i]));
            strcat(msg_mqtt,string_temp); 
        }
    }
    ESP_LOGI("Msg from buffer","\n  \n%s\n",msg_mqtt);

    // Subir datos al broker MQTT
    int a = connect_MQTT_server();
    for(int m=0; m<4; m++){
        if(a != 1){
            deactivate_pin(ESP_READY_PIN);
            activate_pin(ESP_ERROR_PIN);
            vTaskDelay(2000 / portTICK_PERIOD_MS);
            ESP_LOGE(TAG,"Trying connection... N°%d\n",m+1);
            a = M95_CheckConnection();
            continue;
        }
        if(M95_PubMqtt_data((uint8_t*)msg_mqtt,m95.topic,strlen(msg_mqtt),1,0)){
            a = 1;
            deactivate_pin(ESP_ERROR_PIN);
            activate_pin(ESP_READY_PIN);
            break;
        }
        activate_pin(ESP_ERROR_PIN);
    }
    if(a!=1){
        ESP_LOGE(TAG,"No se establecio conexion al broker");
    }

    // Chequeamos si el OTA es requerido
    wake_count = M95_check_subs(m95.IMEI);

    ESP_LOGI(TAG,"\n\tEstado del ota = \" %s \"\n",(wake_count == 1 ? "Requerido":"No requerido"));
    disconnect_mqtt();

    deactivate_pin(WHITE_LED);
    
    // ---------------------------------------------------------------------------
    // ---------------------------------------------------------------------------

    if(wake_count > 0){
        doc = cJSON_CreateObject();
        cJSON_AddItemToObject(doc,"imei",cJSON_CreateString(m95.IMEI));
        cJSON_AddItemToObject(doc,"project",cJSON_CreateString("test_project"));
        cJSON_AddItemToObject(doc,"ota",cJSON_CreateString("true"));
        cJSON_AddItemToObject(doc,"cmd",cJSON_CreateString("false"));
        cJSON_AddItemToObject(doc,"sw",cJSON_CreateString("1.1"));
        cJSON_AddItemToObject(doc,"hw",cJSON_CreateString("1.1"));
        cJSON_AddItemToObject(doc,"otaV",cJSON_CreateString("1.0"));
        output = cJSON_PrintUnformatted(doc);

        printf("Mensaje OTA:\r\n");
        printf(output);
        printf("\r\n");
        
        //TODO:poner en otro lado
        do{
            OTA_check();
        }while(false);
        vTaskDelay(1000 / portTICK_PERIOD_MS);
    }


    ESP_LOGI(TAG,"Apagando Modem y pines\n");
    if(M95_poweroff_command()>2){
            M95_poweroff();
    };
    // Sleep Mode
    free(string_temp);
    free(buffer_rs485);
    free(msg_mqtt);
    activate_pin(RS485_ENABLE_PIN);
    power_off_gpio();

	gpio_hold_en(RS485_ENABLE_PIN);
    gpio_hold_en(PWRKEY_Pin);
    gpio_deep_sleep_hold_en();
    esp_sleep_pd_config(ESP_PD_DOMAIN_RTC_SLOW_MEM, ESP_PD_OPTION_ON);
    printf("\nSleep Mode = %d min\n",(int)(TIME_SLEEP));
    //esp_sleep_enable_timer_wakeup(TIME_SLEEP * MIN_TO_S * S_TO_US- esp_timer_get_time());
    esp_sleep_enable_timer_wakeup(2*60 * S_TO_US);
    esp_deep_sleep_start();


    vTaskDelete(NULL);
}

void OTA_check(void){
  char buffer[700];
  static const char *OTA_TAG = "OTA_task";
  esp_log_level_set(OTA_TAG, ESP_LOG_INFO);

  printf("OTA:Revisando conexion...\r\n");
  bool repuesta_ota = false;
  int intentos = 0;
  do{
      intentos++;
      printf("Intento n°%d\r\n",intentos);
      if(!TCP_open()){
		ESP_LOGI(OTA_TAG,"No se conecto al servidor");
		TCP_close();
		printf("OTA:Desconectado\r\n");
		break;
	  }

	  printf("OTA:Solicitando actualizacion...\r\n");
	  if(TCP_send(output, strlen(output))){                           // 1. Se envia la info del dispositivo
		//printf("OTA:Esperando respuesta...\r\n");
		readAT("}\r\n", "-8/()/(\r\n",15000,buffer);   // 2. Se recibe la 1ra respuesta con ota True si tiene un ota pendiente... (el servidor lo envia justo despues de recibir la info)(}\r\n para saber cuando llego la respuesta)
		debug_ota("main> repta %s\r\n", buffer);
		if(strstr(buffer,"\"ota\": \"true\"") != 0x00  ){
            repuesta_ota = true;
			ESP_LOGI(OTA_TAG,"Iniciando OTA");

			//printf("Watchdog desactivado\r\n");
			watchdog_en=0;

			if(ota_uartControl_M95() == OTA_EX_OK){
			    debug_ota("main> OTA m95 Correcto...\r\n");
                esp_restart();    
			}
			else{
			  debug_ota("main> OTA m95 Error...\r\n");
			}
			current_time = pdTICKS_TO_MS(xTaskGetTickCount())/1000;
			watchdog_en=1;
			printf("Watchdog reactivado\r\n");
		}
		printf("OTA:No hubo respuesta\r\n");
	  }
	}
	while(!repuesta_ota&(intentos<5));
}

void app_main(void)
{
    printf("---- Iniciando programa ... \n");
    esp_sleep_wakeup_cause_t wakeup_reason;
    wakeup_reason = esp_sleep_get_wakeup_cause();
    switch(wakeup_reason){
        case ESP_SLEEP_WAKEUP_TIMER: 
            printf("\t... Comenzando del reinicio\n");
            gpio_hold_dis(PWRKEY_Pin);
            gpio_hold_dis(RS485_ENABLE_PIN);
            break;
        default: printf("\t\t {Comenzo desde el restart} \n"); 
                 break;

    }

    if(true){
        ESP_LOGI("Configuration ...","Pins Mode Input, Output, ADC");
        config_pin_esp32();
        m95_config();
        rs485_config();
    }
    wake_count = 0;
    xTaskCreate(MODEM_event_task, "MODEM_event_task", 4096, NULL, 12, NULL);
    
    M95_checkpower();
    printf("Estado: Configuracion de Modem M95 ...  \n");
    M95_begin();

    xTaskCreate(rs485_task, "rs485_task", SENSOR_TASK_STACK_SIZE, NULL, 9, NULL);
}