from colorama import init, Fore
from parta import *

def print_pass(the_data, i):
    print(f'{Fore.GREEN}Test {i} with {the_data} passed')


def print_fail(the_data, i):
    print(f'''{Fore.RED}*** Test {i} with {the_data} failed.''')
    
    
def test_final_grade():
    
    def call_function(par, expected):
        assert final_grade(par[0], par[1]) == expected, par
    
    print('''
Test final grade function
-------------------------''')

    data_final_grade = [
        {"params": [100, 11], "expected": "A"},
        {"params": [91, 9], "expected": "A"},
        {"params": [90, 11], "expected": "A"},
        {"params": [91, 9], "expected": "A"},
        {"params": [90, 10], "expected": "B"},
        {"params": [76, 5], "expected": "B"},
        {"params": [75, 5], "expected": "C"},
        {"params": [76, 4], "expected": "C"},
        {"params": [51, 2], "expected": "C"},
        {"params": [51, 1], "expected": "F"},
        {"params": [0, 0], "expected": "F"}
    ]
    
    for i, test_params in enumerate(data_final_grade):
        try:
           call_function(test_params.get("params"), test_params.get("expected"))
           print_pass(test_params.get("params"), i+1)
        except AssertionError as test_data:
            print_fail(test_data, i+1)

def test_phone_number():
    
    def call_function(par, expected):
        assert phone_number(par) == expected, par
    
    print('''
Test phone number function
--------------------------''')

    data_phone_number = [
        {"params": [8, 1, 9, 7, 7, 8, 2, 2, 7, 0], "expected": "(819) 778-2270"},
        {"params": [1, 2, 3, 4, 5, 6, 7, 8, 9, 0], "expected": "(123) 456-7890"},
        {"params": [1, 2, 3, 4, 5, 6, 7, 8, 9], "expected": "error"},
        {"params": [10, 2, 3, 4, 5, 6, 7, 8, 9, 0], "expected": "error"}
    ]
    
    for i, test_params in enumerate(data_phone_number):
        try:
           call_function(test_params.get("params"), test_params.get("expected"))
           print_pass(test_params.get("params"), i+1)
        except AssertionError as test_data:
            print_fail(test_data, i+1)


def test_readable_time():
    
    def call_function(par, expected):
        assert readable_time(par) == expected, par
    
    print('''
Test readable time function
--------------------------''')

    data_time = [
        {"params": 3600, "expected": "1:00:00"},
        {"params": 3661, "expected": "1:01:01"},
        {"params": 363054, "expected": "100:50:54"},
        {"params": 86400, "expected": "24:00:00"},
        {"params": 0, "expected": "0:00:00"},
        {"params": 35939, "expected": "9:58:59"}
    ]
    
    for i, test_params in enumerate(data_time):
        try:
           call_function(test_params.get("params"), test_params.get("expected"))
           print_pass(test_params.get("params"), i+1)
        except AssertionError as test_data:
            print_fail(test_data, i+1)


def test_people_on_bus():
    
    def call_function(par, expected):
        assert people_on_bus(par) == expected, par
    
    print('''
Test bus people function
--------------------------''')

    data_bus = [
        {"params": [(3,0)], "expected": 3},
        {"params": [(1,0),(1,1),(1,1),(1,1),(1,1),(1,1),(1,1),(1,1),(1,1),(1,1),(1,1),(1,1)], "expected": 1},
        {"params": [(3,0),(3,2),(5,2),(3,5)], "expected": 5},
        {"params": [(0,0)], "expected": 0},
        {"params": [(10,0), (10,0), (10,0), (10,0), (10,0)], "expected": 50},
        {"params": [(31,0), (0,1), (0,1), (0,1), (0,1), (0,1), (0,1), (0,1), (0,1), (0,1), (0,1), (0,1)], "expected": 20},
    ]
    
    for i, test_params in enumerate(data_bus):
        try:
           call_function(test_params.get("params"), test_params.get("expected"))
           print_pass(test_params.get("params"), i+1)
        except AssertionError as test_data:
            print_fail(test_data, i+1)


def do_tests():
    test_final_grade() 
    test_phone_number()
    test_readable_time()
    test_people_on_bus()
            

if __name__ == "__main__":
    init(autoreset=True)
    do_tests()