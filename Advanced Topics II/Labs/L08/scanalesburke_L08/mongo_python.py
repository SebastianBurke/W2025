from pymongo import MongoClient
from pprint import pprint

try:
    client = MongoClient("mongodb://csdevdb.cegep-heritage.qc.ca:27017")
    db = client["scanalesburke_db"]

    cars_collection = db["cars"]
    employees_collection = db["employees"]

    def find_z(collection):
        print("\nCars with 'z' in model:")
        cursor = collection.find({"model": {"$regex": "z", "$options": "i"}}, {"_id": 0, "model": 1, "manufacturer": 1, "year": 1})
        for doc in cursor:
            pprint(doc)

    def list_employees(collection):
        print("\nEmployees sorted by last name:")
        cursor = collection.find({}, {"name.last_name": 1, "name.first_name": 1, "salary": 1, "_id": 0}).sort("name.last_name", 1)
        for doc in cursor:
            pprint(doc)

    def update_employee(collection, last_name):
        print(f"\n💰 Updating salary for '{last_name}' by $20,000:")

        existing_employee = collection.find_one({ "name.last_name": last_name })
        
        if existing_employee:
            if "salary" not in existing_employee:
                print(f"⚠️ Employee '{last_name}' found, but no salary field exists. Setting default salary to 0.")
                collection.update_one(
                    { "name.last_name": last_name },
                    { "$set": { "salary": 0 } }
                )

            collection.update_one(
                { "name.last_name": last_name },
                { "$inc": { "salary": 20000 } }
            )
            updated_doc = collection.find_one({ "name.last_name": last_name }, {"_id": 0})
            pprint(updated_doc)
        else:
            print(f"⚠️ Employee with last name '{last_name}' not found.")

    find_z(cars_collection)
    list_employees(employees_collection)
    update_employee(employees_collection, "Maine")

except Exception as e:
    print(f"❌ Error: {e}")
