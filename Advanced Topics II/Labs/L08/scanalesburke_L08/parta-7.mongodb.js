use("scanalesburke_db")
db.employees.updateOne(
    { phone: "123-456-7890" },
    {
      $set: {
        name: { first_name: "George", last_name: "Nadeau" },
        phone: "123-456-7899",
        salary: 1000,
        hire_date: new Date("2022-02-01")
      }
    },
    { upsert: true }
  );
  