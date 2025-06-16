using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardView : MonoBehaviour
{
    [SerializeField] private Image _backgroundImage;
    [SerializeField] private GameObject _prizeGameObject;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private float _rotationSpeed = 500f;

    private int _cost;
    private bool _isPaused = false;
    private CancellationTokenSource _cancellationTokenSource;

    public Button Button;

    public event Action<int> OnCardButtonEvent;

    private void OnEnable()
    {
        Button.onClick.AddListener(OnCardButtonPressed);
        _cancellationTokenSource = new CancellationTokenSource();
    }

    private void OnDisable()
    {
        Button.onClick.RemoveListener(OnCardButtonPressed);
        _cancellationTokenSource?.Cancel();
    }

    public void SetCardConfig(Sprite background, int cost)
    {
        _cost = cost;
        _text.text = _cost.ToString();
        _backgroundImage.sprite = background;
    }

    public void PauseFlip()
    {
        _isPaused = true;
    }

    public void ResumeFlip()
    {
        _isPaused = false;
    }

    public async UniTask FlipToShow(float speed = -1)
    {
        try
        {
            float rotationDuration = (speed > 0 ? speed : _rotationSpeed);

            await RotateTo(90, rotationDuration);
            _prizeGameObject.gameObject.SetActive(true);

            await RotateTo(180, rotationDuration);
        }
        catch (OperationCanceledException)
        {
            Debug.Log("FlipToShow was canceled.");
        }
    }

    public async UniTask FlipToHide(float speed = -1)
    {
        try
        {
            float rotationDuration = (speed > 0 ? speed : _rotationSpeed);

            await RotateTo(90, rotationDuration);
            _prizeGameObject.gameObject.SetActive(false);

            await RotateTo(0, rotationDuration);
        }
        catch (OperationCanceledException)
        {
            Debug.Log("FlipToHide was canceled.");
        }
    }

    private async UniTask RotateTo(float targetAngle, float rotationSpeed)
    {
        await UniTask.Yield(_cancellationTokenSource.Token);

        float currentAngle = transform.eulerAngles.y;
        float duration = Mathf.Abs(targetAngle - currentAngle) / rotationSpeed;
        float startTime = Time.realtimeSinceStartup;
        float endTime = startTime + duration;
        float initialAngle = currentAngle;

        while (Time.realtimeSinceStartup < endTime)
        {
            _cancellationTokenSource.Token.ThrowIfCancellationRequested();

            while (Time.timeScale == 0f || _isPaused)
            {
                await UniTask.Yield(_cancellationTokenSource.Token);

                float pausedTime = Time.realtimeSinceStartup;
                endTime = pausedTime + (duration - (pausedTime - startTime));
            }

            float t = (Time.realtimeSinceStartup - startTime) / duration;
            currentAngle = Mathf.Lerp(initialAngle, targetAngle, t);
            transform.eulerAngles = new Vector3(0, currentAngle, 0);

            await UniTask.Yield(_cancellationTokenSource.Token);
        }

        transform.eulerAngles = new Vector3(0, targetAngle, 0);
    }


    private void OnCardButtonPressed()
    {
        OnCardButtonEvent?.Invoke(_cost);
    }

    public void CancelCurrentProcesses()
    {
        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource = new CancellationTokenSource();
    }

    public CancellationToken GetCancellationToken()
    {
        if (_cancellationTokenSource == null)
        {
            _cancellationTokenSource = new CancellationTokenSource();
        }
        return _cancellationTokenSource.Token;
    }
}
