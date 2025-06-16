using System.Threading;
using Core.StateMachine;
using Runtime.UI;
using Cysharp.Threading.Tasks;
using ILogger = Core.ILogger;
using Runtime.Services.UserData;
using Core;
using Zenject;
using Runtime.Services;

namespace Runtime.Game
{
    public class LevelSelectionStateController : StateController, IInitializable
    {
        private readonly IUiService _uiService;
        private readonly IAssetProvider _assetProvider;
        private readonly UserDataService _userDataService;
        private readonly GameplayStateController _gameplayStateController;
        private readonly StartSettingsController _startSettingsController;

        private LevelSelectionScreen _screen;
        private GameLevelsConfig _gameLevelsConfig;

        public LevelSelectionStateController(ILogger logger, IUiService uiService, IAssetProvider assetProvider, UserDataService userDataService, GameplayStateController gameplayStateController) : base(logger)
        {
            _uiService = uiService;
            _assetProvider = assetProvider;
            _userDataService = userDataService;
            _gameplayStateController = gameplayStateController;
        }

        public async void Initialize()
        {
            _gameLevelsConfig = await _assetProvider.Load<GameLevelsConfig>(ConstConfigs.GameLevelsConfig);
        }

        public override UniTask Enter(CancellationToken cancellationToken)
        {
            CreateScreen();
            SubscribeToEvents();

            return UniTask.CompletedTask;
        }

        public override async UniTask Exit()
        {
            UnsubscribeToEvents();
            await _uiService.HideScreen(ConstScreens.LevelSelectionScreen);
        }

        private void CreateScreen()
        {
            _screen = _uiService.GetScreen<LevelSelectionScreen>(ConstScreens.LevelSelectionScreen);
            _screen.Initialize(_gameLevelsConfig, _userDataService.GetUserData().UserInventory.Balance);
            _screen.ShowAsync().Forget();
        }

        private void SubscribeToEvents()
        {
            _screen.OnBackPressed += async () => await GoTo<MenuStateController>();
            _screen.OnLockedLevelPressed += ShowMessagePopup;
            _screen.OnPlayPressed += async (level) =>
            {
                _gameplayStateController.SetLevel(level);
                await GoTo<GameplayStateController>();
            };
        }
        private void UnsubscribeToEvents()
        {
            _screen.OnLockedLevelPressed -= ShowMessagePopup;
        }

        private void ShowMessagePopup()
        {
            var messagePopup = _uiService.GetPopup<MessagePopup>(ConstPopups.MessagePopup);
            var messagePopupData = new MessagePopupData() { Message = "not enough money", IsShowButton = true };
            messagePopup.Show(messagePopupData);
        }
    }
}