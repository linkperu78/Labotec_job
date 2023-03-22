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
#define TIME_SLEEP                      5                   // Time in "min"
#define TAG                             "Radar/"        // Nombre del TAG

#define BUF_SIZE_MODEM                  (1024)
#define RD_BUF_SIZE                     (BUF_SIZE_MODEM)

// Define PARAMETERS OF PROJECT
#define OUT_ALARM       GPIO_NUM_21
#define OUT_LED         GPIO_NUM_18
#define INPUT_PULL      GPIO_NUM_13

#define MAX_LEVEL_DEFAULT   18.6
#define MIN_LEVEL_DEFAULT   11.2


cJSON *doc;
char * output;

static QueueHandle_t uart_modem_queue;
int rxBytesModem;

int wake_count;
int flag_event_serial = 0;

uint8_t rx_modem_ready;
uint8_t * p_RxModem;
bool ota_debug = false;

void OTA_check(void);
void init_project_config();

uint8_t* buffer_rs485;              // buffer para leer datos del RS485
// RTC_DATA_ATTR int gpio_alarm;
RTC_DATA_ATTR float _max_level_sleep = MAX_LEVEL_DEFAULT;
RTC_DATA_ATTR float _min_level_sleep = MIN_LEVEL_DEFAULT;
RTC_DATA_ATTR int status_led = 0;

int end_task_uart_m95;

//float factors[7] = {10.0 , 10.0 , 1.0 , 10.0 , 1.0 , 1.0 , 1.0};
 
struct datos_modem{
    char    IMEI[25];
    char    topic[50];
    char    ota[50];
    int     signal;
    float   battery;
    char    fecha[30];
};

struct datos_modem m95 = {  .IMEI = "0", .topic = TAG, .ota = TAG,
                            .signal = 99, .battery = 0.00,
                            .fecha = "01/01/23,00:00:00"};


