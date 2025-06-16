using System.Threading;
using Cysharp.Threading.Tasks;

namespace Core.StateMachine
{
    public abstract class StateController
    {
        private StateMachine _stateMachine;

        protected readonly ILogger Logger;

        protected StateController(ILogger logger)
        {
            Logger = logger;
        }

        public void Initialize(StateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public abstract UniTask Enter(CancellationToken cancellationToken = default);

        public virtual UniTask Exit()
        {
            return UniTask.CompletedTask;
        }

        protected async UniTask GoTo<T>(CancellationToken cancellationToken = default) where T : StateController
        {
            await _stateMachine.GoTo<T>(cancellationToken);
        }

        protected async UniTask Finish()
        {
            await _stateMachine.Finish();
        }
    }
}