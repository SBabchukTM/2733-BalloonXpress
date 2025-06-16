using Core.UI;
using DG.Tweening;
using UnityEngine;

namespace Core.Services.ScreenOrientation
{
    public class ScreenOrientationAlertPopup : BasePopup
    {
        private const float PortretModeRotationAngleZ = 330f;
        private const float LandscapeModeRotationAngleZ = 240f;
        private const float AnimationInSpeed = 2f;
        private const float AnimationOutSpeed = 0.3f;

        [SerializeField] private RectTransform _phoneRecTransform;

        private Sequence _currentRotationSequence;

        void OnEnable()
        {
            _phoneRecTransform.rotation = Quaternion.Euler(0, 0, LandscapeModeRotationAngleZ);
            RotatingPhoneIconAnimation();
        }

        private void OnDisable()
        {
            _phoneRecTransform.DOKill();
            _currentRotationSequence.Kill();
        }

        private void RotatingPhoneIconAnimation()
        {
            _currentRotationSequence = DOTween.Sequence().SetUpdate(true);

            _currentRotationSequence.Append(_phoneRecTransform.DORotate(new Vector3(0, 0, PortretModeRotationAngleZ),
                AnimationInSpeed, RotateMode.FastBeyond360).SetEase(Ease.InOutSine)).SetUpdate(true);

            _currentRotationSequence.Append(_phoneRecTransform.DORotate(new Vector3(0, 0, LandscapeModeRotationAngleZ),
                AnimationOutSpeed, RotateMode.FastBeyond360).SetEase(Ease.InOutQuad)).SetUpdate(true);

            _currentRotationSequence.SetLoops(-1).SetUpdate(true);
        }
    }
}