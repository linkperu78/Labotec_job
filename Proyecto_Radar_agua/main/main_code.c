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
#define SENSOR_TASK_STACK_SIZE          (4096)
#define MODEM_TASK_STACK_SIZE           (2048)
#define SENSOR_TASK_PRIO                (10)
#define MODEM_TASK_PRIO                 (9)
#define TIME_SLEEP 30                   // Time in "min"
#define TAG "Agro_sensor"               // Nombre del TAG

#define BUF_SIZE_MODEM                  (1024)
#define RD_BUF_SIZE                     (BUF_SIZE_MODEM)

// Define PARAMETERS OF PROJECT
#define OUT_ALARM       GPIO_NUM_21
#define INPUT_PULL      GPIO_NUM_13

#define MAX_LEVEL_DEFAULT   18.6
#define MIN_LEVEL_DEFAULT   11.2


cJSON *doc;
char * output;

static QueueHandle_t uart_modem_queue;
int rxBytesModem;

int wake_count;

uint8_t rx_modem_ready;
uint8_t * p_RxModem;
bool ota_debug = false;

void OTA_check(void);
void init_project_config();

uint8_t* buffer_rs485;              // buffer para leer datos del RS485
RTC_DATA_ATTR int gpio_alarm;
RTC_DATA_ATTR float _max_level_sleep = MAX_LEVEL_DEFAULT;
RTC_DATA_ATTR float _min_level_sleep = MIN_LEVEL_DEFAULT;

int mode;

float factors[7] = {10.0 , 10.0 , 1.0 , 10.0 , 1.0 , 1.0 , 1.0};
 
struct datos_modem{
    char    IMEI[25];
    char    topic[50];
    char    ota[50];
    int     signal;
    float   battery;
    int     datos_sin_enviar;
    char    fecha[30];
};

struct datos_modem m95 = {  .IMEI = "0", .topic="Radar/", .ota = "Radar/",
                            .signal = 99, .battery = 0.00, .datos_sin_enviar=0,
                            .fecha = "01/01/23,00:00:00"};


static void M95_rx_event_task(void *pvParameters){
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
                        //ESP_LOGI(TAG, "OVER [UART DATA]: %d", event.size);
                        vTaskDelay(100/portTICK_PERIOD_MS);
                    }
                    if(rx_modem_ready == 0){  
                        //printf("Event_size = %d\n",event.size);
                        uart_get_buffered_data_len(UART_MODEM,(size_t *)&ring_buff_len);
                        rxBytesModem = uart_read_bytes(UART_MODEM,dtmp,ring_buff_len,0);
                        rx_modem_ready = 1;
                    }
                    break;
                    
                default:
                    break;
            }
        }
        
    }
    free(dtmp);
    dtmp = NULL;
    vTaskDelete(NULL);
}

