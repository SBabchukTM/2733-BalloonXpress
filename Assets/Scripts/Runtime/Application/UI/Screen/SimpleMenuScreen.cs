using System;
using UnityEngine;

namespace Runtime.UI
{
    public class SimpleMenuScreen : UiScreen
    {
        [SerializeField] private SimpleButton _playButton;
        [SerializeField] private SimpleButton _settingsButton;
        [SerializeField] private SimpleButton _infoButtonPress;

        public event Action PlayButtonPressEvent;
        public event Action SettingsButtonPressEvent;
        public event Action InfoButtonPressEvent;

        private void OnDestroy()
        {
            _playButton.Button.onClick.RemoveAllListeners();
            _settingsButton.Button.onClick.RemoveAllListeners();
            _infoButtonPress.Button.onClick.RemoveAllListeners();
        }

        public void Initialize()
        {
            _playButton.Button.onClick.AddListener(OnPlayButtonPress);
            _settingsButton.Button.onClick.AddListener(OnSettingsButtonPress);
            _infoButtonPress.Button.onClick.AddListener(OnSettingsButtonPress);
        }

        private void OnPlayButtonPress()
        {
            PlayButtonPressEvent?.Invoke();
        }

        private void OnSettingsButtonPress()
        {
            SettingsButtonPressEvent?.Invoke();
        }

        private void OnInfoButtonPress()
        {
            InfoButtonPressEvent?.Invoke();
        }
    }
}