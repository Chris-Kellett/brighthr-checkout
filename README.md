# Checkout Class Library
## Users
This library provides functionality to Scan and retrieve the Total Price of a transaction. The following methods are available for use:
- `Scan(string item)`: Provide the SKU to denote that this item has been scanned.
- `GetTotalPrice()`: Returns an int value of the total price, factoring in any Special Prices.
- `SessionComplete()`: Denotes to the Class Library that the transaction has been completed, which will reset the stored SKU List.

Additionally, the following functions are available to use for data verification:
- `GetSKUs()`: Returns a list of all SKUs currently available.
- `GetSpecialPrices()`: Returns a list of all currently active Special Prices.

## Developers
For information on the structure of this Class Library please read the [Developer's Readme](Developers.md)