use("scanalesburke_db")
db.cars.find().sort({ year: 1 }).skip(2).limit(1);