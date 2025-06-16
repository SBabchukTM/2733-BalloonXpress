using System.Collections.Generic;
using Core;
using Cysharp.Threading.Tasks;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

namespace Runtime.IAP
{
    public class IAPServiceMock : IIAPService
    {
        private readonly ILogger _logger;
        public IAPServiceMock(ILogger logger)
        {
            _logger = logger;
        }

        void IIAPService.Initialize(List<ProductData> products)
        {
            _logger.Log($"{nameof(IAPServiceMock)}: initialized!");
        }

        Product IIAPService.GetProductById(string productId)
        {
            return null;
        }

        async UniTask<bool> IIAPService.TryBuyProduct(string productId)
        {
            _logger.Log($"{nameof(IAPServiceMock)}: Try buy product {productId}");

            await UniTask.Delay(2000);
            return true;
        }

        bool IIAPService.IsProductPurchased(string productId)
        {
            return false;
        }

        bool IIAPService.IsInitialized()
        {
            return true;
        }

        public string GetProductPrice(string productId)
        {
            return "UAH 123";
        }

        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
        }

        public void OnInitializeFailed(InitializationFailureReason error)
        {
        }

        public void OnInitializeFailed(InitializationFailureReason error, string message)
        {
        }

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
        {
            return PurchaseProcessingResult.Complete;
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
        {
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
        }
    }
}