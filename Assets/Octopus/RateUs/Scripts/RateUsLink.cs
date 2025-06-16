using UnityEngine;
using UnityEngine.UI;

namespace RateUs
{
    [RequireComponent(typeof(Button))]
    public class RateUsLink : MonoBehaviour
    {
        private Button _button;

        private string key = "";

        private void Start()
        {
            _button = GetComponent<Button>();
            
            _button.onClick.AddListener(OpenLink);
        }

        private void OpenLink()
        {
#if UNITY_ANDROID
            key = Consts.KeyAndroid;
#elif UNITY_IOS
            key = Consts.KeyIOS;
#endif
            if(key == "") return;
            
            UnityEngine.Application.OpenURL(PlayerPrefs.GetString(key));
        }
    }
}
