from mysql.connector import Error
import mysql.connector as pysql

class sql:
    def __init__(self,host_name,user_name, user_password, port):
        self.host_name = host_name
        self.user_name = user_name
        self.user_password = user_password
        self.port = port

    def connect(self):
        try:
            self.cnx = pysql.connect(
                host = self.host_name,
                user = self.user_name,
                port = self.port,
                password = self.user_password,
            )
            print("Sucessful connection")
        except Error as e:
            print(f"ERROR = '{e}'")
        self.cursor = self.cnx.cursor()
        print("Cursos definied")

    def create_db(self,name_db):
        try:
            self.cursor.execute(
                "CREATE DATABASE {} DEFAULT CHARACTER SET 'utf8'".format(name_db)
            )
        except Error as e:
            print(f"Failed creating database {name_db}")


    def connect_database(self,name_db):
        try:
            self.cursor.execute(f"USE {name_db}")
            print(f"Database CHANGED to  _{name_db}_   sucessfully")
        except pysql.Error as e:
            print("Database {} does not exists.".format(name_db))
            if e.errno == 1049:     # Error 1049 = db dont exists
                self.create_db(name_db)
                print("Database {} created successfully.".format(name_db))
                self.cnx.database = name_db
            else:
                print(e)

    def create_table(self,name_db,name_table,command):
        self.connect_database(name_db)
        try:
            print(" ..... Creating table {}: \t".format(name_table), end='')
            self.cursor.execute(command)
            self.cnx.commit()
        except Error as e:
            print("ERROR N° ",end="")
            if e.errno == 1050:
                print("ERROR, this table already exists.")
            else:
                print(e.errno)
        else:
            print("OK")

    def get_columns_struct(self,table_name):
        column_array = {}
        try:
            self.cursor.execute(f"SHOW COLUMNS from {table_name}")
            mensaje = self.cursor.fetchall()
            column_array = [a[0] for a in mensaje]
        except Error as e:
            print(e)
        return column_array

    def array_to_string(self,array_columns,ident):
        new_data = []
        for i in array_columns:
            new_item = f"{ident}"+i+f"{ident}"
            new_data.append(new_item)
        temp = ', '.join(new_data)
        temp = "("+temp+")"
        return temp


    def insert_data(self,table_name,data_array):
        name_columns = self.get_columns_struct(table_name)
        string_columns = self.array_to_string(name_columns,"")
        string_value = self.array_to_string(data_array,"'")
        query = (f"INSERT INTO {table_name} "+string_columns+" values "+string_value)
        try:
            self.cursor.execute(query)
            self.cnx.commit()
            print("DATA UPDATE SUCCESSFULLY")
        except Error as e:
            print(e)

    def view_table_data(self,table_name):
        query = (f"select * from {table_name}")
        self.cursor.execute(query)
        a = self.cursor.fetchall()
        print(f"DATOS EN LA TABLA {table_name} ::: ")
        print(a)