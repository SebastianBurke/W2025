let finalGrade = (grade, num_proj) => {

    let finalGrade = '';

    if (grade > 90 || num_proj > 10) {
        finalGrade = 'A';
    } else if (grade > 75 && num_proj > 4) {
        finalGrade = 'B';
    } else if (grade > 50 && num_proj > 1) {
        finalGrade = 'C';
    } else {
        finalGrade = 'F';
    }

    return finalGrade;
}

let phoneNumber = (integers) => {
    if (integers.length !== 10){
        return "error";
    }

    for (let int of integers){
        if (int < 0 || int > 9 || !Number.isInteger(int)) {
            return "error"; 
        }
    }

    let formattedString = `(${integers[0]}${integers[1]}${integers[2]}) ${integers[3]}${integers[4]}${integers[5]}-${integers[6]}${integers[7]}${integers[8]}${integers[9]}`;

    return formattedString;
}

let readableTime = (seconds) => {
    let hours = Math.floor(seconds / 3600);
    let minutes = Math.floor((seconds % 3600) / 60);
    let secs = seconds % 60;

    let formattedTime = `${hours.toString()}:${minutes.toString().padStart(2, '0')}:${secs.toString().padStart(2, '0')}`;

    return formattedTime;
};

let peopleOnBus = (stopInfo) => {
    let totalOn = 0;

    for (let [on, off] of stopInfo){
        totalOn += on;
        totalOn -= off;
    }
    return totalOn;
}