from sense_hat import SenseHat
from time import sleep
from random import randint

sense = SenseHat()
sense.clear()

# Define colors
Y = (255, 255, 0)  # Yellow (Coin)
W = (255, 255, 255)  # White (Player)
G = (0, 255, 0)  # Green (Success)
R = (255, 0, 0)  # Red (Failure)
B = (0, 0, 0)  # Black (Background)

# Initialize player score
score = 0

# Function to place player and treasure
def reset_game():
    global coin_x, coin_y, player_x, player_y
    coin_x, coin_y = randint(0, 7), randint(0, 7)
    player_x, player_y = randint(0, 7), randint(0, 7)
    sense.set_pixel(coin_x, coin_y, Y)
    sleep(0.8)
    sense.clear()

    sense.set_pixel(player_x, player_y, W)

# Function to move player manually
def move(event):
    global player_x, player_y
    if event.action == "pressed":
        sense.set_pixel(player_x, player_y, B) 

        if event.direction == "up" and player_y > 0:
            player_y -= 1
        elif event.direction == "down" and player_y < 7:
            player_y += 1
        elif event.direction == "left" and player_x > 0:
            player_x -= 1
        elif event.direction == "right" and player_x < 7:
            player_x += 1

        sense.set_pixel(player_x, player_y, W)

sense.stick.direction_any = move

for turn in range(5):
    reset_game()

    # Allow user to move multiple times before guessing
    for _ in range(10):  # Give player 10 moves before selecting
        sleep(0.5)

    # Simulated "press enter" action
    print("Press Enter to guess location!")
    sleep(2)

    # Check if player found the treasure
    if player_x == coin_x and player_y == coin_y:
        sense.set_pixel(player_x, player_y, G)
        score += 1
    else:
        sense.set_pixel(player_x, player_y, R)

    sleep(1)
    sense.clear()

if score == 5:
    message = "Perfect! Score: 5"
    text_color = G
elif score >= 3:
    message = "Great! Score: " + str(score)
    text_color = Y
else:
    message = "Try again! Score: " + str(score)
    text_color = R

sense.show_message(message, text_colour=text_color)
sense.clear()
