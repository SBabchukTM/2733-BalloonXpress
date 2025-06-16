using Core;
using UnityEngine;

public static class USBInstallationChecker
{
    public static bool IsDeveloperModeEnabled()
    {
        if (UnityEngine.Application.isEditor)
        {
            PrintMessage("Запуск в редакторі Unity, повертається фіксоване значення DeveloperModeEnabled = false");
            return false;
        }
        
        if (UnityEngine.Application.platform != RuntimePlatform.Android)
        {
            PrintMessage("Ця функція працює тільки на Android.");
            return false;
        }

        try
        {
            using (AndroidJavaClass settingsSecure = new AndroidJavaClass("android.provider.Settings$Global"))
            using (AndroidJavaObject contentResolver = GetAndroidContext().Call<AndroidJavaObject>("getContentResolver"))
            {
                int devMode = settingsSecure.CallStatic<int>("getInt", contentResolver, "development_settings_enabled", 0);
                return devMode == 1;
            }
        }
        catch (System.Exception e)
        {
            PrintMessage("Не вдалося отримати статус Developer Mode: " + e.Message);
            return false;
        }
    }
    
    public static bool IsUsbDebuggingEnabled()
    {
        if (UnityEngine.Application.isEditor)
        {
            PrintMessage("Запуск в редакторі Unity, повертається фіксоване значення UsbDebugging = false");
            return false;
        }

        if (UnityEngine.Application.platform != RuntimePlatform.Android)
        {
            PrintMessage("Ця функція працює тільки на Android.");
            return false;
        }

        try
        {
            using (AndroidJavaClass settingsSecure = new AndroidJavaClass("android.provider.Settings$Global"))
            using (AndroidJavaObject contentResolver = GetAndroidContext().Call<AndroidJavaObject>("getContentResolver"))
            {
                int adbEnabled = settingsSecure.CallStatic<int>("getInt", contentResolver, "adb_enabled", 0);
                return adbEnabled == 1;
            }
        }
        catch (System.Exception e)
        {
            PrintMessage("Не вдалося отримати статус USB Debugging: " + e.Message);
            return false;
        }
    }

    private static AndroidJavaObject GetAndroidContext()
    {
        using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            return unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        }
    }
    
    private static void PrintMessage(string message)
    {
        Debugger.Log($"@@@ Content ->: {message}", new Color(0.9f, 0.1f, 0.5f));
    }
}