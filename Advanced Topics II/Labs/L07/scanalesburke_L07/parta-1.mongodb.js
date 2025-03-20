use("scanalesburke_db")
db.cars.find(
    { manufacturer: "Cadillac" },
    { manufacturer: 1, model: 1, year: 1, _id: 0 }
).sort({ Year: -1 });
  