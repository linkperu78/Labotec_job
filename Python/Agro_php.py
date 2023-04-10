import mysql.connector
import paho.mqtt.client as mqtt
import datetime
import json
import re
from BaseModel import BaseModel as BM


#TODO: aegurarse que el id del dispositivo coincida con el de la base de datos, si no mostrarlo en el print


""" query para obtener imei, cuarto y 
 SELECT wmbus_sensores.Nombre, gateways.cuarto , wmbus_sensores.id_imei  FROM wmbus_sensores INNER JOIN gateways ON wmbus_sensores.id_imei=gat
eways.imei; 
"""

main_topic = "Agro_test"
base_column = ["id","id_sensor","CM","Humedad","Temperatura","Conductividad","pH"
              ,"Potasio","Sodio","Fosforo","fecha","Estado","BATERIA","BAT"]
log_saphi=[]
def on_connect(client, userdata, flags, rc):
    client.subscribe(main_topic+"/#")
    #print("Comenzo el proceso")

def on_message(client, userdata, msg):
    if(msg.topic.find(main_topic)!=-1):
        try:
            my_db = BM()
        except:
            print("\n NO SE PUDO CONECTAR")
            return

    sub_strings = msg.topic.split("/")
    IMEI = sub_strings[1]

    ## se valida el imei del gateway
    pos_id_IMEI = my_db.get_imei_id_locacion(IMEI)
    if(msg.topic.find("OTA") >= 0):
        return
    # Si el IMEI esta registado, decodificamos el mensaje que viene
    hora_registrada = f"{datetime.datetime.now()}".split('.')[0]
    if(pos_id_IMEI != None):
        gateway_dict = f"- Status: Registrado - IMEI: {IMEI} - ID: {pos_id_IMEI[0]} - Hora: {hora_registrada}" 
        #{'gateway_status':'IMEI registrado','IMEI' : IMEI,'id_locacion': pos_id_IMEI[0], 'Hora' : f"{datetime.datetime.now()}".split('.')[0]}
        #print(json.dumps(log_saphi))   
        tag_sensors = my_db.get_sensors_tags_from_imei(pos_id_IMEI[0])
    else:
        gateway_dict = f"- Status: No Registrado - IMEI: {IMEI} - Hora: {hora_registrada}"
        #{'gateway_status':'IMEI no registrado','IMEI' : IMEI, 'Hora' : hora_registrada}
        log_saphi.append(gateway_dict)
        print_log(log_saphi)
        #print(json.dumps(log_saphi))
        return

    # Decodificamos el mensaje recibido
    msg_in = str(msg.payload.decode("'latin1'")) 
    # Arreglamos los datos en una lista "data_sensor"
    data = msg_in.split("\r\n")
    data_sensor = [""]*len(tag_sensors)

    for i,a in enumerate(data_sensor):
        temp = data[i+1].split("#")
        temp.pop(0)
        data_sensor[i] = temp

    # Obtenemos la fecha
    _temp_header = data[0].split("@")
    date = re.split('@|/|#|,|:|\\+',_temp_header[0].split("+")[0])
    try:
        b = list(map(int, date))
        b[0] = b[0]+2000
        epoch_time = datetime.datetime(b[0], b[1], b[2], b[3], b[4],b[5]).timestamp()
        fecha = (f"{datetime.datetime.fromtimestamp( epoch_time)}")
    except:
        gateway_dict += f" - ERROR: {date}"
        log_saphi.append(gateway_dict)
        print_log(log_saphi)
        return
    
    volt = float(_temp_header[-1].split("V")[0])
    v_p =  min(max(0,int((volt-3.2)*100/0.92)),100)

    is_db  = False
    column_names = []
    column_names = column_names+base_column


    if(msg.topic.find("db") >= 0):
        is_db = True
        column_names += ["sig_lvl"]
    if(is_db):
        signal = _temp_header[1].split("U")[0]
    for i,tag in enumerate(tag_sensors):
        estado = "OK"
        for dato in data_sensor[i]:
            try:
                a = float(dato)
                if(a < -90):
                    estado = "FAIL"
                    pass
            except:
                estado = "FAIL"
                dato = -99.9
        print(tag)
        
        datos_update = ""
        try:
            pro = 30*(i+1)
            if(is_db):
                datos_update = [tag]
                datos_update.append(f"{pro}")
                datos_update += data_sensor[i]
                datos_update.append(fecha)
                datos_update.append(estado)
                datos_update.append(volt)
                datos_update.append(v_p)
                datos_update.append(signal)
                my_db.update_signal_level("saphi_Modem",pos_id_IMEI[0],signal)
            else:
                datos_update = [tag]
                datos_update.append(f"{pro}")
                datos_update += data_sensor[i]
                datos_update += [0,0,0]
                datos_update.append(fecha)
                datos_update.append(estado)
                datos_update.append(volt)
                datos_update.append(v_p)
            my_db.save_in_table_sql("saphi_db",column_names,datos_update)
        except :
            gateway_dict += f" - ERROR: {datos_update}"
    log_saphi.append(gateway_dict)
    print_log(log_saphi)
    print("")

def print_log(msg_array):
    for log in msg_array:
        print(log)



print("---- \tIniciando la supervision de datos \t---- \n")
client = mqtt.Client()
client.on_connect = on_connect
client.on_message = on_message
client.connect("161.97.102.234", 1883, 60)
client.loop_forever()
