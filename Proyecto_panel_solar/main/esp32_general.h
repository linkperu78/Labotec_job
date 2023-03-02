#ifndef __GENERAL_ESP32_
//--------------------------------------------------
#define __GENERAL_ESP32_

#include <string.h>
#include <stdint.h>
#include <stdio.h>
#include <stdlib.h>
#include <sys/time.h>
#include "freertos/FreeRTOS.h"
#include "freertos/task.h"
#include "driver/gpio.h"
#include "driver/uart.h"
#include "esp_sleep.h"
#include "driver/adc.h"
#include "esp_adc_cal.h"

#define RED_LED         GPIO_NUM_23
#define GREEN_LED       GPIO_NUM_22
#define WHITE_LED       GPIO_NUM_21
#define BATERY_LEVEL    GPIO_NUM_32
 
#define ESP_LED_PIN         GREEN_LED
#define ESP_ERROR_PIN       RED_LED
#define ESP_READY_PIN       WHITE_LED
#define ESP_BAT_PIN         BATERY_LEVEL

#define BUF_SIZE 			(512)
#define BUF_SIZE_RS485      100
#define PACKET_READ_TICS 	(100 / portTICK_PERIOD_MS)

// Timeout threshold for UART = number of symbols (~10 tics) with unchanged state on receive pin
#define ECHO_READ_TOUT          (3) // 3.5T * 8 = 28 ticks, TOUT=3 -> ~24..33 ticks

#define S_TO_US         1000000
#define MIN_TO_S        60

void activate_pin(int pin_number);

void deactivate_pin(int pin_number);

float read_battery();

void config_pin_esp32();

void power_off_leds();
//--------------------------------------------------
#endif /* __GENERAL_ESP32_ */
