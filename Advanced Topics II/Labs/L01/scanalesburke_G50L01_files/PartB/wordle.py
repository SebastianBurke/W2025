import random
from colorama import Fore, Style, init

# Initialize colorama
init()

def load_words(file_path):
    with open(file_path, 'r') as file:
        words = [line.strip().upper() for line in file if len(line.strip()) == 5]
    return words

def choose_random_word(words):
    return random.choice(words)

def display_guess_result(guess, target_word):
    result = ""
    for i, letter in enumerate(guess):
        if letter == target_word[i]:
            result += f"{Fore.GREEN}{letter}{Style.RESET_ALL}"
        elif letter in target_word:
            result += f"{Fore.BLUE}{letter}{Style.RESET_ALL}"
        else:
            result += letter
    print(result)

def play_wordle():
    words = load_words('FiveLetterWords.txt')
    target_word = choose_random_word(words)

    print("Welcome to Wordle!")
    print("Guess the 5-letter word. Letters will be colored as follows:")
    print(f"{Fore.GREEN}Green{Style.RESET_ALL} - Correct letter in the correct position")
    print(f"{Fore.BLUE}Blue{Style.RESET_ALL} - Correct letter in the wrong position")
    print("Default - Letter is not in the word")

    while True:
        guess = input("Enter your guess: ").strip().upper()

        if len(guess) != 5 or not guess.isalpha():
            print("Invalid input. Please enter a 5-letter word.")
            continue

        display_guess_result(guess, target_word)

        if guess == target_word:
            print(f"Congratulations! You guessed the word: {target_word}")
            break

if __name__ == "__main__":
    play_wordle()