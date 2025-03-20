use("scanalesburke_db")
db.cars.updateOne(
  {},  // Empty filter gets all documents
  { $set: { colour: "rust" } },
  { sort: { Year: 1 }, limit: 1 }
);
