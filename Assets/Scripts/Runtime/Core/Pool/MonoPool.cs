using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Core
{
    public class MonoPool<TView> where TView : MonoBehaviour
    {
        private readonly ILogger _logger;

        private BaseAsyncFactory<TView> _factory;
        private List<TView> _items;
        private bool _initialize;
        private Transform _container;

        public MonoPool(ILogger logger)
        {
            _logger = logger;
        }

        public async UniTask Initialize(BaseAsyncFactory<TView> factory, int poolSize)
        {
            if (_initialize)
                return;

            _container = new GameObject("Container").transform;
            _factory = factory;
            _items = new List<TView>(poolSize);

            for (int i = 0; i < poolSize; i++)
                await CreateNewItem();

            _initialize = true;
        }

        public async UniTask<TView> Get()
        {
            if (!_initialize)
            {
                _logger.Error($"Pool not initialized");
                return null;
            }

            if (_items.Count == 0)
                await CreateNewItem();

            var item = _items[0];
            _items.Remove(item);
            item.gameObject.SetActive(true);
            return item;
        }

        public void Return(TView item)
        {
            if (!_initialize)
            {
                _logger.Error($"Pool not initialized");
                return;
            }

            item.gameObject.SetActive(false);
            item.transform.SetParent(_container);
            _items.Add(item);
        }

        public void ClearAll()
        {
            Object.Destroy(_container.gameObject);
            _items.Clear();
            _initialize = false;
        }

        private async UniTask CreateNewItem()
        {
            TView item = await _factory.CreateAsync();
            item.gameObject.SetActive(false);
            _items.Add(item);
            item.transform.SetParent(_container);
        }
    }
}