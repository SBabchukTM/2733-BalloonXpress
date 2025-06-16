using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Runtime.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    [DisallowMultipleComponent]
    public abstract class UiScreen : MonoBehaviour
    {
        [SerializeField] protected float _fadeDuration = 0.25f;
        [SerializeField] protected string _id;

        private CanvasGroup CanvasGroup;
        private UniTaskCompletionSource _showCompletionSource;
        private UniTaskCompletionSource _hideCompletionSource;
        private Tween _showFadeTween;
        private Tween _hideFadeTween;

        public string Id => _id;

        private void Awake()
        {
            CanvasGroup = GetComponent<CanvasGroup>();
        }

        public virtual async UniTask ShowAsync(CancellationToken cancellationToken = default)
        {
            _showCompletionSource = new UniTaskCompletionSource();

            CanvasGroup.interactable = true;
            CanvasGroup.blocksRaycasts = true;
            _showFadeTween = CanvasGroup.DOFade(1, _fadeDuration)
                .OnComplete(()=>
                {
                    _showCompletionSource?.TrySetResult();
                })
                .From(0)
                .SetLink(gameObject, LinkBehaviour.KillOnDestroy);
            
            cancellationToken.Register(() =>
            {
                _showFadeTween?.Kill();
                _showCompletionSource?.TrySetCanceled();
            });

            await _showCompletionSource.Task;
        }

        public virtual void ShowImmediately()
        {
            CanvasGroup.alpha = 1;
            CanvasGroup.interactable = true;
            CanvasGroup.blocksRaycasts = true;
        }

        public virtual async UniTask HideAsync(CancellationToken cancellationToken = default)
        {
            _hideCompletionSource = new UniTaskCompletionSource();
            CanvasGroup.interactable = false;

            _hideFadeTween = CanvasGroup.DOFade(0, _fadeDuration)
                .SetLink(gameObject, LinkBehaviour.KillOnDestroy).OnComplete(() =>
                {
                    _hideCompletionSource?.TrySetResult();
                    Destroy(gameObject);
                });

            cancellationToken.Register(() =>
            {
                _hideFadeTween?.Kill();
                _hideCompletionSource?.TrySetCanceled();
            });

            await _hideCompletionSource.Task;
        }

        public virtual void HideImmediately()
        {
            CanvasGroup.alpha = 0;
            CanvasGroup.interactable = false;
            CanvasGroup.blocksRaycasts = false;
        }

        public void DestroyScreen()
        {
            Destroy(gameObject);
        }
    }
}