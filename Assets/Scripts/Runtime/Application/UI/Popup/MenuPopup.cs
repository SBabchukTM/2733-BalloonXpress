using Runtime.UI;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI
{
    public class MenuPopup : BasePopup
    {
        [SerializeField] private SimpleButton _accountButton;
        [SerializeField] private SimpleButton _settingsButton;
        [SerializeField] private SimpleButton _leaderboardButton;
        [SerializeField] private SimpleButton _rulesButton;
        [SerializeField] private SimpleButton _privacyPolicyButton;
        [SerializeField] private SimpleButton _closeButton;

        public event Action OnAccountPressed;
        public event Action OnSettingsPressed;
        public event Action OnLeaderboardPressed;
        public event Action OnRulesPressed;
        public event Action OnClosePressed;
        public event Action OnPrivacyPolicyPressed;

        public override UniTask Show(BasePopupData data, CancellationToken cancellationToken = default)
        {
            SubscribeToEvents();
            return base.Show(data, cancellationToken);
        }

        private void OnDestroy()
        {
            _accountButton.Button.onClick.RemoveAllListeners();
            _settingsButton.Button.onClick.RemoveAllListeners();
            _leaderboardButton.Button.onClick.RemoveAllListeners();
            _rulesButton.Button.onClick.RemoveAllListeners();
            _closeButton.Button.onClick.RemoveAllListeners();
            _privacyPolicyButton.Button.onClick.RemoveAllListeners();
        }

        private void SubscribeToEvents()
        {
            _accountButton.Button.onClick.AddListener(() => OnAccountPressed?.Invoke());
            _settingsButton.Button.onClick.AddListener(() => OnSettingsPressed?.Invoke());
            _leaderboardButton.Button.onClick.AddListener(() => OnLeaderboardPressed?.Invoke());
            _rulesButton.Button.onClick.AddListener(() => OnRulesPressed?.Invoke());
            _closeButton.Button.onClick.AddListener(() => OnClosePressed?.Invoke());
            _privacyPolicyButton.Button.onClick.AddListener(() => OnPrivacyPolicyPressed?.Invoke());
        }
    }
}