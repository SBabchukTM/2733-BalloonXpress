using System.Collections.Generic;

namespace Core
{
    public class BaseSimplePool<T> where T : class
    {
        private readonly ILogger _logger;
        private readonly BaseFactory<T> _factory;

        private List<T> _items;
        private bool _initialize;

        public BaseSimplePool(ILogger logger,
            BaseFactory<T> factory)
        {
            _logger = logger;
            _factory = factory;
        }

        public void Initialize(int size)
        {
            if (_initialize)
                return;

            _items = new List<T>(size);

            for (int i = 0; i < size; i++)
                CreateNewItem();

            _initialize = true;
        }

        public T Get()
        {
            if (!_initialize)
            {
                _logger.Error($"{nameof(BaseSimplePool<T>)}: Pool not initialized");
                return null;
            }

            if (_items.Count == 0)
                CreateNewItem();

            var item = _items[0];
            _items.Remove(item);
            return item;
        }

        public void Return(T obj)
        {
            if (!_initialize)
            {
                _logger.Error($"{nameof(BaseSimplePool<T>)}: Pool not initialized");
                return;
            }

            _items.Add(obj as T);
        }

        public void ClearAll()
        {
            _items.Clear();
            _initialize = false;
        }

        private void CreateNewItem()
        {
            T item = _factory.Create();
            _items.Add(item);

            if (_initialize)
            {
                _logger.Error(
                    $"{nameof(BaseSimplePool<T>)}: Object created after initialization, possible memory leak");
            }
        }
    }
}