import pandas as pd
import os

input_file = "Death_rates_for_suicide__by_sex__race__Hispanic_origin__and_age__United_States.csv"
output_file = "cleaned_suicide_data.csv"

df_raw = pd.read_csv(input_file)

# Drop irrelevant columns
df = df_raw.drop(columns=[
    "INDICATOR", "UNIT", "UNIT_NUM", "STUB_NAME_NUM",
    "STUB_LABEL_NUM", "YEAR_NUM", "FLAG"
], errors='ignore')

# Rename columns
df = df.rename(columns={
    "STUB_NAME": "GroupType",
    "STUB_LABEL": "GroupDescription",
    "AGE": "AgeGroup",
    "AGE_NUM": "AgeGroupCode",
    "ESTIMATE": "SuicideRate"
})

# Drop missing rates
df = df[df["SuicideRate"].notna()]

# Known value sets for classification
sex_values = {"Male", "Female", "All persons"}
ethnicity_values = {"Hispanic or Latino", "Not Hispanic or Latino"}
race_values = {
    "White", "Black or African American", "Asian or Pacific Islander",
    "American Indian or Alaska Native", "Two or more races"
}
age_keywords = ["years", "under", "over", "to"]

# Clean and assign values
def parse_components(group_desc):
    parts = str(group_desc).split(": ")
    sex = race = ethnicity = age = None

    for p in parts:
        p = p.strip()
        if p in sex_values:
            sex = p
        elif p in ethnicity_values:
            ethnicity = p
        elif p in race_values:
            race = p
        elif any(k in p.lower() for k in age_keywords):
            age = p
        else:
            if not race:
                race = p

    return pd.Series([sex, race, ethnicity, age])

df[["Sex", "Race", "Ethnicity", "DetailedAgeGroup"]] = df["GroupDescription"].apply(parse_components)

df_final = df[[
    "YEAR", "Sex", "Race", "Ethnicity", "AgeGroup", "SuicideRate"
]].copy()

df_final.to_csv(output_file, index=False)
print(f"✅ Cleaned and fixed data saved to: {output_file}")
