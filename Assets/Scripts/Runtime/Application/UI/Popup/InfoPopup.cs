using System.Threading;
using Core.UI;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.UI
{
    public class InfoPopup : BasePopup
    {
        [SerializeField] private Button _okButton;
        [SerializeField] private TextMeshProUGUI _label;
        [SerializeField] private TextMeshProUGUI _message;
        [SerializeField] private Image _image;

        public override UniTask Show(BasePopupData data, CancellationToken cancellationToken = default)
        {
            InfoPopupData infoPopupData = data as InfoPopupData;

            _message.text = infoPopupData.Message;
            _label.text = infoPopupData.Label;
            if (infoPopupData.Sprite != null)
            {
                _image.sprite = infoPopupData.Sprite;
                _image.preserveAspect = true;
                _image.gameObject.SetActive(true);
            }

            _okButton.onClick.AddListener(Hide);

            return base.Show(data, cancellationToken);
        }
    }
}