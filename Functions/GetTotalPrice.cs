using CheckoutClassLibrary;
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

        // First we want to account for all Special Prices
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

                // Now get all matching Special Prices associated with the SKU
                List<SpecialPrice> skuSpecials = specialPrices.Where(s => s.SKU == sku).ToList();

                // Check we can fulfil at least one of these specials from the Current Cart we have
                List<SKUItem> cartSkus = cartItems.Where(c => c.SKU == sku).ToList();
                int? minQuantity = skuSpecials.OrderBy(s => s.Quantity).First().Quantity;
                if (cartSkus.Count < minQuantity) continue;

                // If we're here, we have enough SKUs in our Cart that there is at least one special applicable.
                // Now we need to work out an order to apply these special prices, with the best value specials being
                // applied first.

            }
        }

        return 0;
    }
}