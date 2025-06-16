using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace Tools.ObjectRenamer
{
    public class ObjectRenamerWindow : EditorWindow
    {
        private const string ObjectRenamerWindowName = "Object Renamer Window";

        private static readonly Regex _whiteSpacesRegex = new Regex(@"\s+");

        private static ObjectRenamerWindow _objectRenamerWindow;
        private static float _objectsCount;
        private static string _prefix;
        private static string _name;
        private static string _suffix;
        private static bool _isNeedNumber;
        private static int _selectedObjectNumber;

        public static void InitWindow()
        {
            _objectRenamerWindow = GetWindow<ObjectRenamerWindow>(ObjectRenamerWindowName);
            _objectRenamerWindow.Show();
            Debug.LogWarning("show");
        }

        private void OnGUI()
        {
            GUILayout.Label("Rename your Objects easy!");

            _objectsCount = Selection.gameObjects.Length;
            GUILayout.Label($"Selected Objects Count: {_objectsCount}", EditorStyles.boldLabel);
            EditorGUILayout.Space(200, false);

            CreateTextBars();

            if (GUILayout.Button("Rename Objects"))
                RenameObjects();

            RepaintWindow();
        }

        private static void RenameObjects()
        {
            _selectedObjectNumber = 0;

            foreach (var selectedObject in Selection.gameObjects)
                SetSelectedObjectName(selectedObject);

            static void SetSelectedObjectName(Object selectedObject)
            {
                selectedObject.name = $"{_prefix}_{_name}_{_suffix}";

                if (_isNeedNumber)
                    selectedObject.name += $"_{_selectedObjectNumber++}";

                EditorUtility.SetDirty(selectedObject);
            }
        }

        private static void RepaintWindow()
        {
            _objectRenamerWindow?.Repaint();
        }

        private static void CreateTextBars()
        {
            SetTextFields();
            RemoveWhiteSpaceFromFields();
        }

        private static void RemoveWhiteSpaceFromFields()
        {
            _prefix = ReplaceWhitespace(_prefix);
            _name = ReplaceWhitespace(_name);
            _suffix = ReplaceWhitespace(_suffix);
        }

        private static void SetTextFields()
        {
            _prefix = EditorGUILayout.TextField("Prefix: ", _prefix);
            _name = EditorGUILayout.TextField("Name: ", _name);
            _suffix = EditorGUILayout.TextField("Suffix: ", _suffix);
            _isNeedNumber = EditorGUILayout.Toggle("Include Number: ", _isNeedNumber);
        }

        private static string ReplaceWhitespace(string input)
        {
            return string.IsNullOrEmpty(input) ? input : _whiteSpacesRegex.Replace(input, "");
        }
    }
}