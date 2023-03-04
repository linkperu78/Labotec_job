#include "uart_rs485.h"
#include "esp32_general.h"

// Base de datos para checksum rs485
uint16_t    CrcTable[] = {
        0X0000, 0XC0C1, 0XC181, 0X0140, 0XC301, 0X03C0, 0X0280, 0XC241,
        0XC601, 0X06C0, 0X0780, 0XC741, 0X0500, 0XC5C1, 0XC481, 0X0440,
        0XCC01, 0X0CC0, 0X0D80, 0XCD41, 0X0F00, 0XCFC1, 0XCE81, 0X0E40,
        0X0A00, 0XCAC1, 0XCB81, 0X0B40, 0XC901, 0X09C0, 0X0880, 0XC841,
        0XD801, 0X18C0, 0X1980, 0XD941, 0X1B00, 0XDBC1, 0XDA81, 0X1A40,
        0X1E00, 0XDEC1, 0XDF81, 0X1F40, 0XDD01, 0X1DC0, 0X1C80, 0XDC41,
        0X1400, 0XD4C1, 0XD581, 0X1540, 0XD701, 0X17C0, 0X1680, 0XD641,
        0XD201, 0X12C0, 0X1380, 0XD341, 0X1100, 0XD1C1, 0XD081, 0X1040,
        0XF001, 0X30C0, 0X3180, 0XF141, 0X3300, 0XF3C1, 0XF281, 0X3240,
        0X3600, 0XF6C1, 0XF781, 0X3740, 0XF501, 0X35C0, 0X3480, 0XF441,
        0X3C00, 0XFCC1, 0XFD81, 0X3D40, 0XFF01, 0X3FC0, 0X3E80, 0XFE41,
        0XFA01, 0X3AC0, 0X3B80, 0XFB41, 0X3900, 0XF9C1, 0XF881, 0X3840,
        0X2800, 0XE8C1, 0XE981, 0X2940, 0XEB01, 0X2BC0, 0X2A80, 0XEA41,
        0XEE01, 0X2EC0, 0X2F80, 0XEF41, 0X2D00, 0XEDC1, 0XEC81, 0X2C40,
        0XE401, 0X24C0, 0X2580, 0XE541, 0X2700, 0XE7C1, 0XE681, 0X2640,
        0X2200, 0XE2C1, 0XE381, 0X2340, 0XE101, 0X21C0, 0X2080, 0XE041,
        0XA001, 0X60C0, 0X6180, 0XA141, 0X6300, 0XA3C1, 0XA281, 0X6240,
        0X6600, 0XA6C1, 0XA781, 0X6740, 0XA501, 0X65C0, 0X6480, 0XA441,
        0X6C00, 0XACC1, 0XAD81, 0X6D40, 0XAF01, 0X6FC0, 0X6E80, 0XAE41,
        0XAA01, 0X6AC0, 0X6B80, 0XAB41, 0X6900, 0XA9C1, 0XA881, 0X6840,
        0X7800, 0XB8C1, 0XB981, 0X7940, 0XBB01, 0X7BC0, 0X7A80, 0XBA41,
        0XBE01, 0X7EC0, 0X7F80, 0XBF41, 0X7D00, 0XBDC1, 0XBC81, 0X7C40,
        0XB401, 0X74C0, 0X7580, 0XB541, 0X7700, 0XB7C1, 0XB681, 0X7640,
        0X7200, 0XB2C1, 0XB381, 0X7340, 0XB101, 0X71C0, 0X7080, 0XB041,
        0X5000, 0X90C1, 0X9181, 0X5140, 0X9301, 0X53C0, 0X5280, 0X9241,
        0X9601, 0X56C0, 0X5780, 0X9741, 0X5500, 0X95C1, 0X9481, 0X5440,
        0X9C01, 0X5CC0, 0X5D80, 0X9D41, 0X5F00, 0X9FC1, 0X9E81, 0X5E40,
        0X5A00, 0X9AC1, 0X9B81, 0X5B40, 0X9901, 0X59C0, 0X5880, 0X9841,
        0X8801, 0X48C0, 0X4980, 0X8941, 0X4B00, 0X8BC1, 0X8A81, 0X4A40,
        0X4E00, 0X8EC1, 0X8F81, 0X4F40, 0X8D01, 0X4DC0, 0X4C80, 0X8C41,
        0X4400, 0X84C1, 0X8581, 0X4540, 0X8701, 0X47C0, 0X4680, 0X8641,
        0X8201, 0X42C0, 0X4380, 0X8341, 0X4100, 0X81C1, 0X8081, 0X4040 };

