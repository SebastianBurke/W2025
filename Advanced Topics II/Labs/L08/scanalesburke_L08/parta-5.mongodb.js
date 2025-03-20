use("scanalesburke_db")
db.cars.updateOne(
  {}, 
  { $unset: { vin: "" } },
  { sort: { Year: 1 }, skip: 2, limit: 1 }
);
