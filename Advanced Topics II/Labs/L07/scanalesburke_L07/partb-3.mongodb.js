use("scanalesburke_db")
db.listings.find(
  { "address.country": "Canada", "host.is_superhost": true },
  { name: 1, listing_url: 1, address: 1, host: 1, number_of_reviews: 1, amenities: 1, _id: 0 }
);
