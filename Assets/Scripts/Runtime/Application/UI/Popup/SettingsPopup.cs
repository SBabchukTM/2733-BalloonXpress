using System;
using System.Threading;
using Runtime.Services.Audio;
using Runtime.Services.UserData;
using Core.UI;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.UI
{
    public class SettingsPopup : BasePopup
    {
        [SerializeField] private Button _closeButton;
        [SerializeField] private Toggle _soundVolumeToggle;
        [SerializeField] private Toggle _musicVolumeToggle;

        public event Action<bool> SoundVolumeChangeEvent;
        public event Action<bool> MusicVolumeChangeEvent;

        public override UniTask Show(BasePopupData data, CancellationToken cancellationToken = default)
        {
            SettingsPopupData settingsPopupData = data as SettingsPopupData;

            var isSoundVolume = settingsPopupData.IsSoundVolume;
            _soundVolumeToggle.onValueChanged.Invoke(isSoundVolume);
            _soundVolumeToggle.isOn = isSoundVolume;
            
            var isMusicVolume = settingsPopupData.IsMusicVolume;
            _musicVolumeToggle.onValueChanged.Invoke(isMusicVolume);
            _musicVolumeToggle.isOn = isMusicVolume;

            _closeButton.onClick.AddListener(DestroyPopup);

            _soundVolumeToggle.onValueChanged.AddListener(OnSoundVolumeToggleValueChanged);
            _musicVolumeToggle.onValueChanged.AddListener(OnMusicVolumeToggleValueChanged);

            AudioService.PlaySound(ConstAudio.OpenPopupSound);

            return base.Show(data, cancellationToken);
        }

        public override void DestroyPopup()
        {
            AudioService.PlaySound(ConstAudio.PressButtonSound);
            Destroy(gameObject);
        }

        private void OnSoundVolumeToggleValueChanged(bool value)
        {
            AudioService.PlaySound(ConstAudio.PressButtonSound);

            SoundVolumeChangeEvent?.Invoke(value);
        }

        private void OnMusicVolumeToggleValueChanged(bool value)
        {
            AudioService.PlaySound(ConstAudio.PressButtonSound);

            MusicVolumeChangeEvent?.Invoke(value);
        }
    }
}