namespace CheckoutClassLibrary
{
    public interface ICheckout
    {
        void Scan(string item);
        int GetTotalPrice();
        void SessionComplete();
        List<SKUItem> GetSKUs(bool forceRefresh = false);
        List<SpecialPrice>? GetSpecialPrices();
    }
}
