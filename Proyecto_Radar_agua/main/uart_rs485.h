#ifndef __RS485_ESP32_
//--------------------------------------------------
#define __RS485_ESP32_
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
#include "esp_timer.h"

#define UART_RS485          UART_NUM_1

// Puerto Serial N1 -> UART a RS485
#define RS485_TXD           GPIO_NUM_4      
#define RS485_RXD           GPIO_NUM_15 

// RTS for RS485 Half-Duplex Mode manages DE/~RE
#define RS485_ENABLE_PIN    GPIO_NUM_22
#define RS485_RTS           GPIO_NUM_2

//#define BAUD_RATE_RS485     4800  // Defecto baudrate = 4800
#define BAUD_RATE_RS485     9600

// CTS is not used in RS485 Half-Duplex Mode
#define RS485_CTS           GPIO_NUM_23         // Seleccione un PIN que no este en uso


void rs485_config();
void update_checksum(uint8_t data_b[], int size_b, uint8_t *result);
float byte_to_int_rs485(uint8_t *bytes_t);
int send_command(uint8_t *command, int length, bool enable_checksum);
void copy_bytes(const uint8_t* source, uint8_t* destiny, size_t len);
int get_sensor_n_data(int sensor_id,float* output, int data_i, int data_f, uint8_t* buffer);
float get_sensor_data(int sensor_id, int index_data, uint8_t* buffer);

//--------------------------------------------------
#endif // __RS485_ESP32_


