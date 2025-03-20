use("scanalesburke_db")
db.cars.find(
    { manufacturer: "Ford", model: { $regex: "^F" } }
  ).count();
  