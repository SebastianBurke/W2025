from sense_hat import SenseHat
import time

sense = SenseHat()

for _ in range(30):
    temp = sense.get_temperature()
    print("{:.2f}".format(temp))
    time.sleep(5)

print("Temperature data collected.")
