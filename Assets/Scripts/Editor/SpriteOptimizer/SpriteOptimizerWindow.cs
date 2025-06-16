using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Editor.SpriteOptimizer
{
    public class SpriteOptimizerWindow : EditorWindow
    {
        private const string BackgroundCategory = "Background";
        private const string UICategory = "UI";
        private const string CharacterCategory = "Character";
        private const string PropsCategory = "Props";
        private const string ButtonsCategory = "Buttons";
        private const string LogoCategory = "Logo";
        private const string PlayerCategory = "Player";
        private const string SpriteOptimizer = "Sprite Optimizer";
        private const string SizeSettings = "Size Settings";
        private const int MINTextureSize = 32;
        private const int MAXTextureSize = 16384;

        private static readonly List<string> _categories = new List<string>();

        private static readonly Dictionary<string, int> _maxSizes = new Dictionary<string, int>
        {
            {BackgroundCategory, 2048},
            {CharacterCategory, 512},
            {UICategory, 256},
            {PropsCategory, 256},
            {ButtonsCategory, 512},
            {PlayerCategory, 512},
            {LogoCategory, 1024}
        };

        private static SpriteOptimizerWindow _spriteOptimizerWindow;

        private Vector2 _scrollPosition;
        private bool _showSettings;
        private GUIStyle _labelStyle;

        private void OnGUI()
        {
            EditorGUILayout.BeginVertical();

            CreateSpriteOptimizerLabel();

            SetSettingsOptions();

            GUILayout.FlexibleSpace();
            GUILayout.FlexibleSpace();

            var buttonStyle = SpriteOptimizerUtils.SetButtonStyle();

            SetBegin();
            SetButtons(buttonStyle);
            SetEnd();
            Repaint();
        }

        public static void InitWindow()
        {
            _spriteOptimizerWindow = GetWindow<SpriteOptimizerWindow>(SpriteOptimizer);
            _spriteOptimizerWindow.Show();

            AddCategories();
        }

        private void CreateSpriteOptimizerLabel()
        {
            SetLabelStyle();
            CreateLabel();
        }

        private void CreateLabel()
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.FlexibleSpace();
                GUILayout.Label(SpriteOptimizer, _labelStyle);
                GUILayout.FlexibleSpace();
            }
        }

        private void SetLabelStyle()
        {
            _labelStyle ??= new GUIStyle(EditorStyles.label)
            {
                fontSize = 50,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter,
                normal = {textColor = Color.white},
                richText = true,
                wordWrap = true,
            };
        }

        private void SetSettingsOptions()
        {
            _showSettings = EditorGUILayout.Foldout(_showSettings, SizeSettings);
            if (_showSettings)
            {
                _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
                foreach (var category in _categories)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(category, GUILayout.Width(100));
                    _maxSizes[category] = EditorGUILayout.IntField(_maxSizes[category]);
                    EditorGUILayout.EndHorizontal();
                }

                EditorGUILayout.EndScrollView();
            }
        }

        private static void SetEnd()
        {
            GUILayout.EndVertical();

            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            GUILayout.FlexibleSpace();
            EditorGUILayout.EndVertical();
        }

        private static void SetBegin()
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            GUILayout.BeginVertical();
            GUILayout.Space(2);
        }

        private static void AddCategories()
        {
            _categories.Clear();

            foreach (var item in _maxSizes)
                _categories.Add(item.Key);
        }

        private static void SetButtons(GUIStyle buttonStyle)
        {
            if (IsButton("Optimize Selected Sprites"))
                OptimizeSelectedSprites();

            if (IsButton("Optimize All Project Sprites"))
                OptimizeAllSprites();

            bool IsButton(string text) =>
                GUILayout.Button(text, buttonStyle, GUILayout.Height(50), GUILayout.Width(300));
        }

        private static void OptimizeSelectedSprites()
        {
            var selectedAssets = Selection.GetFiltered(typeof(Texture2D), SelectionMode.Assets);
            OptimizeSprites(selectedAssets.Cast<Texture2D>().ToArray());
        }

        private static void OptimizeAllSprites()
        {
            var guids = AssetDatabase.FindAssets("t:texture2D", null);
            var allTextures = new List<Texture2D>();

            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
                if (texture != null)
                    allTextures.Add(texture);
            }

            OptimizeSprites(allTextures.ToArray());
        }

        private static void OptimizeSprites(IEnumerable<Texture2D> textures)
        {
            foreach (var texture in textures)
            {
                var importer = SetTextureData(texture);

                if (importer == null)
                    continue;

                var maxTextureSize = SetMaxSize(texture);

                SetTextureSettings(importer, maxTextureSize);

                EditorUtility.SetDirty(texture);
                importer.SaveAndReimport();
            }

            AssetDatabase.Refresh();
            Debug.Log("Sprite optimization completed!");
        }

        private static void SetTextureSettings(TextureImporter importer, int maxTextureSize)
        {
            var settings = new TextureImporterSettings();
            importer.ReadTextureSettings(settings);

            var platformSettings = importer.GetDefaultPlatformTextureSettings();
            platformSettings.maxTextureSize = maxTextureSize;

            importer.SetPlatformTextureSettings(platformSettings);
        }

        private static int SetMaxSize(Texture2D texture)
        {
            var determineMaxSize = DetermineMaxSize(texture);

            determineMaxSize = ClosePowerOfTwo(determineMaxSize);
            return determineMaxSize;
        }

        private static TextureImporter SetTextureData(Texture2D texture)
        {
            var path = AssetDatabase.GetAssetPath(texture);
            var importer = AssetImporter.GetAtPath(path) as TextureImporter;
            return importer;
        }

        private static int ClosePowerOfTwo(int determineMaxSize)
        {
            determineMaxSize = LimitMaxSize(determineMaxSize);

            determineMaxSize = Mathf.ClosestPowerOfTwo(determineMaxSize);
            return determineMaxSize;
        }

        private static int LimitMaxSize(int determineMaxSize)
        {
            if (determineMaxSize < MINTextureSize)
                determineMaxSize = MINTextureSize;

            if (determineMaxSize > MAXTextureSize)
                determineMaxSize = MAXTextureSize;
            return determineMaxSize;
        }

        private static int DetermineMaxSize(Object texture)
        {
            var path = AssetDatabase.GetAssetPath(texture).ToLower();

            var category = GetCategory(path);

            var maxAllowedSize = _maxSizes[category];

            return maxAllowedSize;
        }

        private static string GetCategory(string path)
        {
            var isHasCategory = _categories.Count > 0 && _categories != null;
            if (isHasCategory)
                foreach (var category in _categories)
                {
                    if (category.Contains(UICategory))
                        continue;

                    if (path.Contains(category.ToLower()))
                        return category;
                }

            else
                throw new Exception(
                    $"Category is null or empty. Categories count: {_categories.Count}, check for null {_categories}");

            Debug.LogWarning($"{path} does not contains any category.\n So it was set {UICategory}");

            return UICategory;
        }

        private new static void Repaint()
        {
            if (_spriteOptimizerWindow is not null)
                ((EditorWindow) _spriteOptimizerWindow).Repaint();
        }
    }
}