/** 
    @brief  Configuramos los pines del ESP para comunicarse con el
            modulo RS485 (baudrate, serial port number, parity char)
 */
void rs485_config(){
    // GPIO RS485 CONFIGURATION
    //gpio_pad_select_gpio(RS485_ENABLE_PIN);
    gpio_reset_pin(RS485_ENABLE_PIN);
    gpio_set_direction(RS485_ENABLE_PIN, GPIO_MODE_OUTPUT);

    // SERIAL PORT RS485 CONFIGURATION
    const int uart_rs485    = UART_RS485;

    uart_config_t uart_config_rs485 = {
        .baud_rate = BAUD_RATE_RS485,
        .data_bits = UART_DATA_8_BITS,
        .parity = UART_PARITY_DISABLE,
        .stop_bits = UART_STOP_BITS_1,
        .flow_ctrl = UART_HW_FLOWCTRL_DISABLE,
        .rx_flow_ctrl_thresh = 122,
        .source_clk = UART_SCLK_APB,
    };
    // Install UART driver (we don't need an event queue here)
    ESP_ERROR_CHECK(uart_driver_install(uart_rs485, BUF_SIZE * 2, 0, 0, NULL, 0));
    // Configure UART parameters
    ESP_ERROR_CHECK(uart_param_config(uart_rs485, &uart_config_rs485));
    // Set UART pins as per KConfig settings
    ESP_ERROR_CHECK(uart_set_pin(uart_rs485, RS485_TXD, RS485_RXD, RS485_RTS, RS485_CTS));
    // Set RS485 half duplex mode
    ESP_ERROR_CHECK(uart_set_mode(uart_rs485, UART_MODE_RS485_HALF_DUPLEX));
    // Set read timeout of UART TOUT feature
    ESP_ERROR_CHECK(uart_set_rx_timeout(uart_rs485, ECHO_READ_TOUT));

    // RS485 Module start: OFF  
    activate_pin(RS485_ENABLE_PIN);
}

/** 
    @brief Genera el checksum del protocolo rs485 - en base al
            array de elementos 0xXX; /data_b y de tamaño /size_b
            y guarda esos valores en /result  
 */
void update_checksum(uint8_t *data_b, int size_b, uint8_t *result)
{
    uint16_t crc = 0xFFFF;
    uint8_t temp_data;
    //uint8_t new_data[size_b +2];
    for(int i=0; i<size_b ; i++)
    {
        temp_data = data_b[i];
        //new_data[i] = temp_data;
        int pos = (crc ^ temp_data) & 0xFF;
        crc = (crc >> 8)^CrcTable[pos];
    }
    uint8_t low_cs=(uint8_t)(crc%256);
    uint8_t high_cs=(uint8_t)(crc/256);

    result[0] = low_cs;
    result[1] = high_cs;
}

/** 
    @brief  Calcula el valor del parametro en bytes,
            convirtiendo los bytes en un valor numerico
    @return valor con decimal (inc. negativos)
 */
float byte_to_int_rs485(uint8_t *bytes_t){
    /*
        i.e:
        send: 0x01 0x03 0x00 0x00 0x00 0x03 0x05 0xcb
        reci: 0x01 0x03 0x06 0x02 0x92 0xFF 0x9b 0x03 0xE8 cs_hb cs_lb
    */

	uint8_t msb = bytes_t[0];
	float valor_actual = (float)(bytes_t[0]*256+bytes_t[1]);
	valor_actual = valor_actual;
	if( msb > ((uint8_t)(0x7F)) )
	{
		valor_actual = valor_actual - 65536.0;		
	}
	return valor_actual;
}

