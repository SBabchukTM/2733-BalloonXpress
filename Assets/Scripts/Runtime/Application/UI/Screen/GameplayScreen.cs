using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.UI
{
    public class GameplayScreen : UiScreen
    {
        private const float FadeAnimTime = 0.5f;

        [SerializeField] private Button _backButton;
        [SerializeField] private TextMeshProUGUI _balanceText;
        [SerializeField] private TextMeshProUGUI _betText;
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private CanvasGroup _gameStartedNotifyCanvas;
        [SerializeField] private CanvasGroup _beforeGameStartedNotifyCanvas;

        [SerializeField] private CardsController _cardsController;

        public event Action OnBackPressed;

        public void Initialize()
        {
            _backButton.onClick.AddListener(() => OnBackPressed?.Invoke());
        }

        public async UniTaskVoid ChangeHelpMessage()
        {
            _beforeGameStartedNotifyCanvas.DOFade(0, FadeAnimTime);
            await UniTask.WaitForSeconds(FadeAnimTime);
            _gameStartedNotifyCanvas.DOFade(1, FadeAnimTime);
        }

        public void DisableBackButton() => _backButton.interactable = false;

        public void SetBalance(int balance) => _balanceText.text = balance.ToString();
        public void SetBet(int bet) => _betText.text = bet.ToString();
        public void SetLevel(int level) => _levelText.text = "Level " + (level + 1);
        public CardsController GetCardsController() => _cardsController;
    }
}