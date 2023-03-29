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
    int adc_raw       = 0;
    int repeat        = 4;
    float _factor   	= 3.23;
    float _offset   	= 0.0;
	for(int i = 0; i < repeat; i++){
        value_bat += adc1_get_raw(PIN1_ADC);
        vTaskDelay(100 / portTICK_PERIOD_MS);
    }
    float raw_volt = (float)value_bat/(float)repeat;
    return (raw_volt*_factor + _offset)/1000;
}

void config_leds_esp32(){
    
    // // Leds for debug

    gpio_reset_pin( WHITE_LED );
    gpio_reset_pin( GREEN_LED );
    
    gpio_pullup_dis(WHITE_LED);
    gpio_pullup_dis(GREEN_LED);

    gpio_set_direction( WHITE_LED, GPIO_MODE_OUTPUT );
    gpio_set_direction( GREEN_LED, GPIO_MODE_OUTPUT );

    // Battery_level
    ESP_ERROR_CHECK(adc1_config_width(ADC_WIDTH_BIT_DEFAULT));
    ESP_ERROR_CHECK(adc1_config_channel_atten(PIN1_ADC, ADC_ATTEN_DB_2_5));    // 3.9 V range

    // Power init pins
    power_off_leds();
}

void power_off_leds(){
    deactivate_pin(GREEN_LED);
    deactivate_pin(WHITE_LED);   
}


