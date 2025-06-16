using Core;
using UnityEngine;

public class GAIDHelper : MonoBehaviour
{
    public static string GetGAID()
    {
        if (UnityEngine.Application.isEditor)
        {
            PrintMessage("Запуск в редакторі Unity, повертається фіксоване значення GAID = 16f3ddfa-0808-4bb8-88e6-a1362cd7bd1a");
            return "16f3ddfa-0808-4bb8-88e6-a1362cd7bd1a";
        }
        
        if (UnityEngine.Application.platform != RuntimePlatform.Android)
        {
            PrintMessage("GAID доступний тільки на Android!");
            return null;
        }
        
        try
        {
            using (var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            using (var currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
            using (var advertisingIdClient = new AndroidJavaClass("com.google.android.gms.ads.identifier.AdvertisingIdClient"))
            using (var adInfo = advertisingIdClient.CallStatic<AndroidJavaObject>("getAdvertisingIdInfo", currentActivity))
            {
                return adInfo.Call<string>("getId");
            }
        }
        catch (System.Exception ex)
        {
            PrintMessage("Помилка отримання GAID: " + ex.Message);
            return null;
        }
    }
    
    private static void PrintMessage(string message)
    {
        Debugger.Log($"@@@ Content ->: {message}", new Color(0.9f, 0.1f, 0.5f));
    }
}