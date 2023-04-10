import mysql.connector

class BaseModel(object):
    def __init__(self):
        super().__init__()
        self.conn = mysql.connector.connect(
            host="localhost",
            #user="cipi",
            user = "root",
            port = 3307,
            password = "debian",
            #password="V6JWr8MlIo78qgRd",
            database="saphi"
        )
        self.db = self.conn.cursor()

    def execute(self,sql,params):
        try:
            self.db.execute(sql,params)
            id = self.db.lastrowid
            self.conn.commit()
            return id
        except Exception as e:
            print(str(e))

    def query(self,sql):
        self.db.execute(sql)
        return self.db.fetchall()

    def get(self,sql,*params):
        self.db.execute(sql,params)
        return self.db.fetchone()
    
    def save_in_table_sql(self,name_tb,columns_name,columns_value):
        sql_wbmus = "INSERT INTO "+f"{name_tb}"

        # Columnas
        temp = " ("
        for name in columns_name:
            if(name == "id"):
                continue
            temp += f"{name},"
        temp= temp[:-1]
        temp += ") VALUES "
        sql_wbmus += temp

        # Valores
        temp = " ("
        for value in columns_value:
            temp += f"\'{value}\',"
        temp = temp[:-1]
        temp += ")"
        sql_wbmus += temp
        #print(sql_wbmus)
        #return "a"
        return self.execute(sql_wbmus,None)
    
    def update_signal_level(self,name_table,ID_IMEI,value):
        sql_wbmus = "UPDATE "+f"{name_table} "
        sql_wbmus += "SET signal_db = " + f"\'{value}\' "
        sql_wbmus += f"WHERE id = {ID_IMEI};"
        #print("Signal update:")
        #print(sql_wbmus)
        #print("\n")
        return self.execute(sql_wbmus,None)

    def check_id_wmbus_sensor(self,Nombre:str,ID_sensor:str):
        sql_check = "SELECT wmbus_sensores_2.ID_sensor FROM wmbus_sensores_2 WHERE wmbus_sensores_2.Nombre=%s"
        self.db.execute(sql_check,[Nombre])
        res = self.db.fetchone()
        if(res[0] == ID_sensor):
            return True
        return False

    def is_in_list_imei(self,imei:int):
        sql = "SELECT cuarto,imei,sim_id FROM gateways WHERE imei=%s"
        elements = [imei]
        self.db.execute(sql,elements)
        return self.db.fetchone()
    
    def get_array_columns(self,name_server,name_tb):
        sql_command = "SELECT column_name FROM information_schema.columns WHERE table_schema = %s AND table_name = %s order by ordinal_position"
        elements = [name_server, name_tb]
        self.db.execute(sql_command,elements)
        a = self.db.fetchall()
        array = []
        for name in a:
            array.append(name[0])
        return array

    def get_imei_id_locacion(self,imei:int):
        sql = "SELECT id FROM saphi_Modem WHERE IMEI=%s"
        elements = [imei]
        self.db.execute(sql,elements)
        #TODO: agregar la data de la locacion (otra tabla) en la respuesta
        return self.db.fetchone()
    def get_sensors_tags_from_imei(self,imei:int):
        sql = "SELECT id_Sensor FROM saphi_Sensors WHERE id_IMEI=%s"
        elements = [imei]
        self.db.execute(sql,elements)
        #TODO: agregar la data de la locacion (otra tabla) en la respuesta
        _id_sensor = self.db.fetchall()
        a = [""]*len(_id_sensor)
        for i,_id in enumerate(_id_sensor):
            a[i] = _id[0]
        return a


    def get_sensor_id(self,Nombre:str,ID_sensor:str,gateway_id:int):
        sql = "SELECT id FROM wmbus_sensores_2 WHERE Nombre=%s AND ID_sensor=%s AND gateway_id=%s"
        elements = [Nombre,ID_sensor,gateway_id]
        self.db.execute(sql,elements)
        return self.db.fetchone()


