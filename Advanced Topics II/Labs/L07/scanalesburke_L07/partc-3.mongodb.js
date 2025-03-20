use("scanalesburke_db")
db.movies.find(
  { Actors: { $in: ["Bradley Cooper", "Jennifer Lawrence"] } }
).count();
