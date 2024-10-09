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
        private static List<SKUItem> SKUCache = new List<SKUItem>();

        // CurrentCart :: Holds all scanned SKUs associated with the current Session
        private static List<SKUItem> CurrentCart = new List<SKUItem>();

        /// <summary>
        /// Returns a Dictionary of <SKU, Price> containing all known SKUs.
        /// </summary>
        /// <param name="forceRefresh">False by default, if True will forcibly obtain new data before returning.</param>
        /// <returns>List of SKUItem objects</returns>
        public static List<SKUItem> SKUs(bool forceRefresh = false)
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
            try
            {
                string itemPricesJson = File.ReadAllText(Config.ItemPricesJson);
                ItemPricesList? skuCache = JsonConvert.DeserializeObject<ItemPricesList>(itemPricesJson);
                if (skuCache != null && skuCache.ItemPrices != null)
                {
                    SKUCache = skuCache.ItemPrices;
                }
                Logging.Debug("UpdateSKUs completed successfully");
            }
            catch (Exception ex) 
            { 
                Logging.Error(ex);
            }
        }

        /// <summary>
        /// Returns all current Special Prices available. This is not cached as these values are subject to change often, meaning
        /// an up to date value is always required.
        /// </summary>
        /// <returns>List of SpecialPrice objects</returns>
        public static List<SpecialPrice>? SpecialPrices()
        {
            try
            {
                string specialPricesJson = File.ReadAllText(Config.SpecialPricesJson);
                SpecialPriceList? specialPriceList = JsonConvert.DeserializeObject<SpecialPriceList>(specialPricesJson);
                if (specialPriceList != null && specialPriceList.SpecialPrices != null)
                {
                    Logging.Debug("SpecialPrices returned successfully");
                    return specialPriceList.SpecialPrices;
                }
            }
            catch (Exception ex)
            {
                Logging.Error(ex);
            }
            return null;            
        }

        /// <summary>
        /// Adds an item to the Current Cart object associated with the session
        /// </summary>
        /// <param name="item">The SKUItem object to add</param>
        public static void AddToCart(SKUItem item)
        {
            CurrentCart.Add(item);
            Logging.Event($"SKU: {item.SKU} added to current cart");
        }
    }

}
