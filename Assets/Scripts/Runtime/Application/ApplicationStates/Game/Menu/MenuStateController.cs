using System.Threading;
using Core.StateMachine;
using Runtime.UI;
using Cysharp.Threading.Tasks;
using ILogger = Core.ILogger;
using Core.UI;
using Runtime.Services.UserData;
using System.Threading.Tasks;

namespace Runtime.Game
{
    public class MenuStateController : StateController
    {
        private readonly IUiService _uiService;
        private readonly UserDataService _userDataService;

        private MenuScreen _menuScreen;
        private MenuPopup _menuPopup;

        public MenuStateController(ILogger logger, IUiService uiService, UserDataService userDataService) : base(logger)
        {
            _uiService = uiService;
            _userDataService = userDataService;
        }

        public override UniTask Enter(CancellationToken cancellationToken)
        {
            CreateScreen();
            SubscribeToEvents();
            return UniTask.CompletedTask;
        }

        public override async UniTask Exit()
        {
            if (_menuPopup != null)
                _menuPopup.DestroyPopup();

            _menuScreen.OnMenuPressed -= OpenMenuPopup;
            await _uiService.HideScreen(ConstScreens.MenuScreen);
        }

        private void CreateScreen()
        {
            _menuScreen = _uiService.GetScreen<MenuScreen>(ConstScreens.MenuScreen);
            _menuScreen.Initialize(_userDataService.GetUserData().UserInventory.Balance);
            _menuScreen.ShowAsync().Forget();
        }

        private void SubscribeToEvents()
        {
            _menuScreen.OnPlayPressed += async () => await GoTo<LevelSelectionStateController>();
            _menuScreen.OnMenuPressed += OpenMenuPopup;
        }

        private async void OpenMenuPopup()
        {
            _menuPopup = await _uiService.ShowPopup(ConstPopups.MenuPopup) as MenuPopup;
            _menuPopup.OnAccountPressed += async () => await GoTo<AccountStateController>();
            _menuPopup.OnLeaderboardPressed += async () => await GoTo<LeaderboardStateController>();
            _menuPopup.OnSettingsPressed += async () => await GoTo<SettingsStateController>();
            _menuPopup.OnPrivacyPolicyPressed += async () =>
            {
                await _uiService.ShowPopup(ConstPopups.PrivacyPolicyPopup);
                _menuPopup.DestroyPopup();
            };

            _menuPopup.OnRulesPressed += async () =>
            {
                await _uiService.ShowPopup(ConstPopups.RulesPopup);
                _menuPopup.DestroyPopup();
            };

            _menuPopup.OnClosePressed += _menuPopup.DestroyPopup;
        }
    }
}