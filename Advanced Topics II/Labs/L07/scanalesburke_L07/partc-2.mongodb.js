use("scanalesburke_db")
db.movies.find(
  { Actors: { $all: ["Bradley Cooper", "Jennifer Lawrence"] } }
).count();
