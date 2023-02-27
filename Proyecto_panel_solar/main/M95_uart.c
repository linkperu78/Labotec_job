#include "M95_uart.h"
#include "esp32_general.h"

int debug = 0;

esp_ota_handle_t otaHandlerXXX = 0;

// Sensor buffer para guardar datos recibidos por RS485
uint8_t*  	buffer_rx_rs485;
// M95 buffer para guardar datos recibidos por el M95
char 		M95_buffer[255];
//char 		M95_buffer[500];
char* 		uart_m95_string;
// Bandera para sincronizar hora
uint8_t  	epoch_ready;
// buffer para almacenar respuesta M95 despues de publicar en MQTT
uint8_t  	buffer_m95[128];

#define BUFFER_SIZE 5140
char buffer[BUFFER_SIZE];


/** 
    @brief Encendemos el equipo mediante el PWR_PIN del M95
 */
void M95_pwron(){
    activate_pin(PWRKEY_Pin); 		
	vTaskDelay(1000 / portTICK_PERIOD_MS);
    deactivate_pin(PWRKEY_Pin); 	
	vTaskDelay(2000 / portTICK_PERIOD_MS);
	while(gpio_get_level(STATUS_Pin) == 0)
	{
		vTaskDelay(300 / portTICK_PERIOD_MS);
	}
	rx_modem_ready = 0;
	
	// Enviamos un commando cualquiera, solo queremos iniciar la
	// comunicacion con el Modem por UART
	char* com = "ATE\r\n";
	uart_write_bytes(UART_MODEM, (uint8_t *)com, strlen(com));
	// Intentos
	int a = 5;
	while(a > 0){
		if((readAT("Call Ready","Char imposible",2000,M95_buffer) == 1)){
			ESP_LOGI("M95"," ... Se inicio correctamente\n");
			break;
		}
		vTaskDelay(300 / portTICK_PERIOD_MS);
		a--;
	}
	uart_flush(UART_MODEM);
}

/** 
    @brief Apagamos el equipo mediante el PWR_PIN del M95
 */
void M95_poweroff(){
	printf("Apagando el modem ....");
	deactivate_pin(PWRKEY_Pin);
	vTaskDelay(1500 / portTICK_PERIOD_MS);
	while(gpio_get_level(STATUS_Pin) == 1){
		activate_pin(PWRKEY_Pin); 		
		vTaskDelay(800 / portTICK_PERIOD_MS);
		deactivate_pin(PWRKEY_Pin);

		while(gpio_get_level(STATUS_Pin) == 1){
			vTaskDelay(1500 / portTICK_PERIOD_MS);
			printf("... \n");
		}
		vTaskDelay(2000 / portTICK_PERIOD_MS);
	}
	vTaskDelay(1000 / portTICK_PERIOD_MS);
}


void M95_checkpower(){
    while(gpio_get_level(STATUS_Pin) == 1 ){
		printf("Power ON detected ... turning off ... \n");
        activate_pin(GREEN_LED);
        M95_poweroff();
    }
	deactivate_pin(GREEN_LED);
    printf("--------------\n");

    if( gpio_get_level(STATUS_Pin) ==0){
		printf("Encendiendo Modem\n");
        M95_pwron();
    }
}

/** 
    @brief  Apagamos el equipo mediante UART y el Comando AT "POWER OFF"
			correspondiente
 */
int M95_poweroff_command(){
	return sendAT("AT+QPOWD=0\r\n","OK\r\n","FAILED\r\n",1000,M95_buffer);
}

/** 
    @brief  Obtenemos el IMEI del M95 mediante comando AT
 */
