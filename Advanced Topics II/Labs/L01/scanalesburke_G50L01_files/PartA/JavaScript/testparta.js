let printPass = (the_data, i) => console.log(`Test ${i} with ${the_data} passed`);

let printFail = (the_data, i) => console.error(`Test ${i} with ${the_data} failed`);

let testFinalGrade = () => {
    console.log(`
Test final grade function
-------------------------`);

    let dataFinalGrade = [
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
    ];

    dataFinalGrade.forEach((testParams, i) => {
        let actual = finalGrade(testParams.params[0], testParams.params[1]);
        if ( actual === testParams.expected)
            console.log(`Test ${i+1} passed with data ${testParams.params}; answer was ${actual}`);
        else
            console.error(`Test ${i+1} failed with data ${testParams.params}; expected: ${testParams.expected}, received ${actual}`);
    });
}

let testPhoneNumber = () => {
    console.log(`
Test phone number function
--------------------------`);

    let dataPhoneNumber = [
        {"params": [8, 1, 9, 7, 7, 8, 2, 2, 7, 0], "expected": "(819) 778-2270"},
        {"params": [1, 2, 3, 4, 5, 6, 7, 8, 9, 0], "expected": "(123) 456-7890"},
        {"params": [1, 2, 3, 4, 5, 6, 7, 8, 9], "expected": "error"},
        {"params": [10, 2, 3, 4, 5, 6, 7, 8, 9, 0], "expected": "error"}
    ]

    dataPhoneNumber.forEach((testParams, i) => {
        let actual = phoneNumber(testParams.params);
        if ( actual === testParams.expected)
            console.log(`Test ${i+1} passed with data ${testParams.params}; answer was ${actual}`);
        else
            console.error(`Test ${i+1} failed with data ${testParams.params}; expected: ${testParams.expected}, received ${actual}`);
    })
}

let testReadableTime = () => {
    console.log(`
Test time function
--------------------------`);

    dataTime = [
        {"params": 3600, "expected": "1:00:00"},
        {"params": 3661, "expected": "1:01:01"},
        {"params": 363054, "expected": "100:50:54"},
        {"params": 86400, "expected": "24:00:00"},
        {"params": 0, "expected": "0:00:00"},
        {"params": 35939, "expected": "9:58:59"}
    ]


    dataTime.forEach((testParams, i) => {
        let actual = readableTime(testParams.params);
        if ( actual === testParams.expected)
            console.log(`Test ${i+1} passed with data ${testParams.params}; answer was ${actual}`);
        else
            console.error(`Test ${i+1} failed with data ${testParams.params}; expected: ${testParams.expected}, received ${actual}`);
    })
}    

let testPeopleOnBus = () => {
    console.log(`
Test people on bus function
--------------------------`);

    dataPeople = [
        {"params": [[3,0]], "expected": 3},
        {"params": [[1,0],[1,1],[1,1],[1,1],[1,1],[1,1],[1,1],[1,1],[1,1],[1,1],[1,1],[1,1]], "expected": 1},
        {"params": [[3,0],[3,2],[5,2],[3,5]], "expected": 5},
        {"params": [[0,0]], "expected": 0},
        {"params": [[10,0], [10,0], [10,0], [10,0], [10,0]], "expected": 50},
        {"params": [[31,0], [0,1], [0,1], [0,1], [0,1], [0,1], [0,1], [0,1], [0,1], [0,1], [0,1], [0,1]], "expected": 20},
]

    dataPeople.forEach((testParams, i) => {
        let actual = peopleOnBus(testParams.params);
        if ( actual === testParams.expected)
            console.log(`Test ${i+1} passed with data ${testParams.params}; answer was ${actual}`);
        else
            console.error(`Test ${i+1} failed with data ${testParams.params}; expected: ${testParams.expected}, received ${actual}`);
    })
}    

console.clear();
testFinalGrade();
testPhoneNumber();
testReadableTime();
testPeopleOnBus();