using System.Threading;
using Cysharp.Threading.Tasks;
using Runtime.Game;
using Core.StateMachine;
using ILogger = Core.ILogger;

namespace Runtime.GameStateMachine
{
    public class GameState : StateController
    {
        private readonly StateMachine _stateMachine;

        private readonly AccountStateController _accountStateController;
        private readonly GameplayStateController _gameplayStateController;
        private readonly LeaderboardStateController _leaderboardStateController;
        private readonly LevelSelectionStateController _levelSelectionStateController;
        private readonly MenuStateController _menuStateController;
        private readonly SettingsStateController _settingsStateController;
        private readonly UserDataStateChangeController _userDataStateChangeController;

        public GameState(ILogger logger,
            AccountStateController accountStateController,
            GameplayStateController gameplayStateController,
            LeaderboardStateController leaderboardStateController,
            LevelSelectionStateController levelSelectionStateController,
            MenuStateController menuStateController,
            SettingsStateController settingsStateController,
            StateMachine stateMachine,
            UserDataStateChangeController userDataStateChangeController) : base(logger)
        {
            _stateMachine = stateMachine;
            _accountStateController = accountStateController;
            _gameplayStateController = gameplayStateController;
            _leaderboardStateController = leaderboardStateController;
            _levelSelectionStateController = levelSelectionStateController;
            _menuStateController = menuStateController;
            _settingsStateController = settingsStateController;
            _userDataStateChangeController = userDataStateChangeController;
        }

        public override async UniTask Enter(CancellationToken cancellationToken)
        {
            await _userDataStateChangeController.Run(default);

            _stateMachine.Initialize(_accountStateController, _gameplayStateController,
                                     _leaderboardStateController, _levelSelectionStateController,
                                     _menuStateController, _settingsStateController);

            _stateMachine.GoTo<MenuStateController>().Forget();
        }
    }
}