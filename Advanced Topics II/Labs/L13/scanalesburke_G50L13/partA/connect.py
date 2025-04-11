from pymongo import MongoClient

def by_pizza_type(collection):
    pipeline = [
        {
            "$group": {
                "_id": "$type",
                "total_quantity": {"$sum": "$quantity"},
                "total_cost": {
                    "$sum": { "$multiply": ["$quantity", "$priceper"] }
                }
            }
        },
        {
            "$sort": {"_id": 1}
        }
    ]

    result = collection.aggregate(pipeline)

    print(f'{"Pizza Type":<16}{"Total Quantity":^18}{"Total Cost":^12}')
    print("-" * 46)

    for item in result:
        print(f'{item["_id"]:<16}{item["total_quantity"]:^18}{item["total_cost"]:^12.2f}')


def main():
    client = MongoClient("mongodb://csdevdb.cegep-heritage.qc.ca:27017")
    db = client["scb_db"]
    collection = db["orders"]

    by_pizza_type(collection)

if __name__ == "__main__":
    main()
