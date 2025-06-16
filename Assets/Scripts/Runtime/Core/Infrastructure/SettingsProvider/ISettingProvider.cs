using Cysharp.Threading.Tasks;

namespace Core
{
    public interface ISettingProvider
    {
        UniTask Initialize();
        T Get<T>() where T : BaseSettings;
        void Set(BaseSettings config);
    }
}