#include "esp32_general.h"

void activate_pin(int pin_number){
	gpio_set_level(pin_number, 1);	
	vTaskDelay(10 / portTICK_PERIOD_MS);	
}

void deactivate_pin(int pin_number){
	gpio_set_level(pin_number, 0);	
	vTaskDelay(10 / portTICK_PERIOD_MS);	
}

void config_pin_leds(){
    
    // // Leds for debug

    gpio_reset_pin( WHITE_LED );
    gpio_reset_pin( GREEN_LED );
    
    gpio_pullup_dis(WHITE_LED);
    gpio_pullup_dis(GREEN_LED);

    gpio_set_direction( WHITE_LED, GPIO_MODE_OUTPUT );
    gpio_set_direction( GREEN_LED, GPIO_MODE_OUTPUT );

    // Power init pins
    power_off_leds();
}

void power_off_leds(){
    deactivate_pin(GREEN_LED);
    deactivate_pin(WHITE_LED);   
}


