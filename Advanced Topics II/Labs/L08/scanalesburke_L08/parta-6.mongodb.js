use("scanalesburke_db");

var bulk = db.employees.initializeUnorderedBulkOp();


bulk.find( { phone: "123-456-7890"} ).upsert().updateOne({
        $set: {
          name: { first_name: "George", last_name: "Nadeau" },
          phone: "123-456-7899",
          salary: 1000,
          hire_date: new Date("2022-02-01")
        }
  }
);

bulk.execute();

print("Bulk write operation completed successfully!");
