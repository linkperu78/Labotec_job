from pyvantagepro import VantagePro2
device = VantagePro2.from_url('tcp:host-ip:port')
data = device.get_current_data()