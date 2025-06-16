using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Octopus.PackageBuilder
{
    public class PackageBuilder : EditorWindow
    {
        private PackageVersionManager.VersionType versionType = PackageVersionManager.VersionType.Patch;

        private List<string> assetPaths = new List<string>
        {
            "Assets/Octopus",
            "Assets/Thirdparty"
        };

        private Vector2 scrollPosition;

        [MenuItem("Octopus/Build Package", priority = 1)]
        private static void OpenWindow()
        {
            var window = GetWindow<PackageBuilder>();

            window.titleContent = new GUIContent("Package Builder");
            window.minSize = new Vector2(300, 200);

            window.Show();
        }

        private void OnGUI()
        {
            GUILayout.Label("Additional Folders", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            for (var i = 0; i < assetPaths.Count; i++)
            {
                GUILayout.BeginHorizontal();
                assetPaths[i] = EditorGUILayout.TextField("Folder " + (i + 1), assetPaths[i]);
                if (GUILayout.Button("-", GUILayout.Width(20)))
                {
                    assetPaths.RemoveAt(i);
                    i--;
                }
                GUILayout.EndHorizontal();
            }
            EditorGUILayout.EndScrollView();

            EditorGUILayout.Space(10f);

            GUILayout.Label("Actions", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Add Folder", GUILayout.Width(120)))
            {
                assetPaths.Add("");
            }
            GUILayout.EndHorizontal();

            EditorGUILayout.Space(10f);
            
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Build Package", GUILayout.Height(60), GUILayout.Width(200)))
            {
                BuildPackage();
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        private void BuildPackage()
        {
            var versionManager = new PackageVersionManager();
            
            var newVersion = versionManager.IncreaseVersion(versionType);
            
            versionManager.UpdateVersion(newVersion);
            
            var packagePath = $"Assets/Octopus_v{newVersion}.unitypackage";

            AssetDatabase.ExportPackage(assetPaths.ToArray(), packagePath, ExportPackageOptions.Recurse);
        }
    }
}
