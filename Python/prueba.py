Columns_db = ["id","id_sensor","Humedad","Temperatura","Conductividad","pH"
              ,"Potasio","Sodio","Fosforo","fecha","Estado","BATERIA"]
print(Columns_db)
Columns_db.insert(2,"POS cm")
Columns_db += ["BAT","signal"]
print(Columns_db)