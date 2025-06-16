using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using System.Linq;
using System;
using Random = UnityEngine.Random;
using System.Threading;
using UnityEngine.UI;

public class CardsController : MonoBehaviour
{
    [SerializeField] private RectTransform _parentArea;
    [SerializeField] private GridLayoutGroup _gridLayoutGroup;
    // [SerializeField] private CardConfig _winCardConfig;
    [SerializeField] private GameObject _cardPrefab;
    [SerializeField] private List<CardView> _cards;
    [SerializeField] private float _moveDuration = 2f;
    [SerializeField] private float _pauseDuration = 1f;
    [SerializeField] private float _flyTime = 5f;

    private bool _isPaused = false;
    private List<Vector2> _initialPositions;
    private CancellationTokenSource _cancellationTokenSource;

    public event Action<int> OnGameOverEvent;

    public void Initialization(LevelConfig levelConfig)
    {

        _initialPositions = new List<Vector2>();
        _moveDuration = levelConfig.MoveDuration;
        _flyTime = levelConfig.LevelTime;
        _cards = GetComponentsInChildren<CardView>().ToList();

        for (int i = 0; i < levelConfig.Prizes.Count; i++)
        {
            SpawnCard(levelConfig.CardSprite, levelConfig.Prizes[i]);
        }
    }

    public void SpawnCard(Sprite sprite, int cost)
    {
        var cardObject = Instantiate(_cardPrefab, _parentArea);
        var card = cardObject.GetComponent<CardView>();
        _initialPositions.Add(card.GetComponent<RectTransform>().anchoredPosition);
        card.Button.interactable = false;
        card.OnCardButtonEvent += WinCheckAsync;
        card.SetCardConfig(sprite, cost);
        _cards.Add(card);
    }

    public void StartGame()
    {
        _cancellationTokenSource = new CancellationTokenSource();
        // _gridLayoutGroup.enabled = false;

        LayoutRebuilder.ForceRebuildLayoutImmediate(_parentArea);
        SaveInitialCardPositions();

        _gridLayoutGroup.enabled = false;

        RestoreCardPositions();
        StartGameAsync(_cancellationTokenSource.Token).Forget();
    }

    private async UniTask StartGameAsync(CancellationToken cancellationToken)
    {
        // await FlipToShowAllCards();

        await UniTask.Delay(1000, cancellationToken: cancellationToken);

        await FlipToHideAllCards();

        await FlyCards(_flyTime, cancellationToken);

        await UniTask.Delay(1000, cancellationToken: cancellationToken);

        foreach (var cardView in _cards)
        {
            cardView.Button.interactable = true;
        }
    }

/*    public async UniTask PauseGame()
    {
        _isPaused = true;
        foreach (var cardView in _cards)
        {
            cardView.PauseFlip();
        }
        await UniTask.CompletedTask;
    }

    public async UniTask ResumeGame()
    {
        _isPaused = false;
        foreach (var cardView in _cards)
        {
            cardView.ResumeFlip();
        }
        await UniTask.CompletedTask;
    }*/

    public async UniTask CancelGame()
    {
        _cancellationTokenSource?.Cancel();
        await UniTask.CompletedTask;
    }

    private async void WinCheckAsync(int winCost)
    {
        foreach (var card in _cards)
        {
            card.Button.interactable = false;
        }

        await FlipToShowAllCards();

        OnGameOverEvent?.Invoke(winCost);
    }

    private async UniTask FlipToHideAllCards()
    {
        foreach (var cardView in _cards)
        {
            await cardView.FlipToHide();
        }
    }

    private async UniTask FlipToShowAllCards()
    {
        foreach (var cardView in _cards)
        {
            await cardView.FlipToShow();
        }
    }

    private async UniTask FlyCards(float duration, CancellationToken cancellationToken)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            cancellationToken.ThrowIfCancellationRequested();

            foreach (var card in _cards)
            {
                await UniTask.Yield(cancellationToken);
                Vector2 randomPosition = GetRandomPositionWithinParent();
                card.GetComponent<RectTransform>().DOAnchorPos(randomPosition, _moveDuration).SetEase(Ease.Linear);
            }

            elapsed += _moveDuration;

            try
            {
                await UniTask.Delay((int)(_moveDuration * 1000), cancellationToken: cancellationToken);
            }
            catch (OperationCanceledException)
            {
                Debug.Log("FlyCards operation was canceled.");
                return;
            }
        }

        ShuffleAndReturn();
    }

    private void ShuffleAndReturn()
    {
        List<Vector2> shuffledPositions = new List<Vector2>(_initialPositions);
        for (int i = 0; i < shuffledPositions.Count; i++)
        {
            int randomIndex = Random.Range(0, shuffledPositions.Count);
            (shuffledPositions[i], shuffledPositions[randomIndex]) = (shuffledPositions[randomIndex], shuffledPositions[i]);
        }

        for (int i = 0; i < _cards.Count; i++)
        {
            _cards[i].GetComponent<RectTransform>().DOAnchorPos(shuffledPositions[i], _pauseDuration).SetEase(Ease.OutQuad);
        }
    }

    private Vector2 GetRandomPositionWithinParent()
    {
        try
        {
            Rect parentRect = _parentArea.rect;

            float randomX = Random.Range(parentRect.xMin, parentRect.xMax);
            float randomY = Random.Range(parentRect.yMin, parentRect.yMax);

            return new Vector2(randomX, randomY);
        }
        catch (Exception)
        {
            Debug.Log("GetRandomPositionWithinParent was canceled.");
            return Vector2.zero;
        }
    }

    private void SaveInitialCardPositions()
    {
        _initialPositions.Clear();
        foreach (var card in _cards)
        {
            var rectTransform = card.GetComponent<RectTransform>();

            _initialPositions.Add(rectTransform.localPosition);
        }
    }

    private void RestoreCardPositions()
    {
        for (int i = 0; i < _cards.Count; i++)
        {
            var rectTransform = _cards[i].GetComponent<RectTransform>();

            rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            rectTransform.anchorMax = new Vector2(0.5f, 0.5f);

            rectTransform.localPosition = _initialPositions[i];
        }
    }
}