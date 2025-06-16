using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.UI
{
    public class MenuScreen : UiScreen
    {
        [SerializeField] private SimpleButton _menuButton;
        [SerializeField] private SimpleButton _playButton;
        [SerializeField] private TextMeshProUGUI _balanceText;

        public event Action OnMenuPressed;
        public event Action OnPlayPressed;

        private void OnDestroy()
        {
            _menuButton.Button.onClick.RemoveAllListeners();
            _playButton.Button.onClick.RemoveAllListeners();
        }

        public void Initialize(int balance)
        {
            _menuButton.Button.onClick.AddListener(() => OnMenuPressed?.Invoke());
            _playButton.Button.onClick.AddListener(() => OnPlayPressed?.Invoke());
            _balanceText.text = balance.ToString();
        }
    }
}