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

        // LogFileName :: The File Name of the Logging files inside the "LogLocation" directory.
        public static string LogFileName = "CheckoutClassLibrary_Logs";

        // MaxLogFileSizeMb :: The Max size a Log File can be in the Log Location, if this size is exceeded a new logging
        //                     file is created.
        public static int MaxLogFileSizeMb = 1;

        // MaxLogFileCount :: The Max number of Logging files that will be stored in the LogLocation. If this number is 
        //                    reached the oldest Logging file will be removed.
        public static int MaxLogFileCount = 10;

        // CacheSKUs :: When enabled, performance is improved by caching the SKU Dictionary for future use.
        //              This can be overwritten by providing "forceRefresh = true" when obtaining the SKUs Dictionary.
        public static bool CacheSKUs = true;

        // These are the locations of the Prices data in the Class Library.
        public static string ItemPricesJson = "Data/Prices/ItemPrices.json";
        public static string SpecialPricesJson = "Data/Prices/SpecialPrices.json";
    }
}
