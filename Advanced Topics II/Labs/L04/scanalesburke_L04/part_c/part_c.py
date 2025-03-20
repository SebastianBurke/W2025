from sense_hat import SenseHat
from time import sleep

sense = SenseHat()

while True:
    for letter in reversed("ABCDEFGHIJKLMNOPQRSTUVWXYZ"):
        sense.show_letter(letter)
        sleep(1)
