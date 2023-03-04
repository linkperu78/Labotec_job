#include "esp32_general.h"

void activate_pin(int pin_number){
	gpio_set_level(pin_number, 1);	
	vTaskDelay(10 / portTICK_PERIOD_MS);	
}

void deactivate_pin(int pin_number){
	gpio_set_level(pin_number, 0);	
	vTaskDelay(10 / portTICK_PERIOD_MS);	
}

float read_battery(){
    int value_bat   	= 0;
    float repeat      	= 8.0;
    float _factor   	= 0.081;
    float _offset   	= 150.47;
	for(int i = 0; i < repeat; i++){
        value_bat += adc1_get_raw(ADC1_CHANNEL_4);
        vTaskDelay(200 / portTICK_PERIOD_MS);
    }
    float raw_volt = (float)value_bat/repeat;
    return (raw_volt*_factor + _offset)/100;
}

void config_pin_esp32(){
    //esp_adc_cal_characteristics_t adc1_chars;

    // // Leds for debug
    // gpio_pad_select_gpio(ESP_LED_PIN);
    // gpio_pad_select_gpio(ESP_ERROR_PIN);
    // gpio_pad_select_gpio(ESP_READY_PIN);
    gpio_reset_pin(ESP_LED_PIN);
    gpio_reset_pin(ESP_ERROR_PIN);
    gpio_reset_pin(ESP_READY_PIN);
    gpio_set_direction(ESP_LED_PIN, GPIO_MODE_OUTPUT);
    gpio_set_direction(ESP_ERROR_PIN, GPIO_MODE_OUTPUT);
    gpio_set_direction(ESP_READY_PIN, GPIO_MODE_OUTPUT);

    // Battery_level
    ESP_ERROR_CHECK(adc1_config_width(ADC_WIDTH_BIT_DEFAULT));
    ESP_ERROR_CHECK(adc1_config_channel_atten(ADC1_CHANNEL_4, ADC_ATTEN_DB_11));    // 3.9 V range
    

    // Power init pins
    power_off_leds();
}

void power_off_leds(){
    deactivate_pin(ESP_ERROR_PIN);
    deactivate_pin(ESP_LED_PIN);
    deactivate_pin(ESP_READY_PIN);
}


