using System;
using System.Collections.Generic;
using Core;
using Newtonsoft.Json;
using Octopus.SceneLoaderCore.Helpers;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

namespace Octopus.Client
{
    [System.Serializable]
    public class Request
    {
        protected string body;
        
        public string url;
        
        protected Action OnResponseParsed;

        public virtual void GenerateBody()
        {
            body = "\"" + Constants.ApiVersion + "\":" + int.Parse(GameSettings.GetValue(Constants.ApiVersion)) +
                   ",\"" + Constants.UniqueAppID + "\":\"" + GameSettings.GetValue(Constants.UniqueAppID) + "\"" +
                   ",\"" + Constants.PackageName + "\":\"" + GameSettings.GetValue(Constants.PackageName) + "\"" +
                   ",\"" + Constants.CodeVersion + "\":\"" + GameSettings.GetValue(Constants.CodeVersion) + "\"" +
                   ",\"" + Constants.AppVersion + "\":\"" + GameSettings.GetValue(Constants.AppVersion) + "\"";
        }

        public virtual void GenerateURL()
        {
            
        }

        public string Json()
        {
            return "{"+ body + "}";
        }
        
        public virtual void Respone(UnityWebRequest response, Action finished)
        {
            OnResponseParsed = finished;
            
            if (response.result == UnityWebRequest.Result.Success)
            {
                ProcessResponse(response);
            }

            else if (response.isHttpError || response.result == UnityWebRequest.Result.ConnectionError || response.result == UnityWebRequest.Result.ProtocolError)
            {
                // Обробляємо HTTP помилку (наприклад, клієнтська або серверна помилка)
                // В цих випадках ми можемо вважати це помилкою мережі або серверною помилкою.
                //TODO Логіка для того, щоб при падінні сервера показати збережене посилання раніше
                PrintMessage($"Обробляємо HTTP помилку");
                SwitchToScene();
            }
            
            else
            {
                PrintMessage($"Error While Sending: {response.error}");
                //TODO Переніс в ShowWhiteApp
                //PlayerPrefs.SetInt(Constants.IsOnlyWhiteRunApp, 1);
                //PlayerPrefs.Save();

                ShowWhiteApp();
            }
        }
        
        public virtual void ProcessResponse(UnityWebRequest response)
        {
            try
            {
                var dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(XBase64.Decode(response.downloadHandler.text));

                ParseResponse(dictionary);
            }
            catch (Exception e)
            {
                PrintMessage("------------Bad Json----------------------");
                PrintMessage($"Json: {Json()}");
                PrintMessage($"response.downloadHandler.text: {response.downloadHandler.text}");
                PrintMessage("Error While Responsing: " + response.error);
                PrintMessage("------------------------------------------");

                //TODO Фікс відправки токена, якщо немає стріма і ми попали на білу частину
                //TODO не впевнений що поможе, або ще щось поламає
                SwitchToScene(); // - з меджека, якщо поганий джейсон то відкривали білу частину
                //ResponseHanding();
            }
        }

        protected virtual void ParseResponse(Dictionary<string, string> dictionary)
        {
            foreach (var (key, value) in dictionary)
            {
                PlayerPrefs.SetString(key, value);

                PrintMessage($"key: {value}");
            }
            
            PlayerPrefs.Save();
            
            ResponseParsed();
        }
        
        protected virtual void ResponseParsed()
        {
            OnResponseParsed?.Invoke();
        }
        
        private void SwitchToScene()
        {
            PrintMessage("!!! Client -> SwitchToScene");
            
            if(CheckReceiveUrlIsNullOrEmpty())
            {
                ShowWhiteApp();
            }
            else
            {
                ResponseParsed();
            }
        }

        private void ShowWhiteApp()
        {
            PrintMessage($"@@@ Request -> ShowWhiteApp");
            
            PlayerPrefs.SetInt(Constants.IsOnlyWhiteRunApp, 1);
            PlayerPrefs.Save();
            
            if (SceneLoader.Instance)
                SceneLoader.Instance.SwitchToScene(SceneLoader.Instance.mainScene);
            else
                SceneManager.LoadScene("MainMenu");
        }
        
        private bool CheckReceiveUrlIsNullOrEmpty()
        {
            var receiveUrl = GameSettings.GetValue(Constants.ReceiveUrl, "");

            return String.IsNullOrEmpty(receiveUrl);
        }
        
        private void PrintMessage(string message)
        {
            Debugger.Log($"@@@ Request ->: {message}", new Color(0.1f, 0.5f, 0.3f));
        }
    }
}
