using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine.Purchasing;

namespace Runtime.IAP
{
    public interface IIAPService
    {
        void Initialize(List<ProductData> products);
        Product GetProductById(string productId);
        UniTask<bool> TryBuyProduct(string productId);
        bool IsProductPurchased(string productId);
        bool IsInitialized();
        string GetProductPrice(string productId);
    }
}