char* get_M95_IMEI(){
	// M95_buffer -> Variable donde se almacena la respuesta del M95
	int intentos = 4;
	while(intentos > 0){
		printf("\tObteniendo IMEI ... \n");
		if(sendAT("\r\nAT+QGSN\r\n","+QGSN","ERROR",2000,M95_buffer) == 1){
			//printf("1- RESPUESTA =%s\n",(char*)M95_buffer);
			break;
		}
		//printf("2- RESPUESTA =%s\n",(char*)M95_buffer);
		vTaskDelay(1000 / portTICK_PERIOD_MS);
		intentos--;
	}
	if(intentos == 0){
		//printf("ERROR: No se obtuvo IMEI");
		return "NULL";
	}

	//printf("3- RESPUESTA =%s\n",(char*)M95_buffer);
	char lim[3] = "\"";		// lim = "
	char * p_clock = strtok(M95_buffer,lim);
	while(p_clock != NULL){
		if(strstr(p_clock,"+")==NULL && strstr(p_clock,":") == NULL){
			break;
		}
		p_clock=strtok(NULL,lim);
	}
	return p_clock;
}

/** 
    @brief  Obtenemos la calidad de señal del M95 mediante comando AT
 */
int get_M95_signal(){
	// M95_buffer -> Variable donde se almacena la respuesta del M95
	int intentos = 4;
	while(intentos > 0){
		if(sendAT("\r\nAT+CSQ\r\n","+CSQ","ERROR",500,M95_buffer) == 1){
			break;
		}
		intentos--;
	}
	if(intentos == 0){
		printf("No responde el M95");
		return 99;
	}
	//printf("RESPUESTA =%s\n",(char*)M95_buffer);
	char lim[3] = ":, ";		// lim = "
	char * p_clock = strtok(M95_buffer,lim);
	while(p_clock != NULL){
		if(strstr(p_clock,"CSQ")!=NULL){
			break;
		}
		p_clock=strtok(NULL,lim);
	}
	p_clock=strtok(NULL,lim);
	return atoi(p_clock);
}

/** 
    @brief Genera el checksum del protocolo rs485 - en base al
            array de elementos 0xXX; /data_b y de tamaño /size_b
            y guarda esos valores en /result  
 */
bool M95_PubMqtt_data(uint8_t * data,char * topic,uint16_t data_len,uint8_t tcpconnectID,int retain){
	sprintf(M95_buffer,"\r\nAT+QMTPUB=%u,1,1,%d,\"%s\",%u\r\n",tcpconnectID,retain,topic,data_len);
	sendAT(M95_buffer,">","ERROR",500,(char *)&buffer_m95);
	vTaskDelay(10 / portTICK_PERIOD_MS);
	uart_write_bytes(UART_MODEM,data,data_len);
	int64_t actual_time = esp_timer_get_time();
	int k = 0;

	while((esp_timer_get_time() - actual_time) < 3000 * 1000){
		if(!(rxBytesModem > 0)){
			continue;
		}
		memcpy(M95_buffer,p_RxModem,rxBytesModem);
		if (strstr(M95_buffer,"+QMTPUB:")!= NULL){
			if(debug == 1) printf("\t .. Public\n");
			k = 1;    
			break;
		}
	}
	uart_flush(UART_MODEM);
	return (k == 1);
}

