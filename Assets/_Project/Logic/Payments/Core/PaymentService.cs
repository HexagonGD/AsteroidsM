using Cysharp.Threading.Tasks;
using ObservableCollections;
using System.Collections.Generic;
using UnityEngine.Purchasing;
using Zenject;

using Debug = UnityEngine.Debug;

namespace Asteroids.Logic.Payments.Core
{
    public class PaymentService : IInitializable
    {
        private StoreController _storeController;
        private ObservableList<string> _boughtProducts = new();

        public IReadOnlyObservableList<string> BoughtProducts => _boughtProducts;

        public void Initialize()
        {
            InitializeIAP().Forget();
        }

        public void BuyProduct(string productID)
        {
            _storeController.PurchaseProduct(productID);
        }

        public void CheckProduct(string productID)
        {
            _storeController.CheckEntitlement(_storeController.GetProductById(productID));
        }

        private async UniTask InitializeIAP()
        {
            _storeController = UnityIAPServices.StoreController();

            _storeController.OnPurchasePending += PurchasePendingHandler;
            _storeController.OnProductsFetched += ProductsFetchedHandler;
            _storeController.OnPurchasesFetched += PurchasesFetchedHandler;
            _storeController.OnStoreDisconnected += StoreDisconnectedHandler;

            await _storeController.Connect();


            var initialProductsToFetch = new List<ProductDefinition>
        {
            new("disable_ads", ProductType.NonConsumable)
        };

            _storeController.FetchProducts(initialProductsToFetch);
        }

        private void PurchasePendingHandler(PendingOrder order)
        {
            ProcessPurchase(order);
            _storeController.ConfirmPurchase(order);
        }

        private void ProductsFetchedHandler(List<Product> products)
        {
            _storeController.FetchPurchases();
        }

        private void PurchasesFetchedHandler(Orders orders)
        {
            foreach (var confirmedOrder in orders.ConfirmedOrders)
            {
                ProcessPurchase(confirmedOrder);
            }
        }

        private void StoreDisconnectedHandler(StoreConnectionFailureDescription description)
        {
            Debug.Log("Store disconnected");
        }

        private void ProcessPurchase(Order order)
        {
            var productID = order.Info.PurchasedProductInfo[0].productId;

            if (IsNonConsumable(productID) && _boughtProducts.Contains(productID) == false)
            {
                _boughtProducts.Add(productID);
            }
        }

        private bool IsNonConsumable(string productID)
        {
            var product = _storeController.GetProductById(productID);
            return product.definition.type == ProductType.NonConsumable;
        }
    }
}