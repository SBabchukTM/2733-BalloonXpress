using System.Threading;
using Core.UI;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Runtime.UI
{
    public class MessagePopup : BasePopup
    {
        [SerializeField] private SimpleButton _okButton;
        [SerializeField] private TextMeshProUGUI _message;

        public override UniTask Show(BasePopupData data, CancellationToken cancellationToken = default)
        {
            MessagePopupData messagePopupData = data as MessagePopupData;

            _message.text = messagePopupData.Message;

            if (messagePopupData.IsShowButton)
            {
                _okButton.gameObject.SetActive(true);
                _okButton.Button.onClick.AddListener(Hide);
            }
            else
            {
                _okButton.gameObject.SetActive(false);
            }

            return base.Show(data, cancellationToken);
        }
    }
}