uint8_t M95_begin(){
    int bandera=5;
	debug = 1;
	// Validate AT Commands
	sendAT("AT\r\n","OK\r\n","ERROR\r\n",1000,M95_buffer);
	// Disable local echo mode			
	sendAT("ATE0\r\n","OK\r\n","ERROR\r\n",5000,M95_buffer);		
	deactivate_pin(GREEN_LED);
	vTaskDelay(100 / portTICK_PERIOD_MS);
	// Disable "result code" prefix
	sendAT("AT+CMEE=0\r\n","OK\r\n","ERROR\r\n",5000,M95_buffer);	
	activate_pin(GREEN_LED);
	vTaskDelay(100 / portTICK_PERIOD_MS);
	// Indicate if a password is required
	sendAT("AT+CPIN?\r\n","OK\r\n","ERROR\r\n",5000,M95_buffer);
	// Set SMS message format as text mode	
	sendAT("AT+CMGF=1\r\n","OK\r\n","ERROR\r\n",5000,M95_buffer);
	// TCPIP Transfer mode (=0 -> Use "AT+QISEND" command)	 
	sendAT("AT+QIMODE=0\r\n","OK\r\n","ERROR\r\n",5000,M95_buffer);	
	// Signal Quality - .+CSQ: 6,0  muy bajo
	sendAT("AT+CSQ\r\n","OK\r\n","ERROR\r\n",300,M95_buffer);
	// Network Registration Status	
	bandera=sendAT("AT+CGREG?\r\n",",1","ERROR\r\n",300, M95_buffer);
	// Request IMEI (Internatinal Mobile Identify)
	sendAT("AT+GSN\r\n","OK\r\n","ERROR\r\n",300, M95_buffer);
	//Delete all SMS
	sendAT("AT+QMGDA=\"DEL ALL\"\r\n","OK\r\n","ERROR\r\n",10000,   M95_buffer); 
	// Activate GPRS context
	sendAT("AT+QIACT\r\n","OK\r\n","ERROR\r\n" ,1000,   M95_buffer);	
	// Synchronize the Local Time Via NTP
	vTaskDelay(1000/portTICK_PERIOD_MS);
	bandera = sendAT("AT+QNTP=\"0.south-america.pool.ntp.org\"\r\n"
					,"+QNTP","3",10000,M95_buffer);
	if((bandera == 0)){
		printf("No se pudo actualizar la hora\n");
		activate_pin(ESP_ERROR_PIN);
		vTaskDelay(100/portTICK_PERIOD_MS);
	}
	else{
	deactivate_pin(ESP_ERROR_PIN);
	printf("Hora Actualizada\n Equipo Inicializado Correctamente\n");
	}
	activate_pin(ESP_READY_PIN);
	return bandera;
}

int connect_MQTT_server(){
	sendAT("AT+QMTOPEN=1,\"161.97.102.234\",1883\r\n","+QMTOPEN:","ERROR\r\n",5000,M95_buffer);
	if(strstr((char*)M95_buffer,"-1"))
	{
		ESP_LOGE("M95 E","Cant open IP");
		return 0;
	}
	vTaskDelay(1000/portTICK_PERIOD_MS);
    // Connect with name "M95" to Broker
	int a = sendAT("AT+QMTCONN=1,\"Agro_test\"\r\n","OK\r\n","ERROR\r\n",5000,M95_buffer);
	vTaskDelay(500 / portTICK_PERIOD_MS);
	return a;
}

int M95_CheckConnection(){
	activate_pin(ESP_ERROR_PIN);
	vTaskDelay(2500 / portTICK_PERIOD_MS);	
	return connect_MQTT_server();
}


int M95_check_subs(char* IMEI_number){
	char command_sub[100];
	sprintf(command_sub,"AT+QMTSUB=1,2,\"Agro_test/%s/OTA\",0\r\n",IMEI_number);
	int intentos = 3;
	int success = 0;
	
	while(intentos > 0){
		if(sendAT(command_sub,"+QMTRECV:","ERROR\r\n",10000,M95_buffer) == 1){
			success = 1;
			break;
		}
		activate_pin(RED_LED);
		intentos--;
		vTaskDelay(1);
	}
	
	if(intentos < 1){
		return -1;
	}

	if(success == 0){
		return -1;
	}

	deactivate_pin(RED_LED);
	activate_pin(GREEN_LED);

	char lim[3] = ",";
	char * p_clock = strtok((char*)M95_buffer,lim);
	while(p_clock != NULL){
		if(strstr(p_clock,"OTA")!= NULL){
			break;
		}
		p_clock=strtok(NULL,lim);
		//printf("Iteracion = |%s|\n",p_clock);
	}
	p_clock=strtok(NULL,lim);
	//printf("Iteracion = |%s|\n",p_clock);
	
	vTaskDelay(1);
	deactivate_pin(GREEN_LED);
	return atoi(p_clock);
}


