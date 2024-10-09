using CheckoutClassLibrary;
using NUnit.Framework.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class Checkout : ICheckout
{
    public int GetTotalPrice()
    {
        // First get our Current Cart SKU items and the current Special Prices list
        List<SKUItem>? cartItems = Data.GetCart();
        List<SpecialPrice>? specialPrices = Data.SpecialPrices();

        // Quick validation to make sure we have scanned items
        if (cartItems == null || cartItems.Count == 0) return 0;

        // Initialise the Total Price to be populated via Specials and stand-alone SKUs
        int totalPrice = 0;

        // Initialise a collection of SKUs which have had Special Prices checked/applied to them. We'll
        // later revisit this to add any SKUs in the Cart which are not affected by Special Pricing.
        List<string> specialSkusApplied = new List<string>();

        // If we have any SpecialPrices, we will process them down this route. This will apply as many Special Prices
        // as possible then add a standard price for any quantities left over for each SKU.
        if (specialPrices !=  null && specialPrices.Count > 0)
        {
            // Get a list of all SKUs affected by a Special Price first, we will then calculate each potential special
            // price for a SKU to calculate the lowest available price for the total price. Doing it in this way accounts
            // for potential future conflicts where a SKU has multiple Special Prices, with more than one being applicable
            // to our current transaction.
            List<string?> distinctSKUs = specialPrices.Select(s => s.SKU).Distinct().ToList();

            // Now loop each SKU which has a Special Price
            foreach(string? sku in distinctSKUs)
            {
                // First account for the Nullable string
                if (sku == null || sku == "") continue;

                // Add this to the Processed List of Special Price SKUs
                specialSkusApplied.Add(sku);

                // Now get all matching Special Prices associated with the SKU
                List<SpecialPrice> skuSpecials = specialPrices.Where(s => s.SKU == sku).ToList();

                // Get all SKUItems from the Cart associated with the SKU we're currently processing, make sure we have
                // at least 1 entry of this SKU in the Cart
                List<SKUItem> cartSkus = cartItems.Where(c => c.SKU == sku).ToList();
                int? minQuantity = skuSpecials.OrderBy(s => s.Quantity).First().Quantity;
                int cartCount = cartSkus.Count();
                if (cartCount == 0) continue;

                // If we're here, we have enough SKUs in our Cart that there is at least one special applicable.
                // Now we need to work out an order to apply these special prices, with the best value specials being
                // applied first. We'll add each Special Price to the Dictionary below with a float key, which will 
                // denote the calculated price per item factoring in the special item price.
                Dictionary<float, SpecialPrice> valueCalculation = new Dictionary<float, SpecialPrice>();
                foreach(SpecialPrice skuSpecial in skuSpecials)
                {
                    float? value = (float?)skuSpecial.Price / skuSpecial.Quantity;
                    valueCalculation.Add(value ?? 0, skuSpecial);
                }

                // Sort the Specials we have by their calculated Value
                List<KeyValuePair<float, SpecialPrice>> valueSortedSpecials = valueCalculation.OrderBy(kvp => kvp.Key).ToList();

                // Now we have a Value sorted list, we can apply these specials in this order to SKUs in the Cart
                foreach (KeyValuePair<float, SpecialPrice> special in valueSortedSpecials)
                {
                    // Keep applying this Special until we don't have the Quantity remaining to apply any more
                    while (cartCount >= special.Value.Quantity)
                    {
                        totalPrice += special.Value.Price ?? 0;
                        cartCount -= special.Value.Quantity ?? 0;
                    }
                }

                // Now ensure that any remaining SKUs which fall outside of the Special Quantities are applied to the Total Price
                if (cartCount > 0)
                {
                    totalPrice += cartCount * cartSkus[0].Price ?? 0;
                }
            }


        }

        // Now we've processed any available Special Prices, we need to add SKUs which did not have any Special Prices
        // to the Total Price.
        List<SKUItem> remainingItems = cartItems.Where(s => !specialSkusApplied.Contains(s.SKU ?? "")).ToList();
        foreach (SKUItem skuItem in remainingItems)
        {
            totalPrice += skuItem.Price ?? 0;
        }
        
        Logging.Event($"Cart containing {cartItems.Count()} items was calculated at {totalPrice}");
        return totalPrice;
    }
}