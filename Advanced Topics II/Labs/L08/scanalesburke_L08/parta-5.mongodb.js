use("scanalesburke_db")
db.cars.updateOne(
  { year: 1984}, 
  { $unset: { vin: "" } },
);