// Tarea principal donde leemos y enviamos los datos a la database
static void _main_task(){
    // Allocate buffers for UART
    buffer_rs485  = (uint8_t*) malloc(BUF_SIZE_RS485);
    float _max = 0;
    float _min = 0;
    float level = 0.00;

    if(mode == 1){
        gpio_alarm = 0;
        deactivate_pin(RS485_ENABLE_PIN);
        vTaskDelay( 1000 / portTICK_PERIOD_MS );
        level = get_sensor_data(1, 1, buffer_rs485);
        printf("Altura = %.2f\n",level);
        if( (level >  _max) || (level < _min)){
            ESP_LOGE("WARNING", "Niveles fuera del rango aceptable\n");
            gpio_set_level( OUT_ALARM, 1 ); 
        }
        else{
            ESP_LOGI("OK","Niveles dentro del rango\n");
            gpio_set_level( OUT_ALARM, 0 );    
        }
        vTaskDelay( 1000 / portTICK_PERIOD_MS );
        activate_pin(RS485_ENABLE_PIN);


        uint64_t _time_sleep = (int)(3 * 60) * S_TO_US;
        printf("\nSleep Mode = %0.2f seconds\n", (float)( _time_sleep / S_TO_US ));
        esp_sleep_enable_timer_wakeup(_time_sleep/2);
        esp_deep_sleep_start();
    }

    char mensaje_subs[100];
    m95.signal= get_M95_signal();
    strcpy(m95.IMEI,get_M95_IMEI());
    strcat(m95.topic,m95.IMEI);
    strcpy(m95.ota,m95.topic);
    //strcat(m95.topic,"/altura");
    strcat(m95.ota,"/OTA");
    printf("Topic direction = %s\n",m95.topic);
    
    int a = connect_MQTT_server(0);


    // Activamos el RS485 aqui para darle tiempo al sensor para inicializar correctamente
    deactivate_pin(RS485_ENABLE_PIN);

    // Actualizamos el valor del max y min de los niveles
    wake_count = m95_sub_topic(0,m95.topic,mensaje_subs);

    int sucess_parse = 0;
    _max = get_json_value(mensaje_subs,"max",_max_level_sleep, &sucess_parse);
    printf("Estado =\n%d\n",sucess_parse);
    if(sucess_parse == 1) {
        _max_level_sleep = _max; 
        printf("- Se actualizado el valor MAXIMO a =\n\t%.2f\n",_max_level_sleep);
    }

    _min = get_json_value(mensaje_subs,"min",_min_level_sleep, &sucess_parse);
    printf("Estado =\n%d\n",sucess_parse);
    if(sucess_parse == 1) {
        _min_level_sleep = _min; 
        printf("- Se actualizado el valor MINIMO a =\n\t%.2f\n",_min_level_sleep);
    }

    printf("MAX = %.2f - MIN = %.2f\n",_max,_min);

    // Lecutra SENSOR
    ESP_LOGI(TAG, ": Activando modulo RS485 ...  \n");
    level = get_sensor_data(1, 1, buffer_rs485);

    char* msg_mqtt   = (char*) malloc(256);
    sprintf(mensaje_subs,"%.2f",level);
    strcat(m95.topic,"/altura");

    // Publicamos el valor leido
    for(int m=0; m<3; m++){
        if( M95_PubMqtt_data((uint8_t*)mensaje_subs,m95.topic,strlen(mensaje_subs),0) ){
            printf("\t... PUBLICADO \n");
            break;
        }
        activate_pin(LED_UART_BLINK);
        activate_pin(LED_READY);
        vTaskDelay( 1500 / portTICK_PERIOD_MS );
        deactivate_pin(LED_UART_BLINK);
        deactivate_pin(LED_READY);
        vTaskDelay( 1500 / portTICK_PERIOD_MS );
    }

    // Evaluamos el valor de la altura
    printf("Altura = %.2f\n",level);
    if( (level >  _max) || (level < _min)){
        ESP_LOGE("WARNING", "Niveles fuera del rango aceptable\n");
        gpio_alarm = 1;
        gpio_set_level(OUT_ALARM,1);    
    }
    else{
        ESP_LOGI("OK","Niveles dentro del rango\n");
        gpio_set_level(OUT_ALARM,0);
        gpio_alarm = 0;
    }
    
    disconnect_mqtt();

    ESP_LOGI(TAG,"Apagando Modem y pines\n");
    if( M95_poweroff_command() > 2 ){
            M95_poweroff();
    };

    activate_pin(RS485_ENABLE_PIN);
    power_off_leds();

	gpio_hold_en(RS485_ENABLE_PIN);
    gpio_hold_en(PWRKEY_Pin);
    gpio_hold_en(OUT_ALARM);

    free(msg_mqtt);
    free(buffer_rs485);

    gpio_deep_sleep_hold_en();
    esp_sleep_pd_config(ESP_PD_DOMAIN_RTC_PERIPH, ESP_PD_OPTION_ON);

    //uint64_t _time_sleep = TIME_SLEEP * MIN_TO_S * S_TO_US - esp_timer_get_time();
    uint64_t _time_sleep = (int)(3 * 60) * S_TO_US;
    float time_min = _time_sleep / (S_TO_US);
    printf("\nSleep Mode = %0.2f min\n", (float)(time_min/60.0));

    if(gpio_alarm == 1){
        esp_sleep_enable_timer_wakeup(_time_sleep/2);
    }
    else{
        esp_sleep_enable_timer_wakeup(_time_sleep);
    }
    
    esp_deep_sleep_start();

    vTaskDelete(NULL);
}


