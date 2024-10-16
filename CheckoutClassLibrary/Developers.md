# Developer's Readme
This file contains information regarding the structure of the project, and the function of its components.

## Config
The Config folder contains `Config.cs`, which acts as a repository for configurable global variables. 

Descriptions for each item are commented above the declaration.

## Data
The Data folder contains elements relating to the requesting and storage of Data objects within the Class Library.

### Prices/
This folder contains `.json` files which provide the `ItemPrices` and `SpecialPrices` values.
### Data.cs
This file is to house functions relating to the delivery and manipulation of data within the Class Library.

The following internal variables are stored within this file.

`SKUCache`: When the SKUCache is in operation (as defined in `Config.cs`), this is where the last SKU list is stored.

`CurrentCart`: The collection of scanned items associated with the current session.

### Enums.cs
This file houses all `enums` which are used globally within the Class Library.

## Functions
This folder contains both Public and Private functions used within the Class Library.

### GetSKUs.cs
Contains `GetSKUs()` which exposes `Data.SKUs()` publically
### GetSpecialPrices()
Contains `GetSpecialPrices()` which exposes `Data.SpecialPrices()` publically
### Checkout.cs
Contains `GetTotalPrice()`
### Logging.cs
Contains all Logging related functions used Privately within the Class Library, as well as its dependant functions used to write Log files.
### Scan.cs
Contains `Scan(string item)`
### SessionComplete.cs
Contains `SessionComplete()`

## Models
This folder contains all Model objects used within the Class Library. Each model is listed as an individual file.

`ItemPricesList`: Used to Deserialise the `ItemPrices.json` file.

`SKUItem`: The Model of an individual SKU object.

`SpecialPrice`: The Model of an individual Special Price object.

`SpecialPriceList`: Used to Deserialise the `SpecialPrices.json` file.

## Testing
### BasicTests.cs
Contains tests aimed at general functional testing with no great degree of complexity.

### SpecialTests.cs
Contains tests aimed at the Special Prices functionality.