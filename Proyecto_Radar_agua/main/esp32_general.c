#include "esp32_general.h"

void activate_pin(int pin_number){
	gpio_set_level(pin_number, 1);	
	vTaskDelay(10 / portTICK_PERIOD_MS);	
}

void deactivate_pin(int pin_number){
	gpio_set_level(pin_number, 0);	
	vTaskDelay(10 / portTICK_PERIOD_MS);	
}

int get_length(const char* str){
  int l = 0;
  while(*str != '\0'){
    l++;
    str++;
  }
  return l;
}

float get_json_value(const char *json_string, const char *char_find, float def, int *sucess){
  int len_find = get_length(char_find);
  char* p_find;
  p_find = char_find;

  int matched;
  matched = 0;

  while(*json_string != '\0'){
    if(*json_string == *p_find){
      while(*json_string == *p_find){
        if(*json_string == '\0') break;
        if(*p_find == '\0') break;
        matched++;
        json_string++;
        p_find++;
      }
    
      if(matched == len_find){
        break;
      }
      matched = 0;
      p_find = char_find;
    }
    json_string++;
  }

  if(matched != len_find){
    *sucess = 0;
    return def;
  }

  matched = 0;
  int len_float = 0;

  while(matched <3){
    if(*json_string == '\"'){
      matched++;
    }
    json_string++;
    if(*json_string == '\"'){
      continue;
    }
    if(matched == 2){
      len_float++;
    }
  }

  json_string--;
  char value[len_float];

  for(int m = 0; m<len_float; m++){
    json_string--;
    value[len_float-m-1] = *json_string;
  }

  *sucess = 1;
  printf("Resultado = %s se detecto el valor\n",(*sucess == 1? "SI":"NO"));
  return atof(value);
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
    
    // // Leds for debug

    gpio_reset_pin( WHITE_LED );
    gpio_reset_pin( GREEN_LED );
    
    gpio_pullup_dis(WHITE_LED);
    gpio_pullup_dis(GREEN_LED);

    gpio_set_direction( WHITE_LED, GPIO_MODE_OUTPUT );
    gpio_set_direction( GREEN_LED, GPIO_MODE_OUTPUT );

    // Battery_level
    ESP_ERROR_CHECK(adc1_config_width(ADC_WIDTH_BIT_DEFAULT));
    ESP_ERROR_CHECK(adc1_config_channel_atten(ADC1_CHANNEL_4, ADC_ATTEN_DB_11));    // 3.9 V range
    

    // Power init pins
    power_off_leds();
}

void power_off_leds(){
    deactivate_pin(GREEN_LED);
    deactivate_pin(WHITE_LED);   
}


