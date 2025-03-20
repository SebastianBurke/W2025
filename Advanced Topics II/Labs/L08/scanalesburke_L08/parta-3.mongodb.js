use("scanalesburke_db")
db.cars.updateOne(
  { manufacturer: "Lincoln", model: "Continental" },
  { $set: { model: "Continental Mark X" } }
);

