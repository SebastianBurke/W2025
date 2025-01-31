def final_grade(grade, num_proj):
    if grade > 90 or num_proj > 10:
        return 'A'
    elif grade > 75 and num_proj > 4:
        return 'B'
    elif grade > 50 and num_proj > 1:
        return 'C'
    else:
        return 'F'

def phone_number(integers):
    if len(integers) != 10:
        return "error"
    
    for integer in integers:
        if integer < 0 or integer > 9 or not isinstance(integer, int):
            return "error"
    
    formatted_string = f"({integers[0]}{integers[1]}{integers[2]}) {integers[3]}{integers[4]}{integers[5]}-{integers[6]}{integers[7]}{integers[8]}{integers[9]}"
    return formatted_string

def readable_time(seconds):
    hours = seconds // 3600
    minutes = (seconds % 3600) // 60
    secs = seconds % 60
    
    formatted_time = f"{hours}:{minutes:02}:{secs:02}"
    return formatted_time

def people_on_bus(stop_info):
    total_on = 0
    
    for on, off in stop_info:
        total_on += on
        total_on -= off
    
    return total_on
