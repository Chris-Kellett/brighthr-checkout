using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckoutClassLibrary.Tests
{
    [TestFixture]
    public class SpecialTests
    {
        private ICheckout _checkout;
        private List<SKUItem>? _skuItems;
        private List<SpecialPrice>? _specialPrices;

        [SetUp]
        public void Setup()
        {
            _checkout = new Checkout();
            _skuItems = _checkout.GetSKUs();
            _specialPrices = _checkout.GetSpecialPrices();
        }

        [Test]
        public void OneSpecialOnly()
        {
            if (_specialPrices != null)
            {
                foreach (SpecialPrice special in _specialPrices)
                {
                    int totalPrice = special.Price ?? 0;
                    _checkout.SessionComplete();
                    for (int i = 0; i < special.Quantity; i++)
                    {
                        _checkout.Scan(special.SKU ?? "");
                    }

                    int total = _checkout.GetTotalPrice();
                    Assert.That(total, Is.EqualTo(totalPrice));
                }
            }
            else
            {
                Assert.Fail("_specialPrices is NULL");
            }
        }

        [Test]
        public void MultipleDifferentSpecials()
        {
            if (_specialPrices != null)
            {
                _checkout.SessionComplete();
                int totalPrice = 0;
                foreach (SpecialPrice special in _specialPrices)
                {
                    totalPrice += special.Price ?? 0;
                    for (int i = 0; i < special.Quantity; i++)
                    {
                        _checkout.Scan(special.SKU ?? "");
                    }
                }
                int total = _checkout.GetTotalPrice();
                Assert.That(total, Is.EqualTo(totalPrice));
            }
            else
            {
                Assert.Fail("_specialPrices is NULL");
            }
        }

        [Test]
        public void SpecialsAndRegularItems()
        {
            if (_skuItems == null)
            {
                Assert.Fail("_skuItems is NULL");
                return;
            }

            if (_specialPrices != null)
            {
                _checkout.SessionComplete();
                int totalPrice = 0;
                List<string> specialSkus = new List<string>();
                foreach (SpecialPrice special in _specialPrices)
                {
                    specialSkus.Add(special.SKU ?? "");
                    totalPrice += special.Price ?? 0;
                    for (int i = 0; i < special.Quantity; i++)
                    {
                        _checkout.Scan(special.SKU ?? "");
                    }
                }

                List<SKUItem> nonSpecials = _skuItems.Where(s => !specialSkus.Contains(s.SKU ?? "")).ToList();
                foreach (SKUItem nonSpecial in nonSpecials)
                {
                    _checkout.Scan(nonSpecial.SKU ?? "");
                    totalPrice += nonSpecial.Price ?? 0;
                }

                int total = _checkout.GetTotalPrice();
                Assert.That(total, Is.EqualTo(totalPrice));
            }
            else
            {
                Assert.Fail("_specialPrices is NULL");
            }
        }

        [Test]
        public void MultipleMatchingSpecials()
        {
            if (_specialPrices != null)
            {
                _checkout.SessionComplete();
                int totalPrice = 0;
                foreach (SpecialPrice special in _specialPrices)
                {
                    totalPrice += special.Price * 2 ?? 0;
                    for (int i = 0; i < special.Quantity * 2; i++)
                    {
                        _checkout.Scan(special.SKU ?? "");
                    }
                }
                int total = _checkout.GetTotalPrice();
                Assert.That(total, Is.EqualTo(totalPrice));
            }
            else
            {
                Assert.Fail("_specialPrices is NULL");
            }
        }

        [Test]
        public void MultipleMatchingSpecialsWithExtraItems()
        {
            if (_skuItems == null)
            {
                Assert.Fail("_specialPrices is NULL");
                return;
            }
            if (_specialPrices != null)
            {
                _checkout.SessionComplete();
                int totalPrice = 0;
                foreach (SpecialPrice special in _specialPrices)
                {
                    totalPrice += special.Price * 2 ?? 0;
                    for (int i = 0; i < special.Quantity * 2; i++)
                    {
                        _checkout.Scan(special.SKU ?? "");
                    }

                    SKUItem? specialSku = _skuItems.FirstOrDefault(s => s.SKU == special.SKU);
                    if (specialSku == null)
                    {
                        Assert.Fail("Could not find associated SKU with Special");
                        return;
                    }

                    for (int i = 0; i < special.Quantity - 1; i++)
                    {
                        _checkout.Scan(specialSku.SKU ?? "");
                        totalPrice += specialSku.Price ?? 0;
                    }
                }
                int total = _checkout.GetTotalPrice();
                Assert.That(total, Is.EqualTo(totalPrice));
            }
            else
            {
                Assert.Fail("_specialPrices is NULL");
            }
        }

        [Test]
        public void MultipleSpecialsRandomOrder()
        {
            if (_specialPrices != null)
            {
                _checkout.SessionComplete();
                List<string> scanSkus = new List<string>();
                int totalPrice = 0;
                foreach (SpecialPrice special in _specialPrices)
                {
                    totalPrice += special.Price ?? 0;
                    for (int i = 0; i < special.Quantity; i++)
                    {
                        scanSkus.Add(special.SKU ?? "");
                    }
                }

                if (scanSkus.Count == 0)
                {
                    Assert.Fail("scanSkus Array is empty");
                    return;
                }

                // As part of this Unit Test, I'm going to create a new list of SKUs which takes items from the
                // Start, then the End, of the SKU list I have into a new list until there's nothing left.
                // Doing this will give me an Array of the format A-B-A-B-A
                List<string> shuffledSkus = new List<string>();

                int start = 0;
                int end = scanSkus.Count - 1;
                bool takeFromStart = true;

                while (start <= end)
                {
                    if (takeFromStart)
                    {
                        shuffledSkus.Add(scanSkus[start]);
                        start++;
                    }
                    else
                    {
                        shuffledSkus.Add(scanSkus[end]);
                        end--;
                    }
                    takeFromStart = !takeFromStart;
                }

                foreach(string sku in shuffledSkus)
                {
                    _checkout.Scan(sku);
                }

                int total = _checkout.GetTotalPrice();
                Assert.That(total, Is.EqualTo(totalPrice));
            }
            else
            {
                Assert.Fail("_specialPrices is NULL");
            }
        }

    }
}
