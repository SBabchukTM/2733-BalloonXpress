using Cysharp.Threading.Tasks;

namespace Core
{
    public abstract class BaseAsyncFactory<T>
    {
        public abstract UniTask<T> CreateAsync();
    }
}