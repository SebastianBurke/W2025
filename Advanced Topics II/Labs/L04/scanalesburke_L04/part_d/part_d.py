from sense_hat import SenseHat
import random

sense = SenseHat()

jokes = [
    "Why don’t scientists trust atoms? Because they make up everything!",
    "Parallel lines have so much in common. It’s a shame they’ll never meet.",
    "Why don’t programmers like nature? It has too many bugs!"
]

try:
    while True:
        joke = random.choice(jokes)
        sense.show_message(joke, scroll_speed=0.05)
except KeyboardInterrupt:
    sense.clear()
