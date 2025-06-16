using UnityEditor;
using UnityEngine;

public static class SettingsEditorMenu
{
    [MenuItem("Tools/Open Settings")]
    public static void OpenSettings()
    {
        var settings = Resources.Load<Settings>("Settings");

        if (settings != null)
        {
            Selection.activeObject = settings;
            EditorGUIUtility.PingObject(settings);
        }
        else
        {
            Debug.LogError("❌Settings no found in Resources!");
        }
    }
}