using CheckoutClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class Checkout : ICheckout
{
    public List<SKUItem> GetSKUs(bool forceRefresh = false)
    {
        return Data.SKUs(forceRefresh);
    }
}
