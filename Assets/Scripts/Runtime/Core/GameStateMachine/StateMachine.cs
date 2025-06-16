using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Core.StateMachine
{
    public class StateMachine
    {
        private Dictionary<Type, StateController> _states;
        private StateController _activeState;
        private bool IsActive => _activeState != null;

        public void Initialize(params StateController[] stateControllers)
        {
            _states = new Dictionary<Type, StateController>(stateControllers.Length);
            foreach (var executiveStateController in stateControllers)
            {
                _states.Add(executiveStateController.GetType(), executiveStateController);
                executiveStateController.Initialize(this);
            }
        }

        public async UniTask GoTo<TState>(CancellationToken cancellationToken = default) where TState : StateController
        {
            StateController state = await ChangeState<TState>();
            await state.Enter(cancellationToken);
        }

        public async UniTask Finish()
        {
            if (_activeState != null)
                await _activeState.Exit();

            _activeState = null;
        }

        private async UniTask<TState> ChangeState<TState>() where TState : StateController
        {
            if (_activeState != null)
                await _activeState.Exit();

            TState state = GetState<TState>();
            _activeState = state;

            return state;
        }

        private TState GetState<TState>() where TState : class
        {
            return _states[typeof(TState)] as TState;
        }
    }
}