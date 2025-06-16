using Cysharp.Threading.Tasks;

namespace Core
{
    public abstract class BaseTargetPresenter<TTarget> : BasePresenter where TTarget : class
    {
        public TTarget Target { get; private set; } = default(TTarget);

        public virtual UniTask Show(TTarget target)
        {
            Target = target;
            return UniTask.CompletedTask;
        }

        public virtual UniTask Hide()
        {
            Target = default;
            return UniTask.CompletedTask;
        }
    }
}