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

main_topic = "Agro_test/"
subtopic = ["1","1"]
pub_message = ["1","1"]

subtopic[0] = "Agro_test/863192058179343/db"
subtopic[1] = "Agro_test/123456789101112"

pub_message[0] = "23/02/15,17:54:54+00@9U@4.06V\r\n#2.7#28.2#0.0#3.0#0.0#0.0#0.0\r\n#0.0#28.2#0.0#3.0#0.0#0.0#0.0"
pub_message[1] = "23/02/16,20:04:15+00@4.18V\r\n#-99.9#25.8#0.0#5.1\r\n#66.9#25.7#0.0#6.7"

def on_publish(client, userdata, result):
    print("data published \n")
    pass
client = mqtt.Client()
client.on_publish = on_publish
client.connect("161.97.102.234", 1883)
for i,topic in enumerate(subtopic):
    print(f"Topic = {topic}  & Index = {i}")
    ret = client.publish(topic,pub_message[i])
