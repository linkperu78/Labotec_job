#include "esp32_general.h"
#include "M95_uart.h"

#include <stdio.h>
#include <string.h>
#include <stdlib.h>
#include <time.h>
#include <cjson.h>

#include "freertos/FreeRTOS.h"
#include "freertos/task.h"
#include "esp_system.h"
#include "nvs_flash.h"
#include "driver/uart.h"
#include "freertos/queue.h"
#include "esp_log.h"
#include "sdkconfig.h"


// Read packet timeout
#define SENSOR_TASK_STACK_SIZE          (4096)
#define MODEM_TASK_STACK_SIZE           (2048)
#define SENSOR_TASK_PRIO                10
#define MODEM_TASK_PRIO                 9
#define TIME_SLEEP                      5                   // Time in "min"
#define TAG                             "Project_name"        // Nombre del TAG

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


// Tarea principal donde leemos y enviamos los datos a la database
static void _main_task(){
    vTaskDelete(NULL);
}


void app_main(void)
{
    xTaskCreate(_main_task, "_main_task", SENSOR_TASK_STACK_SIZE, NULL, 15, NULL);
}