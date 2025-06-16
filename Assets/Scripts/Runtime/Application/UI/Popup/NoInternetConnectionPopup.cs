using System.Threading;
using Core.UI;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.UI
{
    public class NoInternetConnectionPopup : BasePopup
    {
        [SerializeField] private Button _okButton;

        private UniTaskCompletionSource _completionSource;

        public override async UniTask Show(BasePopupData data, CancellationToken cancellationToken = default)
        {
            _completionSource = new UniTaskCompletionSource();
            _okButton.onClick.AddListener(Hide);

            await _completionSource.Task;

            DestroyPopup();
        }

        public override void Hide()
        {
            _completionSource?.TrySetResult();
        }
    }
}