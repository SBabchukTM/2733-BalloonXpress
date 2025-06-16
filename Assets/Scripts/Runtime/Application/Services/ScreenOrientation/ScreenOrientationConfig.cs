using UnityEngine;

namespace Core.Services.ScreenOrientation
{
    [CreateAssetMenu(fileName = "ScreenOrientationConfig", menuName = "Config/ScreenOrientationConfig")]
    public sealed class ScreenOrientationConfig : BaseSettings
    {
        public ScreenOrientationTypes ScreenOrientationTypes;
        public bool EnableScreenOrientationPopup;
    }
}