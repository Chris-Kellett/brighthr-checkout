namespace CheckoutClassLibrary
{
    public interface ICheckout
    {
        void Scan(string item);
        int GetTotalPrice();
        void SessionComplete();
    }
}
