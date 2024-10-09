using CheckoutClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class Checkout : ICheckout
{
    public void SessionComplete()
    {
        Data.ResetSession();   
    }
}
