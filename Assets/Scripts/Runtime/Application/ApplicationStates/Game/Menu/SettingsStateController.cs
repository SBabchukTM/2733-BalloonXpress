using System.Threading;
using Core.StateMachine;
using Runtime.UI;
using Cysharp.Threading.Tasks;
using ILogger = Core.ILogger;
using Runtime.Services.UserData;
using Core.Services.Audio;
using UnityEngine;
using Runtime.Services.Audio;

namespace Runtime.Game
{
    public class SettingsStateController : StateController
    {
        private readonly IUiService _uiService;
        private readonly IAudioService _audioService;
        private readonly UserDataService _userDataService;
        private readonly StartSettingsController _startSettingsController;

        private SettingsScreen _screen;

        public SettingsStateController(ILogger logger, IAudioService audioService, IUiService uiService, UserDataService userDataService, StartSettingsController startSettingsController) : base(logger)
        {
            _uiService = uiService;
            _audioService = audioService;
            _userDataService = userDataService;
            _startSettingsController = startSettingsController;
        }

        public override UniTask Enter(CancellationToken cancellationToken)
        {
            CreateScreen();
            SubscribeToEvents();

            return UniTask.CompletedTask;
        }

        public override async UniTask Exit()
        {
            await _uiService.HideScreen(ConstScreens.SettingsScreen);
        }

        private void CreateScreen()
        {
            _screen = _uiService.GetScreen<SettingsScreen>(ConstScreens.SettingsScreen);
            _screen.Initialize(_userDataService.GetUserData().SettingsData);
            _screen.ShowAsync().Forget();
        }

        private void SubscribeToEvents()
        {
            _screen.OnBackPressed += async () => await GoTo<MenuStateController>();
            _screen.OnSoundChanged += OnChangeSoundVolume;
            _screen.OnMusicChanged += OnChangeMusicVolume;
            _screen.OnVibrationChanged += OnChangeVibration;
        }

        private void OnChangeSoundVolume(bool state)
        {
            _audioService.SetVolume(Core.Services.Audio.AudioType.Sound, state ? 1 : 0);
            var userData = _userDataService.GetUserData();
            userData.SettingsData.IsSoundVolume = state;

            _audioService.PlaySound(ConstAudio.PressButtonSound);
        }

        private void OnChangeMusicVolume(bool state)
        {
            _audioService.SetVolume(Core.Services.Audio.AudioType.Music, state ? 1 : 0);
            var userData = _userDataService.GetUserData();
            userData.SettingsData.IsMusicVolume = state;

            _audioService.PlaySound(ConstAudio.PressButtonSound);
        }

        private void OnChangeVibration(bool state)
        {
            var userData = _userDataService.GetUserData();
            userData.SettingsData.IsVibration = state;

            _audioService.PlaySound(ConstAudio.PressButtonSound);

            if (state)
                Handheld.Vibrate();
        }
    }
}