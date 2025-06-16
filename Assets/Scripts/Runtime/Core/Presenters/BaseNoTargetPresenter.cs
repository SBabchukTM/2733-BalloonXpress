using Cysharp.Threading.Tasks;

namespace Core
{
    public abstract class BaseNoTargetPresenter : BasePresenter
    {
        public abstract UniTask Show();
        public abstract UniTask Hide();
    }
}