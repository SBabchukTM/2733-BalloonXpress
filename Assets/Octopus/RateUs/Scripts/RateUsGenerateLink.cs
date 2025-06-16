using UnityEngine;

namespace RateUs
{
    public class RateUsGenerateLink : MonoBehaviour
    {
        [SerializeField, Header("For Andriod")]
        private string _bundleId;
    
        [SerializeField, Header("For IOS")]
        private string _appID;

        private string key = "";
        private string value = "";
        
        private void Start()
        {
            GenerateInPlayerPrefs();
        }

        private void GenerateInPlayerPrefs()
        {
#if UNITY_ANDROID
            key = Consts.KeyAndroid;
            
            if(_bundleId == "")
            {
                Debug.Log("!!! Check _bundleId in Rate Us");
                return;
            }
            
            value = Consts.PrefixAndroid + _bundleId;
#elif UNITY_IOS
            key = Consts.KeyIOS;
            
            if(_appID == "")
            {
                Debug.Log("!!! Check _appID in Rate Us");
                return;
            }
            
            value = Consts.PrefixIOS + _appID;
#endif
            if(key == "")
            {
                Debug.Log("!!! Check Key in Rate Us");
                return;
            }
            
            
            if (PlayerPrefs.HasKey(key)) return;
            
            PlayerPrefs.SetString(key, value);
            PlayerPrefs.Save();
        }
        
        [ContextMenu("Delete Keys")]
        private void DeleteKeys()
        {
            PlayerPrefs.DeleteKey(Consts.KeyAndroid);
            
            PlayerPrefs.DeleteKey(Consts.KeyIOS);
        }
    }
}