char* get_m95_date(){
	int a = sendAT("AT+CCLK?\r\n","+CCLK: ","ERROR",1000,M95_buffer);
	if( a != 1){
		printf("hora no conseguida\r\n");	
		return -1;		
	}
	char lim[3] = "\"";
	char * p_clock = strtok(M95_buffer,lim);
	while(p_clock != NULL){
		if(strstr(p_clock,"/")!= NULL){
			break;
		}
		p_clock=strtok(NULL,lim);
	}
	return (char*) p_clock;
}

int sendAT(char *command, char *ok, char *error, uint32_t timeout, char *response)
{
	memset(response, '\0',strlen(response));
	bool _timeout = true;
	//Define "UART_MODEM" in program (i.e. #define UART_MODEM UART_NUM_2)
	uint64_t n = esp_timer_get_time();
	uint64_t idle_time = (uint64_t)(timeout*1000);
	bool _error = false;

	if(debug == 1){
		activate_pin(GREEN_LED);
		vTaskDelay(100 / portTICK_PERIOD_MS);
		deactivate_pin(GREEN_LED);
		printf("Request Command =\n\t%s",command);
	}

	rx_modem_ready = 0;
	uart_write_bytes(UART_MODEM, (uint8_t *)command, strlen(command));
	
	while((esp_timer_get_time() - n) < idle_time){
        vTaskDelay(1);
		if(rxBytesModem > 0){
            break;
        }
    }

	while((esp_timer_get_time() - n) < idle_time){
		if (strstr((char *)p_RxModem,ok)!= NULL){
			if(debug == 1) printf(" ...Sucessful\n");    
			_timeout = false;
			_error = false;
			break;
		}
		else if(strstr((char *)p_RxModem,error)!= NULL){
			if(debug == 1) printf(" ...Error\n");
			_timeout = false;
			_error = true;
			break;
		}
	}

    if(_timeout){
		printf("- M95 not responded\n");
        return 0;
    }

	memcpy(response,p_RxModem,rxBytesModem);
	if(!_error){
		uart_flush(UART_MODEM);
		vTaskDelay(100 / portTICK_PERIOD_MS);
		return 1;
	}
	return 99;

}

int readAT(char *ok, char *error, uint32_t timeout, char *response)
{
	memset(response, '\0',strlen(response));
	int correcto = 0;
	bool _timeout = true;
	//uint16_t RxLen;
	uint64_t n = esp_timer_get_time();
	uint64_t idle_time = (uint64_t)(timeout*1000);
	rx_modem_ready = 0;
	
	while((esp_timer_get_time() - n) < idle_time){
        vTaskDelay(1);
        if(rx_modem_ready == 1){
            break;
        }
    }

	while((esp_timer_get_time() - n) < idle_time){
		vTaskDelay(2);	
		if (strstr((char *)p_RxModem,ok)!= NULL){
			correcto=1;
			_timeout = false;
			break;
		}
		else if(strstr((char *)p_RxModem,error)!= NULL){
			correcto=2;
			_timeout = false;
			break;
		}
	}	

    if(_timeout){
		printf("- M95 not responded\n");
        return 0;
    }

	memcpy(response,p_RxModem,rxBytesModem);

	if(debug==1){
		printf(response);
		printf("\r\n");
	}

	return correcto;
}

void disconnect_mqtt(){
	//sendAT("AT+QMTDISC=1\r\n","OK\r\n", "ERROR\r\n", 1000, M95_buffer);
	//vTaskDelay(1000 / portTICK_PERIOD_MS);
	sendAT("AT+QMTCLOSE=1\r\n","OK\r\n","ERROR\r\n", 1000,M95_buffer);
	vTaskDelay(1000/portTICK_PERIOD_MS);
}

