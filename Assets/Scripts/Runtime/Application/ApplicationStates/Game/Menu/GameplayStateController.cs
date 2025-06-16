using System.Threading;
using Runtime.Services;
using Runtime.Services.Audio;
using Runtime.Services.UserData;
using Runtime.UI;
using Core;
using Core.Services.Audio;
using Core.StateMachine;
using Core.UI;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;
using ILogger = Core.ILogger;

namespace Runtime.Game
{
    public class GameplayStateController : StateController, IInitializable
    {
        private readonly IUiService _uiService;
        private readonly IAudioService _audioService;
        private readonly UserDataService _userDataService;
        private readonly IAssetProvider _assetProvider;

        private GameplayScreen _screen;
        private CardsController _cardsController;
        private GameLevelsConfig _gameLevelsConfig;
        private UserInventory _userInventory;

        private bool _gameEnded;
        private int _level;
        private int _bet;

        public GameplayStateController(ILogger logger, IUiService uiService, IAudioService audioService, IAssetProvider assetProvider,
                UserDataService userDataService) : base(logger)
        {
            _uiService = uiService;
            _assetProvider = assetProvider;
            _audioService = audioService;
            _userDataService = userDataService;
        }

        public async void Initialize()
        {
            _gameLevelsConfig = await _assetProvider.Load<GameLevelsConfig>(ConstConfigs.GameLevelsConfig);

            _userInventory = _userDataService.GetUserData().UserInventory;
        }

        public override UniTask Enter(CancellationToken cancellationToken = default)
        {

            _bet = _gameLevelsConfig.LevelConfigs[_level].Bet;
            _userDataService.GetUserData().UserInventory.Balance -= _bet;
            _userInventory = _userDataService.GetUserData().UserInventory;
            _gameEnded = false;

            CreateScreen();
            _cardsController = _screen.GetCardsController();
            _cardsController.Initialization(_gameLevelsConfig.LevelConfigs[_level]);
            _cardsController.StartGame();
            SubscribeToEvents();

            return UniTask.CompletedTask;
        }

        public override async UniTask Exit()
        {
            await _cardsController.CancelGame();
            await _uiService.HideScreen(ConstScreens.GameplayScreen);
        }

        public void SetLevel(int level) =>
                _level = level;

        private void CreateScreen()
        {
            _screen = _uiService.GetScreen<GameplayScreen>(ConstScreens.GameplayScreen);
            _screen.Initialize();
            _screen.ShowAsync().Forget();
            _screen.SetBalance(_userInventory.Balance);
            _screen.SetBet(_bet);
            _screen.SetLevel(_level);
        }

        private void SubscribeToEvents()
        {
            _screen.OnBackPressed += async () => await GoTo<LevelSelectionStateController>();
            _cardsController.OnGameOverEvent += ProcessGameEnd;
        }

        private async void ShowGameEndPopup(int bet, int win, bool lockContinue)
        {
            Time.timeScale = 0;

            var winPopup = await _uiService.ShowPopup(ConstPopups.WinPopup) as WinPopup;
            winPopup.SetData(bet, win);

            if (lockContinue)
                winPopup.LockContinue();

            winPopup.OnContinuePressed += async () =>
            {
                Time.timeScale = 1;
                winPopup.DestroyPopup();
                await GoTo<GameplayStateController>();
            };

            winPopup.OnHomePressed += async () =>
            {
                Time.timeScale = 1;
                winPopup.DestroyPopup();
                await GoTo<MenuStateController>();
            };
        }

        private void ProcessGameEnd(int winAmount)
        {
            if (_gameEnded)
                return;

            _gameEnded = true;

            _userInventory.Balance += winAmount;
            _audioService.PlaySound(_bet > winAmount ? ConstAudio.LoseSound : ConstAudio.VictorySound);

            ShowGameEndPopup(_bet, winAmount, _userInventory.Balance < _bet);
        }

        private void GameStart()
        {
            if (_userInventory.Balance < _bet)
                GoTo<LevelSelectionStateController>().Forget();

            _screen.DisableBackButton();

            _userInventory.Balance -= _bet;
            _screen.SetBalance(_userInventory.Balance);
        }
    }
}