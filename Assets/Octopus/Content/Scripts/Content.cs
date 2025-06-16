using Core;
using Octopus.Client;
using Octopus.VerifyInternet;
using Octopus.Preloader;
using Octopus.SceneLoaderCore.Helpers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Octopus.Content
{
   public class Content : MonoBehaviour
   {
      private void Start()
      { 
         PrintMessage("Started");
         
         Loading.Instance.Visible(true);
         
         CheckInternetConnection(ConnectivityManager.Instance.IsConnected);
      }

      private void CheckInternetConnection(bool? isConnection)
      {
         if (isConnection == null)
         {
            ConnectivityManager.Instance.OnChangedInternetConnection.AddListener(CheckInternetConnection);
            
            return;
         }
         else
         {
            ConnectivityManager.Instance.OnChangedInternetConnection.RemoveListener(CheckInternetConnection);
         }
         
         if (isConnection == true)
         {
            PrintMessage($"IsOnlyWhiteRunApp = {PlayerPrefs.GetInt(Constants.IsOnlyWhiteRunApp, 0) != 0}");
            //перевіряємо, якщо вже запускали апку і знаєм, що вона не біла назавжди
            if(PlayerPrefs.GetInt(Constants.IsOnlyWhiteRunApp, 0) == 0)
            {
               Client.Client.Instance.Initialize();
            }
            else //назавжди біла буде
            {
               ShowWhiteApp();
            }
         }
         else if (isConnection == false)
         {
            PrintMessage("No Internet");
            
            if (GameSettings.HasKey(Constants.IsFirstRunApp))
            {
               //TODO Добавив перевірку чи варто запускати саме білу апку
               if(PlayerPrefs.GetInt(Constants.IsOnlyWhiteRunApp, 0) == 1)
               {
                  ShowWhiteApp();
               }
               else
               {
                  ConnectivityManager.Instance.OnChangedInternetConnection.AddListener(CheckInternetConnection);
               }
            }
            else
            {
               ConnectivityManager.Instance.OnChangedInternetConnection.AddListener(CheckInternetConnection);

               //ShowWhiteApp(); //закоментував, щоб доки не появиться інет ми висіли на прелодері
            }
         }
      }
      
      private void ShowWhiteApp()
      {
         if (SceneLoader.Instance)
         {
            SceneLoader.Instance.SwitchToScene(SceneLoader.Instance.mainScene);
         }
         else
         {
            SceneManager.LoadScene("MainMenu");
         }
      }
      
      private void PrintMessage(string message)
      {
         Debugger.Log($"@@@ Content ->: {message}", new Color(0.8f, 0.5f, 0));
      }
   }
}
