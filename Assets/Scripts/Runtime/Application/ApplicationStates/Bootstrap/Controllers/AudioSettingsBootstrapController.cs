using System.Collections.Generic;
using System.Threading;
using Runtime.Services.UserData;
using Core;
using Core.Services.Audio;
using Cysharp.Threading.Tasks;
using UnityEngine;
using AudioType = Core.Services.Audio.AudioType;

namespace Runtime.GameStateMachine
{
    public class AudioSettingsBootstrapController : BaseController
    {
        private readonly IAudioService _audioService;
        private readonly ISettingProvider _staticSettingsService;
        private readonly UserDataService _userDataService;

        private CancellationTokenSource _cancellationTokenSource;

        public AudioSettingsBootstrapController(IAudioService audioService, ISettingProvider staticSettingsService, UserDataService userDataService)
        {
            _audioService = audioService;
            _staticSettingsService = staticSettingsService;
            _userDataService = userDataService;
        }

        public override UniTask Run(CancellationToken cancellationToken)
        {
            base.Run(cancellationToken);

            _cancellationTokenSource = new CancellationTokenSource();
            var linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, _cancellationTokenSource.Token);

            SetVolume();
            PlayMusic(linkedTokenSource.Token).Forget();
            return UniTask.CompletedTask;
        }

        public override UniTask Stop()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();

            return base.Stop();
        }

        private async UniTask PlayMusic(CancellationToken cancellationToken)
        {
            var audioSettings = _staticSettingsService.Get<AudioConfig>();
            var allMusicAudioData = audioSettings.Audio.FindAll(x => x.AudioType == AudioType.Music);
            var allMusicClips = new List<AudioClip>(allMusicAudioData.Count);

            foreach (var audioData in allMusicAudioData)
                allMusicClips.Add(audioData.Clip);

            var clipsCount = allMusicClips.Count;

            var clipIndex = 0;
            while (!cancellationToken.IsCancellationRequested)
            {
                var clipDuration = (int)allMusicClips[clipIndex].length * 1000 + 1000;
                _audioService.PlayMusic(allMusicClips[clipIndex]);
                await UniTask.Delay(clipDuration, cancellationToken: cancellationToken);
                clipIndex++;
                if (clipIndex >= clipsCount)
                    clipIndex = 0;
            }
        }

        private void SetVolume()
        {
            var isSoundVolume = _userDataService.GetUserData().SettingsData.IsSoundVolume;
            _audioService.SetVolume(AudioType.Sound, isSoundVolume ? 1 : 0);

            var isMusicVolume = _userDataService.GetUserData().SettingsData.IsMusicVolume;
            _audioService.SetVolume(AudioType.Music, isMusicVolume ? 1 : 0);
        }
    }
}