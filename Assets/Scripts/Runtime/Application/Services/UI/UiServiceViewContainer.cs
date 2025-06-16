using System.Collections.Generic;
using Core.UI;
using UnityEngine;

namespace Runtime.UI
{
    public sealed class UiServiceViewContainer : MonoBehaviour
    {
        [SerializeField] private ScreenFade _screenFade;
        [SerializeField] private List<UiScreen> _screensPrefab;
        [SerializeField] private List<BasePopup> _popupsPrefab;
        [SerializeField] private Transform _screenParent;

        public ScreenFade ScreenFade => _screenFade;
        public List<UiScreen> ScreensPrefab => _screensPrefab;
        public List<BasePopup> PopupsPrefab => _popupsPrefab;
        public Transform ScreenParent => _screenParent;
    }
}