/*
static void _main_task(){
    
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
    vTaskDelay( 300 / portTICK_PERIOD_MS );
    
    // Lectura SENSOR 2
    ESP_LOGI(TAG," Leyendo sensor 2");
    int result2 = get_sensor_n_data(2,data2,param_initial,numero_param,buffer_rs485);
    vTaskDelay( 300 / portTICK_PERIOD_MS );

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
            sprintf(string_temp,"#%.1f",(float)( data1[i] / factors[i] ));
            strcat(msg_mqtt,string_temp);
        }
    }
    strcat(msg_mqtt,"\r\n");
    if(result2 != 1){
        for(int i = 0; i < numero_param; i++){
            sprintf(string_temp,"#%.1f",-99.9);
            strcat(msg_mqtt,string_temp);
        }
    }
    else{
        for(int i = 0; i < numero_param; i++){
            sprintf(string_temp,"#%.1f",(float)( data2[i] / factors[i] ));
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
            printf("\t... Sucess\n");
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
    power_off_leds();

	gpio_hold_en(RS485_ENABLE_PIN);
    gpio_hold_en(PWRKEY_Pin);
    gpio_deep_sleep_hold_en();
    esp_sleep_pd_config(ESP_PD_DOMAIN_RTC_PERIPH, ESP_PD_OPTION_ON);
    printf("\nSleep Mode = %d min\n",(int)(TIME_SLEEP));
    //esp_sleep_enable_timer_wakeup(TIME_SLEEP * MIN_TO_S * S_TO_US- esp_timer_get_time());
    esp_sleep_enable_timer_wakeup( 2 * 60 * S_TO_US );
    esp_deep_sleep_start();

    vTaskDelete(NULL);
}
*/

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
		ESP_LOGI(OTA_TAG,"No se conecto al servidor\r\n");
		TCP_close();
		printf("OTA:Desconectado\r\n");
		break;
	  }

	  ESP_LOGI(OTA_TAG,"Solicitando actualizacion...\r\n");
	  if(TCP_send(output, strlen(output))){                           // 1. Se envia la info del dispositivo
		
        printf("\t OTA : Esperando respuesta...\r\n");
		readAT("}\r\n", "-8/()/(\r\n",15000,buffer);   // 2. Se recibe la 1ra respuesta con ota True si tiene un ota pendiente... (el servidor lo envia justo despues de recibir la info)(}\r\n para saber cuando llego la respuesta)
		
        debug_ota("main> repta %s\r\n", buffer);

		if(strstr(buffer,"\"ota\": \"true\"") != 0x00  ){
            repuesta_ota = true;
			
            ESP_LOGI(OTA_TAG,"Iniciando OTA");
			//printf("Watchdog desactivado\r\n");
			//watchdog_en = 0;

///////// REVISAR
			if(ota_uartControl_M95() == OTA_EX_OK){
			    debug_ota("main> OTA m95 Correcto...\r\n");
                esp_restart();    
			}
			else{
			  debug_ota("main> OTA m95 Error...\r\n");
			}
			printf("Watchdog reactivado\r\n");
		}
		ESP_LOGE("OTA ERROR","No hubo respuesta\r\n");
	  }

      vTaskDelay(2000);
	}
	while(!repuesta_ota&(intentos<5));
}

void app_main(void)
{
    printf("---- Iniciando programa ... \n");
    mode = 0;
    esp_sleep_wakeup_cause_t wakeup_reason;
    wakeup_reason = esp_sleep_get_wakeup_cause();
    switch(wakeup_reason){
        case ESP_SLEEP_WAKEUP_TIMER: 
            printf("\t... Comenzando del reinicio\n");
            if(gpio_alarm == 1) mode = 1;
            gpio_hold_dis(PWRKEY_Pin);
            gpio_hold_dis(RS485_ENABLE_PIN);
            gpio_hold_dis(OUT_ALARM);
            break;
        default: printf("\t\t ----------------------- \n"); 
                 break;
    }

    // ---------------------------------------------------------------
    ESP_LOGI("\nInit","Begin Configuration:\nPins Mode Input, Output, ADC\n");
    config_pin_esp32();
    init_project_config();
    rs485_config();
    if(mode != 1){
        m95_config();
        xTaskCreate(M95_rx_event_task, "M95_rx_event_task", 4096, NULL, 12, NULL);
        M95_checkpower();
        M95_begin();
    }
    ESP_LOGI(TAG,"Configuracion Finalizada\n");
    ESP_LOGI(TAG,"Iniciando tarea \n");
    xTaskCreate(_main_task, "_main_task", SENSOR_TASK_STACK_SIZE, NULL, 9, NULL);
}

void init_project_config(){
    // GPIO PIN CONFIGURATION
    gpio_reset_pin(OUT_ALARM);
    gpio_reset_pin(INPUT_PULL);
    ESP_ERROR_CHECK(gpio_pulldown_en(INPUT_PULL));

    gpio_set_direction(OUT_ALARM, GPIO_MODE_OUTPUT);    // Sirena power
    gpio_set_level(OUT_ALARM,mode);

    gpio_set_direction(INPUT_PULL, GPIO_MODE_INPUT);      // Desactiva Circulina
}

void m95_config(){
	// GPIO PIN CONFIGURATION
	gpio_reset_pin(STATUS_Pin);
	gpio_reset_pin(PWRKEY_Pin);
    ESP_ERROR_CHECK(gpio_pulldown_en(STATUS_Pin));
    gpio_set_direction(STATUS_Pin, GPIO_MODE_INPUT);
    gpio_set_direction(PWRKEY_Pin, GPIO_MODE_OUTPUT);

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