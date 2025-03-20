use("scanalesburke_db")
// db.employees.insertMany([
//     { name: { first_name: "Mary", last_name: "Smith" }, phone: "819-111-1111", salary: 75000, hire_date: new Date("2020-09-30"), trained: true },
//     { name: { first_name: "John", last_name: "Jones" }, phone: "613-887-7777", salary: 56000, hire_date: new Date("2021-03-15"), trained: false },
//     { name: { first_name: "Peter", last_name: "Dinklage" }, phone: "613-555-5555", salary: 120000, hire_date: new Date("2020-12-25"), trained: true },
//     { name: { first_name: "Monica", last_name: "Farrow" }, phone: "255-985-7451", salary: 55621, hire_date: new Date("2021-05-20"), trained: true },
//     { name: { first_name: "Siobhan", last_name: "Fox" }, phone: "613-845-7824", salary: 75412, hire_date: new Date("2021-10-18"), trained: false },
//     { name: { first_name: "Roderick", last_name: "Maine" }, phone: "819-222-4444", salary: 21481, hire_date: new Date("2020-01-17"), trained: false },
//     { name: { first_name: "Faye", last_name: "Ray" }, phone: "819-123-4567", salary: 85412, hire_date: new Date("2021-11-20"), trained: true },
//     { name: { first_name: "William", last_name: "Frederick" }, phone: "819-555-6666", salary: 65231, hire_date: new Date("2020-03-06"), trained: false },
//     { name: { first_name: "Andrew", last_name: "Younger" }, phone: "819-222-8888", salary: 85475, hire_date: new Date("2021-01-01"), trained: true },
//     { name: { first_name: "Karen", last_name: "Efford" }, phone: "250-444-1233", salary: 250000, hire_date: new Date("2021-09-23"), trained: false }
//   ]);

  use("scanalesburke_db")
  db.employees.find(
    { hire_date: { $gte: new Date("2021-01-01"), $lt: new Date("2022-01-01") } }
  ).count();
  