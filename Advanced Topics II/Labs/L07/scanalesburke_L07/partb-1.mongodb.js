use("scanalesburke_db")
db.listings.find(
    { number_of_reviews: { $gt: 0 } },
    { name: 1, listing_url: 1, number_of_reviews: 1, bedrooms: 1, beds: 1, _id: 0 }
  ).count();
  