uint8_t OTA(uint8_t *buff, uint8_t *inicio, uint8_t *fin, uint32_t len){
    const char *TAG = "OTA";
    if (*inicio) { //If it's the first packet of OTA since bootup, begin OTA
        ESP_LOGI(TAG,"BeginOTA");
        //Serial.println("BeginOTA");
        const esp_task_wdt_config_t config_wd = {
            .timeout_ms = 20,
            .idle_core_mask = 0,
            .trigger_panic = false,
        };
        
        esp_task_wdt_init(&config_wd);
        esp_ota_begin(esp_ota_get_next_update_partition(NULL), OTA_SIZE_UNKNOWN, &otaHandlerXXX);
        *inicio = 0;
    }
    if (len > 0)
    {
        esp_ota_write(otaHandlerXXX,(const void *)buff, len);

        if (len != 512 || *fin ==1)
        {
            esp_ota_end(otaHandlerXXX);
			const esp_task_wdt_config_t config_wd = {
				.timeout_ms = 5,
				.idle_core_mask = 0,
				.trigger_panic = false,
			};
			
			esp_task_wdt_init(&config_wd);
            ESP_LOGI(TAG,"EndOTA");
            //Serial.println("EndOTA");
            if (ESP_OK == esp_ota_set_boot_partition(esp_ota_get_next_update_partition(NULL))) {
                vTaskDelay(2000 / portTICK_PERIOD_MS);
                esp_restart();
            }
            else {
                ESP_LOGI(TAG,"Upload Error");
                //Serial.println("Upload Error");
            }
        }

    }

    //uint8_t txData[5] = {1, 2, 3, 4, 5};
    //delay(1000);
    return 1;
}

char M95_readSMS(char* sms_tmp){
	memset(sms_tmp,'\0',strlen(sms_tmp));
	if(sendAT("AT+CMGL=\"REC UNREAD\"\r\n","+CMGL","ERROR\r\n",5000,sms_tmp)==1){
		printf("SMS: SMS encontrado\r\n");
		return 1;
	} else{
		return 0;
	}
}

void M95_sendSMS(char *mensaje, char *numero){
	char temp_resp[200];
	memset(buffer, '\0',strlen(buffer));
	sprintf(buffer,"AT+CMGS=\"%s\"\r\n",numero);
	sendAT(buffer,">", "ERROR\r\n", 5000,temp_resp);

	memset(buffer, '\0',strlen(buffer));
	sprintf(buffer,"%s%c",mensaje,26);
	uart_flush(UART_MODEM);
	uart_write_bytes(UART_MODEM,buffer,strlen(buffer));
	uart_wait_tx_done(UART_MODEM,100000/portTICK_PERIOD_MS);
	readAT("+CMGS","ERROR",10000,temp_resp);
}



