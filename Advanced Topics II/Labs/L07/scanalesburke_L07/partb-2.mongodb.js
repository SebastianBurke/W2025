use("scanalesburke_db")
db.listings.find(
  { number_of_reviews: 0 }
).count();
