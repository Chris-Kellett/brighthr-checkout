using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CheckoutClassLibrary
{
    public static class Data
    {
        // SKUCache :: Holds the last obtained Dictionary of <SKU, Price>
        private static Dictionary<string, int> SKUCache = new Dictionary<string, int>();

        /// <summary>
        /// Returns a Dictionary of <SKU, Price> containing all known SKUs.
        /// </summary>
        /// <param name="forceRefresh">False by default, if True will forcibly obtain new data before returning.</param>
        /// <returns>Dictionary <SKU, Price></returns>
        public static Dictionary<string, int> SKUs(bool forceRefresh = false)
        {
            // Do we need to obtain new Data? This will either be on the first instance of the SKUs being requested
            // or the calling function specifically requesting new data. Data is not automatically obtained each
            // time to account for future cases where the SKU list is large, as this could impact performance.
            if (!Config.CacheSKUs || (SKUCache.Count == 0 || forceRefresh))
            {
                UpdateSKUs();
            }

            return SKUCache;
        }

        /// <summary>
        /// Updates the SKUCache private dictionary with new values. This function is separated to account for potential
        /// future changes in where this data will be obtained. (ie. Movement to Database driven data)
        /// </summary>
        private static void UpdateSKUs()
        {
            string itemPricesJson;
            try
            {
                itemPricesJson = File.ReadAllText(Config.ItemPricesJson);
                Dictionary<string, Dictionary<string, int>>? skuCache = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, int>>>(itemPricesJson);
                if (skuCache != null)
                {
                    SKUCache = skuCache["ItemPrices"];
                }
                Logging.Debug("UpdateSKUs completed successfully");
            }
            catch (Exception ex) 
            { 
                Logging.Error(ex);
            }
        }
    }

}