uint8_t TCP_open(char *IP, char *PORT){
    uint8_t temporal=0;
	//debug = 0;
	// Select a Context as Foreground Context : = 0 VIRTUAL_UART_1  := 1 VIRTUAL_UART_2 
    sendAT("AT+QIFGCNT=0\r\n","OK","ERROR",10000,M95_buffer);
	vTaskDelay(20 / portTICK_PERIOD_MS);

	// Select CSD or GPRS as the Bearer
	sendAT("AT+QICSGP=1,\"sm.entel.pe\"\r\n","OK","ERROR",10000,M95_buffer);
	vTaskDelay(20 / portTICK_PERIOD_MS);

    // Control Whether or Not to Enable Multiple TCPIP Session: 0 Simple - 1 Multiple
    sendAT("AT+QIMUX=0\r\n","OK","ERROR",10000,M95_buffer);
    vTaskDelay(20 / portTICK_PERIOD_MS);

    // Transparente 0 no trasnparente 1 transparente
	// Select TCPIP Transfer Mode
    sendAT("AT+QIMODE=0\r\n","OK","ERROR",10000,M95_buffer);
	vTaskDelay(20 / portTICK_PERIOD_MS);

    //IMPORTANTE: LEER ESTE COMANDO
    ///enviar_AT("AT+QITCFG=3,2,512,1\r\n","K\r\n","ERROR\r\n",25500,   M95_buffer);

    // USE ip address as the address to establish TCP/UDP
	// Connect with IP Address or Domain Name Server
    sendAT("AT+QIDNSIP=0\r\n","OK","ERROR",25500,M95_buffer);
    vTaskDelay(2000 / portTICK_PERIOD_MS);

    // register TCP/IP  stack
	// AT+QIREGAPP Start TCPIP Task and Set APN, User Name and Password
    sendAT("AT+QIREGAPP\r\n","OK","ERROR",25500,M95_buffer);
    vTaskDelay(2000 / portTICK_PERIOD_MS);

    // Activate FGCNT
	// Activate GPRS/CSD Context
    sendAT("AT+QIACT\r\n","OK","ERROR",25500,M95_buffer);
    vTaskDelay(2000 / portTICK_PERIOD_MS);

    // Get Local IP address
    sendAT("AT+QILOCIP\r\n",".","ERROR",35000,M95_buffer);
    vTaskDelay(2000 / portTICK_PERIOD_MS);

    char qiopen[40];
    sprintf(qiopen,"AT+QIOPEN=\"TCP\",\"%s\",\"%s",IP,PORT);
    uart_write_bytes(UART_MODEM,qiopen,strlen(qiopen));

    sendAT("\"\r\n","OK","ERROR",5000,M95_buffer);
    temporal = readAT("CONNECT OK","ERROR",10000,M95_buffer);
	vTaskDelay(5000 / portTICK_PERIOD_MS);
    return temporal; //1 == ok 1 != error
}

uint8_t TCP_send(char *msg, uint8_t len){
    //sendAT(m95,"ATE1\r\n","OK\r\n","ERROR\r\n",5000,  m95->M95_buffer);//DES Activa modo ECO.

    uint8_t temporal = 0;
	debug = 0;
    temporal = sendAT("AT+QISEND\r\n",">","ERROR\r\n",25500,M95_buffer);
    vTaskDelay(200 / portTICK_PERIOD_MS);
    if(temporal != 1){ 
        return 0; 
    }
    uart_write_bytes(UART_MODEM,(void *)msg,len);
    temporal = sendAT("\x1A","SEND OK\r\n","ERROR\r\n",25500,M95_buffer);
	if(temporal != 1){
        return 0;
    }
    //printf("SEND OK CORRECTO\r\n");
    memset(M95_buffer, 0 , 50);
    return 1;
}

void TCP_close(){
	vTaskDelay(200 / portTICK_PERIOD_MS);          
    sendAT("AT+QICLOSE\r\n","OK\r\n","ERROR\r\n",5000,M95_buffer);
    sendAT("AT+QIDEACT\r\n","OK\r\n","ERROR\r\n",5000,M95_buffer);
	/*
    if(!sendAT("AT\r\n","OK\r\n","ERROR\r\n",5000,M95_buffer)){
        if(!sendAT("AT\r\n","OK\r\n","ERROR\r\n",5000,M95_buffer)){
            reiniciar();
        }
    }
	*/
}

void reiniciar(){
    //encender();

    M95_pwron();
    uint8_t fin = 0;
    const char * TAG = "Reinciar";
    do{
        vTaskDelay(5000 / portTICK_PERIOD_MS);
        if(sendAT("AT\r\n","OK\r\n","ERROR\r\n",5000,M95_buffer)){
            fin=1;
            ESP_LOGI(TAG,"M95 respondio, encendido");
            //DBGSerial->println("M95 respondio, encendido");
        }
        else{
            ESP_LOGI(TAG,"No respondio el M95... encendiendo....");
            //DBGSerial->println("No respondio el M95... encendiendo....");
            //encender();
            M95_pwron();
        }

    }while(!fin);

    //delay(30000);
    vTaskDelay(30000 / portTICK_PERIOD_MS);
    M95_begin();
    vTaskDelay(20000 / portTICK_PERIOD_MS);
}

