# 161.97.102.234
import random
import time
from paho.mqtt import client as mqtt_client

# Definimos los IMEIS
IMEI1 = "Sensor1"
IMEI2 = "Sensor2"

broker = '161.97.102.234'
port = 1883
topic = "Agro_test/#"
# generate client ID with pub prefix randomly
client_id = f'python-mqtt-{random.randint(0, 100)}'
username = 'agro_test'
password = ''


def connect_mqtt() -> mqtt_client:
    def on_connect(client, userdata, flags, rc):
        if rc == 0:
            print("Connected to MQTT Broker!")
        else:
            print("Failed to connect, return code %d\n", rc)

    client = mqtt_client.Client(client_id)
    client.username_pw_set(username, password)
    client.on_connect = on_connect
    client.connect(broker, port)
    return client

def msg_from_mqtt():
    def __init__(self,tag):
        self.tag = tag
        self.data = [None] * 7
        self._h = 0
        self._t = 0
        self._c = 0
        self._pH = 0
        self._N = 0    
        self._P = 0
        self._K = 0
    def actualizar(self,mensaje_str):
        mensaje_str.split()

def subscribe(client: mqtt_client):
    def on_message(client, userdata, msg):
        a = msg.payload.decode()
        print(f"Received `{a}` from `{msg.topic}` topic")
    client.subscribe(topic)
    client.on_message = on_message


def run():
    client = connect_mqtt()
    subscribe(client)
    client.loop_forever()


if __name__ == '__main__':
    run()
    