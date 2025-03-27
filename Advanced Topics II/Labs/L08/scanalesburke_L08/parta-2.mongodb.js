use("scanalesburke_db")
db.cars.updateMany(
  { year: { $lt: 1990 } },
  { $set: { safetied: false } }
);
