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


#define GREEN_LED           GPIO_NUM_25
#define WHITE_LED           GPIO_NUM_26

#define LED_READY           GREEN_LED
#define LED_UART_BLINK      WHITE_LED

#define PIN1_ADC            ADC1_CHANNEL_4


#define BUF_SIZE 			(512)
#define BUF_SIZE_RS485      100
#define PACKET_READ_TICS 	(100 / portTICK_PERIOD_MS)

// Timeout threshold for UART = number of symbols (~10 tics) with unchanged state on receive pin
#define ECHO_READ_TOUT      (3)         // 3.5T * 8 = 28 ticks, TOUT=3 -> ~24..33 ticks

#define S_TO_US         1000000
#define MIN_TO_S        60

// Set digital status to HIGH
void activate_pin(int pin_number);

// Set digital status to LOW
void deactivate_pin(int pin_number);

// Configuracion del ESP32
void config_leds_esp32();

// Apagamos los leds
void power_off_leds();

// Leemos el valor de la bateria
float read_battery();
//--------------------------------------------------
#endif /* __GENERAL_ESP32_ */
