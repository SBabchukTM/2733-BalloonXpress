using System;
using System.Collections.Generic;
using Core;
using UnityEngine;
using UnityEngine.Networking;

namespace Octopus.Client
{
    [System.Serializable]
    public class StartRequest : Request
    {
        public override void GenerateBody()
        {
            base.GenerateBody();
            
            body += ",\"" + Constants.ReqType + "\":\"" + ReqType.receive.ToString() + "\"";
           
            //body += ",\"" + "startApfUID" + "\":\"" + AppsFlyer.getAppsFlyerId() + "\"";
            //TODO Пробую зберегти AppsFlyer.getAppsFlyerId()
            //body += ",\"" + "startApfUID" + "\":\"" + PlayerPrefs.GetString(Constants.AppsFlyerId) + "\"";
            Debug.Log(":" + PlayerPrefs.GetString(Constants.StartApfConvData, "null")); 
            //body += ",\"" + Constants.StartApfConvData + "\":\"" + PlayerPrefs.GetString(Constants.StartApfConvData, "null")+ "\"";
            //startInstallRef 
        }

        public override void ProcessResponse(UnityWebRequest response)
        {
            Debugger.Log($"@@@ StartRequest-> ProcessResponse");
            
            GameSettings.SetFirstRunApp();
            
            base.ProcessResponse(response);
        }
        
        protected override void ParseResponse(Dictionary<string, string> dictionary)
        {
            if (dictionary.ContainsKey("receiveUrl"))
            {
                CheckBinom(dictionary[Constants.ReceiveUrl]);
            }
           
            base.ParseResponse(dictionary);
        }
        
        public void CheckBinom(string newBinom)
        {
            var log = "Біном перший раз прийшов";
            
            if (GameSettings.HasKey(Constants.Binom))
                log = "Бінома не змінився";

            var currentBinom = GameSettings.GetValue(Constants.Binom, "");
            
            var isChangedBinom = newBinom != currentBinom;
            
            if (isChangedBinom) 
                log = "Біном змінився";
            
            Debugger.Log($"@@@ StartRequest -> CheckBinom: {log}");
            
            Debugger.Log($"@@@ SetParam: currentBinom{currentBinom}, newBinom: {newBinom}");
            
            if (isChangedBinom)
            {
                GameSettings.SetValue(Constants.Binom, newBinom);
                
                GameSettings.DeleteKey(Constants.StartUrl);
                
                GameSettings.DeleteKey(Constants.LastUrl);
            }
        }
    }
}
