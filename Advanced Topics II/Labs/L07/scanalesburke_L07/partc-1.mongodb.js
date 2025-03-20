use("scanalesburke_db")
db.movies.find(
  { Actors: "Bradley Cooper" }
).count();
