using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using System.IO;
using System.Linq;

public class SceneSwitcherWindow : EditorWindow
{
    [MenuItem("Tools/Scene Switcher")]
    public static void ShowWindow()
    {
        GetWindow<SceneSwitcherWindow>("Scene Switcher");
    }

    private void OnGUI()
    {
        GUILayout.Label("Scenes in Build", EditorStyles.boldLabel);

        foreach (var scene in EditorBuildSettings.scenes.Select(s => Path.GetFileNameWithoutExtension(s.path)))
        {
            if (GUILayout.Button(scene))
            {
                OpenSceneInEditor(scene);
            }
        }
    }

    private void OpenSceneInEditor(string sceneName)
    {
        string scenePath = EditorBuildSettings.scenes
            .FirstOrDefault(s => Path.GetFileNameWithoutExtension(s.path) == sceneName)?.path;

        if (!string.IsNullOrEmpty(scenePath))
        {
            EditorSceneManager.OpenScene(scenePath);
        }
        else
        {
            Debug.LogError($"Scene '{sceneName}' не знайдено в Build Settings!");
        }
    }
}