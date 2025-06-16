using Runtime.Services.UserData;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.UI
{
    public class SettingsScreen : UiScreen
    {
        [SerializeField] private Button _backButton;
        [SerializeField] private Toggle _soundToggle;
        [SerializeField] private Toggle _musicToggle;
        [SerializeField] private Toggle _vibrationToggle;

        public event Action OnBackPressed;

        public event Action<bool> OnSoundChanged;
        public event Action<bool> OnMusicChanged;
        public event Action<bool> OnVibrationChanged;

        private void OnDestroy()
        {
            _backButton.onClick.RemoveAllListeners();
            _soundToggle.onValueChanged.RemoveAllListeners();
            _musicToggle.onValueChanged.RemoveAllListeners();
            _vibrationToggle.onValueChanged.RemoveAllListeners();
        }

        public void Initialize(SettingsData data)
        {
            SetData(data);
            SubscribeToEvents();
        }

        private void SubscribeToEvents()
        {
            _backButton.onClick.AddListener(() => OnBackPressed?.Invoke());
            _soundToggle.onValueChanged.AddListener((value) => OnSoundChanged?.Invoke(value));
            _musicToggle.onValueChanged.AddListener((value) => OnMusicChanged?.Invoke(value));
            _vibrationToggle.onValueChanged.AddListener((value) => OnVibrationChanged?.Invoke(value));    
        }

        private void SetData(SettingsData data)
        {
            _soundToggle.isOn = data.IsSoundVolume;
            _musicToggle.isOn = data.IsMusicVolume;
            _vibrationToggle.isOn = data.IsVibration; 
        }
    }
}