using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckoutClassLibrary
{
    public static class Config
    {
        // DebugMode :: When disabled, Debug logs are not written and no entries are written to the Console.
        public static bool DebugMode = true;

        // LogLocation :: The Location to write Logging Text files on the instance's local machine.
        public static string LogLocation = "C:/DebugLogs/CheckoutClassLibrary/";

        // CacheSKUs :: When enabled, performance is improved by caching the SKU Dictionary for future use.
        //              This can be overwritten by providing "forceRefresh = true" when obtaining the SKUs Dictionary.
        public static bool CacheSKUs = true;

        // These are the locations of the Prices data in the Class Library.
        public static string ItemPricesJson = "Data/Prices/ItemPrices.json";
        public static string SpecialPricesJson = "Data/Prices/SpecialPrices.json";
    }
}
