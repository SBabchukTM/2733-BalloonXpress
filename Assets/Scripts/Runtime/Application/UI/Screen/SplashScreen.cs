using System.Threading;
using Cysharp.Threading.Tasks;

namespace Runtime.UI
{
    public class SplashScreen : UiScreen
    {
        public override async UniTask HideAsync(CancellationToken cancellationToken = default)
        {
            await WaitSplashScreenAnimationFinish(cancellationToken);
            await base.HideAsync(cancellationToken);
        }

        private async UniTask WaitSplashScreenAnimationFinish(CancellationToken cancellationToken)
        {
            await UniTask.Delay(2000, cancellationToken: cancellationToken);
        }
    }
}