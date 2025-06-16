using System.Threading;
using Core.UI;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.UI
{
    public class SimpleDecisionPopup : BasePopup
    {
        [SerializeField] private Button _confirmButton;
        [SerializeField] private Button _cancelButton;
        [SerializeField] private TextMeshProUGUI _message;

        public override UniTask Show(BasePopupData data, CancellationToken cancellationToken = default)
        {
            SimpleDecisionPopupData simpleDecisionPopupData = data as SimpleDecisionPopupData;

            _message.text = simpleDecisionPopupData.Message;
            _confirmButton.onClick.AddListener(() =>
            {
                simpleDecisionPopupData?.PressOkEvent?.Invoke();
                Hide();
            });

            _cancelButton.onClick.AddListener(Hide);

            return base.Show(data, cancellationToken);
        }
    }
}