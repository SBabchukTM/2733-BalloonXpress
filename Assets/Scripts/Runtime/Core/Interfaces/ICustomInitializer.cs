using Cysharp.Threading.Tasks;

namespace Core
{
    public interface ICustomInitializer
    {
        UniTask Initialize();
    }
    
    public interface ICustomInitializer<P>
    {
        UniTask Initialize(P param);
    }
}