static void M95_rx_event_task(void *pvParameters){
    uart_event_t event;
    uint8_t* dtmp = (uint8_t*) malloc(RD_BUF_SIZE);
    p_RxModem = dtmp;
    int ring_buff_len;
    for(;;) {
        //Waiting for UART event.
        if(xQueueReceive(uart_modem_queue, (void * )&event, (TickType_t )portMAX_DELAY)) {
            if(end_task_uart_m95 > 0){
                break;
            }
            switch(event.type) {
                case UART_DATA:

                    if(event.size >= 120){
                        ESP_LOGI(TAG, "OVER [UART DATA]: %d", event.size);
                        vTaskDelay(100/portTICK_PERIOD_MS);
                    }
                    if(rx_modem_ready == 0){
                        bzero(dtmp, RD_BUF_SIZE);
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
    bool mode_light_sleep = false;

    //char* msg_mqtt   = (char*) malloc(256); 
    char* mensaje_recv = (char*) malloc(256); 

    // Obtenemos el IMEI
    strcpy(m95.IMEI,get_M95_IMEI());
    // Seteamos el topico donde se publicara los resultados
    strcat(m95.topic,m95.IMEI);
    // Seteamos el topico donde consultaremos el estado del OTA
    strcpy(m95.ota,m95.topic);
    strcat(m95.ota,"/OTA");
   
    //printf("Topic direction = %s\n",m95.topic);

    // El mensaje que se va a enviar al broker, requiere de:
    // Sensor de distancia - Fecha - Nivel de Bateria - Nivel de señal
    // Fecha
    strcpy(m95.fecha,get_m95_date());
    // Nivel de bateria
    m95.battery = read_battery();
    ESP_LOGI(TAG,"Nivel de bateria = %.2f\n", m95.battery);
    // Nivel de señal
    m95.signal= get_M95_signal();

    connect_MQTT_server(0);

    // Activamos el RS485 aqui para darle tiempo al sensor para inicializar correctamente
    ESP_LOGI(TAG, ": Activando modulo RS485 ...  \n");
    deactivate_pin(RS485_ENABLE_PIN);

    // Actualizamos el valor del max y min de los niveles
    wake_count = m95_sub_topic_json(0,m95.topic,mensaje_recv);

    int sucess_parse = 0;
    _max = get_json_value(mensaje_recv,"max",_max_level_sleep, &sucess_parse);
    if(sucess_parse == 1) {
        _max_level_sleep = _max; 
        printf("\n- Se actualizado el valor MAXIMO a =\n\t%.2f\n",_max_level_sleep);
    }

    _min = get_json_value(mensaje_recv,"min",_min_level_sleep, &sucess_parse);
    if(sucess_parse == 1) {
        _min_level_sleep = _min; 
        printf("\n- Se actualizado el valor MINIMO a =\n\t%.2f\n",_min_level_sleep);
    }

    printf("MAX = %.2f - MIN = %.2f\n",_max,_min);
    //m95_unsub_topic(0,m95.topic);

    // ------------------ Lecutra SENSOR ---------------------------
    level = get_sensor_data_rs485(1, 1, buffer_rs485);
    sprintf(mensaje_recv,"%s\r\nS=%d,L=%.2f,V=%.2f",m95.fecha,m95.signal,level,m95.battery);
    ESP_LOGI("PUB MQTT","Mensaje a publicar:\n%s\n",mensaje_recv);
    
    strcat(m95.topic,"/altura");        // EN Radar/IMEI#/ se setearan los valores
                                        // max y min en formato json
    // Publicamos el valor leido
    for(int m=0; m<3; m++){
        if( M95_PubMqtt_data((uint8_t*)mensaje_recv,m95.topic,strlen(mensaje_recv),0) ){
            printf("\t... PUBLICADO \n");
            break;
        }
        activate_pin(LED_UART_BLINK);
        activate_pin(LED_READY);
        vTaskDelay( 500 / portTICK_PERIOD_MS );
        deactivate_pin(LED_UART_BLINK);
        deactivate_pin(LED_READY);
        vTaskDelay( 500 / portTICK_PERIOD_MS );
        M95_CheckConnection();
    }
    activate_pin(RS485_ENABLE_PIN);

    // Evaluamos el valor de la altura
    ESP_LOGI("\nRS485 Sensor","\t Altura = %.2f\n\n",level);
    if( (level >  _max) || (level < _min)){
        ESP_LOGE("WARNING", "Niveles fuera del rango aceptable\n");
        mode_light_sleep = true;
        status_led = 1;
        gpio_set_level(OUT_ALARM,1);
        gpio_set_level(OUT_LED,1);
    }
    else{
        ESP_LOGI("OK","Niveles dentro del rango\n");
        status_led = 0;
        gpio_set_level(OUT_ALARM,0);
        gpio_set_level(OUT_LED,0);
    }
    
    // Chequeamos si el OTA es requerido
    wake_count = m95_sub_topic_OTA(0,m95.ota,mensaje_recv);
    if( wake_count == 1){
        wake_count = 0;
        if(strstr(mensaje_recv,"1")){
            wake_count = 1;
        }
    }
    ESP_LOGI(TAG,"\n\tEstado del ota = \" %s \"\n",(wake_count == 1 ? "Requerido":"No requerido"));
    deactivate_pin(WHITE_LED);
    disconnect_mqtt();

    // ---------------------------------------------------------------------------
    // ---------------------------------------------------------------------------
    if(wake_count == 1){
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
    if( M95_poweroff_command() > 2 ){
            M95_poweroff();
    };

    power_off_leds();

    free(mensaje_recv);
    free(buffer_rs485);

    int64_t _time_sleep = TIME_SLEEP * MIN_TO_S * S_TO_US - (int64_t)esp_timer_get_time();
    if(_time_sleep < (1 * S_TO_US) ){
        _time_sleep = 1 * S_TO_US;
    }
    //uint64_t _time_sleep = (int)(3 * 60) * S_TO_US;
    int time_sec = _time_sleep / (S_TO_US);
    int time_min = time_sec / 60 ;
    time_sec = time_sec - time_min * 60;
    printf("\nSleep Mode = %d min y %d sec\n", time_min,time_sec);

    // Si la alarma esta encendida hacemos un light_sleep mode
    // donde podamos recibir interrupcion y leer el tiempo que ha pasado
    if(mode_light_sleep){
        // Terminamos la tarea de uart m95
        end_task_uart_m95 = 1;
        // Configuramos los metodos para despertar al ESP32
        uint64_t time_before_sleep = esp_timer_get_time();      // Obtenemos el tiempo antes de dormir
        esp_sleep_disable_wakeup_source(ESP_SLEEP_WAKEUP_ALL);
        esp_sleep_enable_ext0_wakeup(INPUT_PULL, 0);            // Se despierta si detecta un valor bajo
        esp_sleep_enable_timer_wakeup(_time_sleep);    // Se despierta despues de TIME_SLEEP segundos
        
        ESP_LOGI("Sleep","Comenzamos a dormir en modo ligero\n");
        vTaskDelay( 100 / portTICK_PERIOD_MS );

        // ------------------------------------------------------------------------------
        esp_light_sleep_start();        // Funcion para dormir el ESP32 en modo ligero

        int64_t remain_time_sleep = _time_sleep + time_before_sleep - esp_timer_get_time();
        ESP_LOGI("Sleep","Quedaban %llu segundos para despertar\n",( remain_time_sleep / S_TO_US ) );
        if( remain_time_sleep < 200000){
            printf("Desperte por timer, reiniciando ... \n");
            esp_restart();
        }
        printf("Desperte por interrupccion ... \n");
        gpio_set_level(OUT_ALARM,0);
        esp_sleep_disable_wakeup_source(ESP_SLEEP_WAKEUP_ALL);
        _time_sleep = remain_time_sleep;
    }

    gpio_hold_en(RS485_ENABLE_PIN);
    gpio_hold_en(PWRKEY_Pin);
    gpio_hold_en(OUT_ALARM);
    gpio_hold_en(OUT_LED);
    gpio_deep_sleep_hold_en();
    
    ESP_LOGI("Sleep","Comenzamos a dormir en modo profundo\n");
    vTaskDelay( 100 / portTICK_PERIOD_MS );
    esp_sleep_enable_timer_wakeup(_time_sleep);
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
    end_task_uart_m95 = 0;
    esp_sleep_wakeup_cause_t wakeup_reason;
    wakeup_reason = esp_sleep_get_wakeup_cause();
    switch(wakeup_reason){
        case ESP_SLEEP_WAKEUP_TIMER: 
            printf("\t... Comenzando del reinicio\n");
            gpio_hold_dis(PWRKEY_Pin);
            gpio_hold_dis(RS485_ENABLE_PIN);
            gpio_hold_dis(OUT_ALARM);
            gpio_hold_dis(OUT_LED);
            break;
        default: printf("\t\t ----------------------- \n"); 
                 break;
    }
    init_project_config();
    // ---------------------------------------------------------------
    ESP_LOGI("\nInit","Begin Configuration:\nPins Mode Input, Output, ADC\n");
    config_pin_esp32();
    rs485_config();
    m95_config();
    // ---------------------------------------------------------------
    xTaskCreate(M95_rx_event_task, "M95_rx_event_task", 4096, NULL, 12, NULL);
    M95_checkpower();
    M95_begin();
    // ---------------------------------------------------------------
    ESP_LOGI(TAG,"Configuracion Finalizada\n");
    ESP_LOGI(TAG,"Iniciando tarea \n");
    xTaskCreate(_main_task, "_main_task", SENSOR_TASK_STACK_SIZE, NULL, 15, NULL);
}

void init_project_config(){
    // ----------- Configuramos los pines ---------------
    // Salida de sirena     OUT_ALARM
    // Salida de alarma     OUT_SIRENA     - No implementado
    // Apagado de sirena    INPUT_PULL
    gpio_reset_pin(OUT_ALARM);
    gpio_reset_pin(OUT_LED);
    
    gpio_set_direction( OUT_ALARM , GPIO_MODE_OUTPUT );
    gpio_set_direction( OUT_LED , GPIO_MODE_OUTPUT );

    if(status_led == 0){
        ESP_LOGI(TAG," - Nivel dentro del rango - \n");
        gpio_set_level( OUT_ALARM , 0 );
        gpio_set_level( OUT_LED , 0 );
    }
    else{
        ESP_LOGE(TAG," - Nivel fuera del rango - \n");
        gpio_set_level( OUT_LED , 1 );
    }

    gpio_reset_pin(INPUT_PULL);
    gpio_pulldown_dis(INPUT_PULL);
    gpio_pullup_en(INPUT_PULL);
    gpio_set_direction(INPUT_PULL,GPIO_MODE_INPUT);

    // ----------- Configure light sleep mode --------------------
    esp_sleep_pd_config(ESP_PD_DOMAIN_RTC_PERIPH, ESP_PD_OPTION_ON);
    esp_sleep_pd_config(ESP_PD_DOMAIN_RTC_SLOW_MEM, ESP_PD_OPTION_ON);
    //esp_sleep_pd_config(ESP_PD_DOMAIN_RTC_FAST_MEM, ESP_PD_OPTION_ON);
    
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