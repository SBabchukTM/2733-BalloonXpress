using System;
using UnityEngine;
using UnityEngine.Purchasing;

namespace Runtime.IAP
{
    [Serializable]
    public class ProductData
    {
        [SerializeField] private string _productId;
        [SerializeField] private ProductType _productType;

        public string ProductId => _productId;
        public ProductType ProductType => _productType;

        public ProductData(string productId, ProductType productType)
        {
            _productId = productId;
            _productType = productType;
        }
    }
}