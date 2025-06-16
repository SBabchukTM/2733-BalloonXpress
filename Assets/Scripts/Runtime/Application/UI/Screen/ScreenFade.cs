using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.UI
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Image))]
    public class ScreenFade : MonoBehaviour
    {
        [SerializeField] [Min(float.Epsilon)] private float _fadeDuration = 0.55f;

        private Image _image;
        private Tween _fadeOutTween;
        private Tween _fadeInTween;
        private Color _defaultColor;
        private UniTaskCompletionSource _fadeInCompletionSource;
        private UniTaskCompletionSource _fadeOutCompletionSource;

        private void Awake()
        {
            _image = GetComponent<Image>();
            _defaultColor = _image.color;
        }

        private void Start()
        {
            _image.enabled = true;
            _image.raycastTarget = false;
            _image.color = Color.clear;
        }

        public async UniTask FadeInAsync(Color? color = null, float? duration = null,
            CancellationToken cancellationToken = default)
        {
            _image.DOKill();
            _image.color = color ?? _defaultColor;
            _fadeInCompletionSource = new UniTaskCompletionSource();

            _fadeInTween = _image.DOFade(1, duration ?? _fadeDuration)
                .From(0)
                .SetEase(Ease.InCubic)
                .OnStart(() => _image.raycastTarget = true)
                .OnComplete(()=> _fadeInCompletionSource?.TrySetResult())
                .SetLink(gameObject, LinkBehaviour.KillOnDestroy);

            cancellationToken.Register(() =>
            {
                _fadeInTween?.Kill();
                _fadeInCompletionSource?.TrySetCanceled();
            });

            await _fadeInCompletionSource.Task;
        }

        public async UniTask FadeOutAsync(Color? color = null, float? duration = null,
            CancellationToken cancellationToken = default)
        {
            _image.DOKill();
            _image.color = color ?? _defaultColor;
            _fadeOutCompletionSource = new UniTaskCompletionSource();

            _fadeOutTween = _image.DOFade(0, duration ?? _fadeDuration)
                .From(1)
                .SetEase(Ease.InCubic)
                .OnStart(() => _image.raycastTarget = false)
                .OnComplete(()=> _fadeOutCompletionSource?.TrySetResult())
                .SetLink(gameObject, LinkBehaviour.KillOnDestroy);

            cancellationToken.Register(() =>
            {
                _fadeOutTween?.Kill();
                _fadeOutCompletionSource?.TrySetCanceled();
            });

            await _fadeOutCompletionSource.Task;
        }
    }
}