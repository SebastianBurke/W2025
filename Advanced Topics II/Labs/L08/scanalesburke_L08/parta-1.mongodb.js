use("scanalesburke_db")
db.cars.updateOne(
    { model: "Vibe" },
    { $set: { year: 2001, safetied: false } }
  );
  