use("scanalesburke_db")
db.cars.find(
  { year: { $gte: 1990, $lt: 2000 } }
).count();

  