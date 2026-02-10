using ObservableCollections;

namespace Asteroids.Logic.Payments.Core
{
    public interface IPaymentService
    {
        public IReadOnlyObservableList<string> BoughtProducts { get; }
        public void BuyProduct(string productID);
        public void CheckProduct(string productID);
    }
}