using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckoutClassLibrary.Tests
{
    [TestFixture]
    public class BasicTests
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
        public void EmptyCart()
        {
            _checkout.SessionComplete();
            int total = _checkout.GetTotalPrice();
            Assert.That(total, Is.EqualTo(0));
        }

        [Test]
        public void OneItem()
        {
            if (_skuItems != null)
            {
                foreach (SKUItem sku in _skuItems)
                {
                    _checkout.SessionComplete();
                    _checkout.Scan(sku.SKU ?? "");
                    int total = _checkout.GetTotalPrice();
                    Assert.That(total, Is.EqualTo(sku.Price));
                }
            }
            else
            {
                Assert.Fail("_skuItems was NULL");
            }
        }

        [Test]
        public void MultipleItems()
        {
            if (_skuItems != null)
            {
                int totalPrice = 0;
                _checkout.SessionComplete();
                foreach (SKUItem sku in _skuItems)
                {
                    _checkout.Scan(sku.SKU ?? "");
                    totalPrice += sku.Price ?? 0;
                }

                int total = _checkout.GetTotalPrice();
                Assert.That(total, Is.EqualTo(totalPrice));
            }
            else
            {
                Assert.Fail("_skuItems was NULL");
            }
        }

        [Test]
        public void OnlyInvalidItems()
        {
            if (_skuItems != null)
            {
                foreach (SKUItem sku in _skuItems)
                {
                    _checkout.SessionComplete();
                    _checkout.Scan("X" + sku.SKU + "X" ?? "");
                    int total = _checkout.GetTotalPrice();
                    Assert.That(total, Is.EqualTo(0));
                }
            }
            else
            {
                Assert.Fail("_skuItems was NULL");
            }
        }

        [Test]
        public void InvalidAndValidItems()
        {
            if (_skuItems != null)
            {
                foreach (SKUItem sku in _skuItems)
                {
                    _checkout.SessionComplete();
                    _checkout.Scan(sku.SKU ?? "");
                    _checkout.Scan("X" + sku.SKU + "X" ?? "");
                    int total = _checkout.GetTotalPrice();
                    Assert.That(total, Is.EqualTo(sku.Price));
                }
            }
            else
            {
                Assert.Fail("_skuItems was NULL");
            }
        }
    }
}
