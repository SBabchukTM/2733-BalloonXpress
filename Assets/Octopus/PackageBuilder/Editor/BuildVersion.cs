using UnityEditor;
using UnityEngine;

namespace Octopus.PackageBuilder
{
    public class BuildVersion : EditorWindow
    {
        [MenuItem("Octopus/About Octopus", priority = 0)]
        private static void OpenWindow()
        {
            var window = GetWindow<BuildVersion>();

            window.titleContent = new GUIContent("About Octopus");

            window.minSize = new Vector2(250, 50);
            window.maxSize = new Vector2(250, 50); 
            
            window.maximized = true;

            window.Show();
        }
        
        private void OnGUI()
        {
            GUILayout.BeginVertical();
            GUILayout.FlexibleSpace();
            GUILayout.Label("Version: " + GetCurrentVersion(), GUILayout.Height(30));
            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();
        }
        
        private string GetCurrentVersion()
        {
            var versionManager = new PackageVersionManager();
            
            return versionManager.GetCurrentVersion();
        }
    }
}
