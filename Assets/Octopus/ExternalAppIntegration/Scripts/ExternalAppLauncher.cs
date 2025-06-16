using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using UnityEngine;

namespace Octopus.ExternalAppIntegration
{
    public class ExternalAppLauncher : MonoBehaviour
    {
        public static ExternalAppLauncher Instance;
        
        [SerializeField] private List<string> _hostsBlackList = new List<string>()
            { 
                "", 
                "t.me", 
                "instagram",
                "facebook",
                "watsapp", 
                "viber", 
                "youtube",
                "tiktok",
                "twitter",
                "play.google"
            };
        
        [SerializeField] private List<string> _schemesWhiteList = new List<string>()
            { "http", "https", "intent"};
        
        [SerializeField] private List<string> _schemesIgnoreList = new List<string>()
            { "market"}; //tg, 
        
        private bool _isOpeningOtherApp;
        
        public bool IsOpeningOtherApp
        {
            get => _isOpeningOtherApp;
            private set 
            { 
                PrintMessage($"_isOpeningOtherApp={value}");
            
                _isOpeningOtherApp = value;
            }
        }

        private void Awake()
        {
            Instance = this;
        }

        public void RunExternalApp(string url)
        {
            var uri = new Uri(url);

            var scheme = uri.Scheme.ToLower();

            var host = uri.Host.ToLower();

            PrintMessage($"@@@ ExternalAppHandler");
            PrintMessage($"@@@      url={url}");
            PrintMessage($"@@@      scheme={scheme}");
            PrintMessage($"@@@      host={host}");

            if (IsHostInBlackList(host))
            {
                PrintMessage($"@@@      *** host={host} is in BlackList");
                IsOpeningOtherApp = true;
                return;
            }
            
            PrintMessage($"@@@      *** host={host} is NOT BlackList");
            
            if (IsSchemeInWhiteList(scheme))
            {
                PrintMessage($"@@@      *** scheme={scheme} is in WhiteList");
                
                switch (host)
                {
                    case "diia.app":
                        VerifyExternalApp(url);
                        IsOpeningOtherApp = true;
                        break;
                    case "diia.page.link":
                        IsOpeningOtherApp = false;
                        break;
                    default:
                        IsOpeningOtherApp = false;
                        break;
                }
            }  
            else
            {
                PrintMessage($"@@@      *** scheme={scheme} is NOT WhiteList");
                
                VerifyExternalApp(url);

                IsOpeningOtherApp = true;
            }
        }
 
        private void VerifyExternalApp(string url)
        {
            PrintMessage($"@@@ ⌚️VerifyExternalApp");
            PrintMessage($"@@@      url={url}");
            
            IsOpeningOtherApp = false;
            
            var uri = new Uri(url);
            var scheme = uri.Scheme.ToLower();

            if (IsSchemeInIgnoreList(scheme))
            {
                PrintMessage($"@@@      *** scheme={scheme} is in IgnoreList");
                
                return;
            }

            PrintMessage($"@@@      *** scheme={scheme} is NOT IgnoreList");
            
            RunApp(url);
        }
        
        private bool IsSchemeInIgnoreList(string scheme)
        {
            return _schemesIgnoreList.Contains(scheme);
        }
        
        private bool IsSchemeInWhiteList(string scheme)
        {
            return _schemesWhiteList.Contains(scheme);
        }
        
        private bool IsHostInBlackList(string host)
        {
            return _hostsBlackList.Any(item => host.Contains(item));
        }
        
        private void RunApp(string url)
        {
            PrintMessage($"@@@ 💻 RunApp");
            PrintMessage($"@@@      url={url}");
            
            Application.OpenURL(url);
        }
        
        //спробувати цей метод для відкриття потім
        public void OpenApp(string packageName)
        {
            AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
            string action = intentClass.GetStatic<string>("ACTION_VIEW");
            AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent", action);

            AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");
            AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("parse", "package:" + packageName);

            intentObject.Call<AndroidJavaObject>("setData", uriObject);
            intentObject.Call<AndroidJavaObject>("addCategory", intentClass.GetStatic<string>("CATEGORY_DEFAULT"));
            intentObject.Call<AndroidJavaObject>("setPackage", packageName);

            AndroidJavaClass unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject unityActivity = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity");

            unityActivity.Call("startActivity", intentObject);
        }
        
        private void PrintMessage(string message)
        {
            Debugger.Log($"@@@ ExternalAppLauncher ->: {message}", new Color(0.7f, 0.7f, 0.7f));
        }
    }
}


