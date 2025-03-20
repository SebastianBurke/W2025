use("scanalesburke_db")
db.movies.find(
  { Title: { $regex: "^The" } }
).count();
