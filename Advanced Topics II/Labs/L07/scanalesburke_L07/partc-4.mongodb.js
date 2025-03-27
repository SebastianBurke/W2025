use("scanalesburke_db")
db.movies.find(
  { Genre: { $nin: ["Action", "Adventure"] } }
).count();