int send_command_rs485(uint8_t *command, int length, bool enable_checksum, uint8_t *response_rs485){
    uint8_t check_sum[2];
    uint _delay_max = 600 * 1000; // us
    update_checksum(command,length,check_sum);
    uart_flush(UART_RS485);
    vTaskDelay(50/ portTICK_PERIOD_MS);

    /*
    for(int i = 0; i < length; i++){
        printf("0x%02x ",command[i]);
    }
    for(int i = 0; i<2; i++){
        printf("0x%02x ",check_sum[i]);
    }
    printf("\n");
    */

    uart_write_bytes(UART_RS485, command, length);
    uart_write_bytes(UART_RS485, check_sum, 2);
    vTaskDelay(10/ portTICK_PERIOD_MS);
    int length_buffer = 0;
    uint64_t actual_time = esp_timer_get_time();
    uint64_t limit_time = actual_time + _delay_max;
    while((length_buffer < length) && (actual_time < limit_time))
    {   
        uart_get_buffered_data_len(UART_RS485,(size_t*)&length_buffer);
        //printf("Respuesta = %d bytes \n", length_buffer);
        vTaskDelay(10/ portTICK_PERIOD_MS);
        actual_time = esp_timer_get_time();
    }
    if(actual_time >= limit_time){
        printf("Sensor RS485 not response ... \n");
        return 0;    
    }
    int lrs = uart_read_bytes(UART_RS485,response_rs485,BUF_SIZE_RS485,PACKET_READ_TICS);
    if(!enable_checksum){
        return 1;
    }
    printf("Checking checksum ...");
    uint8_t temp_response[lrs-2];
    copy_bytes(response_rs485,temp_response,lrs-2);
    update_checksum(temp_response,lrs-2,check_sum);
    if((check_sum[0] == response_rs485[lrs-2]) && (check_sum[1] == response_rs485[lrs-1])){
        printf(" : CORRECT \n");
        return 1;
    } 
        printf(" : INCORRECT \n");   
    return -1;
}

void copy_bytes(const uint8_t* source, uint8_t* destiny, size_t len){
    //printf("\nRespuesta RS485 =");
    int j = 0;
    for(j = 0; j < (int) len; j++){
        destiny[j] = source [j];
        //printf(" 0x%.2x",source[j]);
    }
    // printf(" 0x%.2x",source[j+1]);
    // printf(" 0x%.2x",source[j+2]);
    // printf("\n");
}

/**
    @brief Obtenemos datos despues de comunicarnos con el sensor
    @param sensor_id
**/
int get_sensor_n_data(int sensor_id, float* output, int data_i, int data_n, uint8_t* buffer){

    uint8_t input_rs485[6];
    // i.e. : 0x01 0x03 0x00 0x00 0x00 0x03 0x05 0xcb 
    input_rs485[0] = (uint8_t)sensor_id;            
    input_rs485[1] = 0x03;                  // Indicamos que queremos recopilar datos
    input_rs485[2] = 0x00;                  // 0x00 0xn    -> n posicion inicial en hex
    input_rs485[3] = (uint8_t)data_i;  
    input_rs485[4] = 0x00;                  // 0x00 0xext  -> ext cantidad de campos en hex
    input_rs485[5] = (uint8_t)data_n;

    int res = send_command_rs485(input_rs485, 6, true, buffer);
    if(res != 1){
        for(int i = 0; i < data_n; i++){
            output[i] = -99;
        }
        return res;
    }
    int n_response = (int)buffer[2]/2;
    uint8_t data_leida[2];
    data_leida[0] = 0; data_leida[1]=0;
    printf("Data : ");
    for (int i = 0; i< n_response; i++){
        data_leida[0] = buffer[i*2+3];
        data_leida[1] = buffer[i*2+4];
        output[i] = byte_to_int_rs485(data_leida);
        printf("%.1f  ",output[i]);
    }
    printf("\n\n");
    return 1;
}

float get_sensor_data(int sensor_id, int index_data, uint8_t* buffer){
    uint8_t value[2] = {0x00 , 0x00};
    uint8_t msg_485[6] = {  (uint8_t)sensor_id, 0x03, 0x00, 
                            (uint8_t)index_data, 0x00, 0x01};
    /*
    printf("msg = ");
    for(int a = 0; a < 6; a++){
        printf("0x%.2x ", msg_485[a]);
    }
    */
    int res = 0;
    res = send_command_rs485(msg_485, 6, false, buffer);
    if(res != 1) {return -99.9;}

    value[0] = buffer[3]; value[1] = buffer[4];
    return byte_to_int_rs485(value)/10;
}
