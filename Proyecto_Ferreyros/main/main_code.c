#include "esp32_general.h"
#include "M95_uart.h"

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
    config_as_server("entel.pe");



    for(;;){
        vTaskDelay( 100 / portTICK_PERIOD_MS );
    }
    vTaskDelete(NULL);
}

void app_main(void)
{
    printf("---- Iniciando programa ... \n");
    // ---------------------------------------------------------------
    ESP_LOGI("\nInit","Begin Configuration:\nPins Mode Input, Output, ADC\n");
    config_leds_esp32();
    m95_config();
    // ---------------------------------------------------------------
    xTaskCreate(M95_rx_event_task, "M95_rx_event_task", 4096, NULL, 12, NULL);
    M95_checkpower();
    M95_begin();
    // ---------------------------------------------------------------
    xTaskCreate(_main_task, "_main_task", SENSOR_TASK_STACK_SIZE, NULL, 15, NULL);
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