use("scanalesburke_db")
db.movies.find().sort({ Revenue: -1 }).skip(3).limit(1);
