using CheckoutClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class Checkout : ICheckout
{
    public void Scan(string item)
    {
        // Get the list of SKUs from the Data class
        List<SKUItem> SKUList = Data.SKUs();

        // See if we have a match for the Provided SKU
        SKUItem? matchedItem = SKUList.Where(s => s.SKU == item).FirstOrDefault();

        // Depending on whether we have a match, add the SKU to our session's Cart or raise an Error.
        if (matchedItem != null)
        {
            Data.AddToCart(matchedItem);
        }
        else
        {
            Logging.Error(new Exception($"Scanned SKU: [{item}] was not found in the SKU List"));
        }
    }
}
