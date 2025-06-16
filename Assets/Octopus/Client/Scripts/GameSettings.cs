using System;
using Core;
using UnityEngine;

namespace Octopus.Client
{
    public static class GameSettings
    {
        public static void Init()
        {
            SetValue(Constants.ApiVersion, Settings.ApiVersion().ToString());

            SetValue(Constants.UniqueAppID, Guid.NewGuid().ToString());

            SetValue(Constants.PackageName, UnityEngine.Application.identifier);

            SetValue(Constants.CodeVersion, Settings.CodeVersion());

            SetValue(Constants.AppVersion, UnityEngine.Application.version);
            
            //SetValue(Constants.StartUrl, "");
            
            SetValue(Constants.GAID, GAIDHelper.GetGAID());
        }
        
        public static bool HasKey(string key)
        {
            return PlayerPrefs.HasKey(key);
        }
      
        public static void SetValue(string key, string value)
        {
            PlayerPrefs.SetString(key, value);
            PlayerPrefs.Save();
        }

        public static string GetValue(string key, string defaultValue)
        {
            return PlayerPrefs.GetString(key, defaultValue);
        }
        
        public static string GetValue(string key)
        {
            return PlayerPrefs.GetString(key);
        }
        
        public static void DeleteKey(string key)
        {
            PlayerPrefs.DeleteKey(key);
        }
        
        public static void SetFirstRunApp()
        {
            PlayerPrefs.SetInt(Constants.IsFirstRunApp, 1);
            PlayerPrefs.Save();
        }
        
        public static void SetFirstWebView()
        {
            PlayerPrefs.SetInt(Constants.IsFirstRunWebView, 1);
            PlayerPrefs.Save();
        }
        
        public static void CheckStartUrl(string url)
        {
            if(HasKey(Constants.StartUrl)) return;
            
            Debugger.Log($"@@@ GameSettings -> CheckStartUrl: ReceiveUrl: {GetValue(Constants.StartUrl)},  newUrl{url}");
            
            SetValue(Constants.StartUrl, url);
        }
    }
}
