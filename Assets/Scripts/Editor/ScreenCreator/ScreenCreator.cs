using Runtime.UI;
using Core.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class ScreenCreator : EditorWindow
{
    public static ScreenCreator Instance;

    private const string ScreenSavePath = "Assets\\ProjectAssets\\Prefabs\\UI\\Screens";
    private const string ClassSavePath = "Assets\\Scripts\\Runtime\\Application\\UI\\Screen";

    [SerializeField] private VisualTreeAsset m_VisualTreeAsset = default;
    [SerializeField] private GameObject ScreenPrefab;
    [SerializeField] private TextAsset _scriptTemplate;
    [SerializeField] private UiServiceViewContainer _uiServiceViewContainer;
    [SerializeField] private MonoScript _constScreenNamesFile;

    public List<string> CachedAssetPaths = new ();

    private ListView _namesListView;
    private ListView _prefabsListView;

    private bool _createdNewScreen = false;

    private List<GameObject> _prefabs = new();

    private List<string> _screenNames = new();
    [SerializeField] private List<string> _validNames = new ();

    [MenuItem("Tools/Screen Creator")]
    public static void ShowExample()
    {
        ScreenCreator wnd = GetWindow<ScreenCreator>();
        wnd.titleContent = new GUIContent("Screen Creator");
    }

    public void CreateGUI()
    {
        _createdNewScreen = false;

        rootVisualElement.Add(m_VisualTreeAsset.Instantiate());
        FindFields();
    }

    private void OnEnable()
    {
        if(Instance == null)
            Instance = this;
        AssemblyReloadEvents.afterAssemblyReload += AssemblyReloadEvents_afterAssemblyReload;
    }


    private void OnDisable()
    {
        AssemblyReloadEvents.afterAssemblyReload -= AssemblyReloadEvents_afterAssemblyReload;
    }


    private void FindFields()
    {
        _namesListView = rootVisualElement.Q<ListView>("ScreenNamesList");
        _prefabsListView = rootVisualElement.Q<ListView>("PrefabsList");

        _prefabs = FindAllScreens();

        _prefabsListView.makeItem = () => new ObjectField();
        _prefabsListView.bindItem = BindGOItem;
        _prefabsListView.itemsSource = _prefabs;

        _screenNames = new();
        _screenNames.Add("");

        _namesListView.itemsSource = _screenNames;
        _namesListView.makeItem = () => new TextField();
        _namesListView.bindItem = BindNameItem;

        rootVisualElement.Q<UnityEngine.UIElements.Button>("CreateScreensButton").clicked += CreateScreen;
    }

    private List<GameObject> FindAllScreens()
    {
        List<GameObject> popupPrefabs = new List<GameObject>();

        string[] prefabGuids = AssetDatabase.FindAssets("t:Prefab");

        foreach (string guid in prefabGuids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

            if (prefab == null)
                continue;

            if (prefab.GetComponent<UiScreen>() != null)
            {
                popupPrefabs.Add(prefab);
                continue;
            }
        }

        return popupPrefabs;
    }

    private void BindNameItem(VisualElement element, int index)
    {
        var textField = (TextField)element;
        textField.value = _screenNames[index];
        textField.RegisterValueChangedCallback(evt =>
        {
            _screenNames[index] = evt.newValue;
        });
    }

    private void BindGOItem(VisualElement element, int index)
    {
        var objectField = (ObjectField)element;
        objectField.value = _prefabs[index];
    }

    private void CreateScreen()
    {
        _validNames = new();

        for (int i = 0; i < _screenNames.Count; i++)
        {
            if (ClassNameValidator.IsValid(_screenNames[i]))
                _validNames.Add(_screenNames[i]);
        }

        for(int i = 0; i < _validNames.Count; i++)
            CreateScreenClass(_validNames[i]);
    }

    private void CreateScreenClass(string name)
    {
        string templateContent = _scriptTemplate.text;
        string classContent = templateContent.Replace("#SCREENNAME#", name);

        string classFilePath = Path.Combine(ClassSavePath, name + ".cs");

        Directory.CreateDirectory(ClassSavePath);

        File.WriteAllText(classFilePath, classContent);

        AssetDatabase.Refresh();

        _createdNewScreen = true;
    }

    private void AssemblyReloadEvents_afterAssemblyReload()
    {
        if (!_createdNewScreen)
            return;

        _createdNewScreen = false;

        for(int i = 0; i< _validNames.Count; i++) 
        {
            CreateNewPrefab(_validNames[i]);
            AddConstToFile(_validNames[i]);
        }
    }

    private void CreateNewPrefab(string name)
    {
        var prefabInstance = PrefabUtility.InstantiatePrefab(ScreenPrefab) as GameObject;
        Type classType = FindTypeInAssemblies($"Application.UI.{name}");

        string prefabSavePath = Path.Combine(ScreenSavePath, name + ".prefab");

        Component component = prefabInstance.AddComponent(classType);

        AddIdToClass(component, name);

        PrefabUtility.SaveAsPrefabAsset(prefabInstance, prefabSavePath);
        CachedAssetPaths.Add(prefabSavePath);

        DestroyImmediate(prefabInstance);
    }

    private Type FindTypeInAssemblies(string fullClassName)
    {
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            Type type = assembly.GetType(fullClassName);
            if (type != null)
                return type;
        }
        return null;
    }

    private void AddIdToClass(Component component, string name)
    {
        var idField = component.GetType().GetField("_id", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
        idField.SetValue(component, name);
    }

    public void RegisterInUIServiceViewContainer(GameObject prefabAsset)
    {
        SerializedObject serializedUiServiceViewContainer = new SerializedObject(_uiServiceViewContainer);

        UiScreen uiScreenComponent = prefabAsset.GetComponent<UiScreen>();

        SerializedProperty screensPrefabProperty = serializedUiServiceViewContainer.FindProperty("_screensPrefab");

        screensPrefabProperty.arraySize++;
        screensPrefabProperty.GetArrayElementAtIndex(screensPrefabProperty.arraySize - 1).objectReferenceValue = uiScreenComponent;

        serializedUiServiceViewContainer.ApplyModifiedProperties();
    }

    private void AddConstToFile(string className)
    {
        string filePath = AssetDatabase.GetAssetPath(_constScreenNamesFile);

        string fileContent = File.ReadAllText(filePath);

        string constLine = $"        public const string {className} = \"{className}\";";

        string pattern = @"public\s+static\s+class\s+ConstScreens\s*\{([^}]*)\}";
        var match = Regex.Match(fileContent, pattern);

        if (match.Success)
        {
            int insertIndex = match.Index + match.Length - 1;

            fileContent = fileContent.Insert(insertIndex - 1, "\n" + constLine);

            fileContent = Regex.Replace(fileContent, @"\n\s*\n", "\n");

            File.WriteAllText(filePath, fileContent);

            AssetDatabase.Refresh();
        }
        else
        {
            Debug.LogError("Failed to locate ConstScreens class in the file.");
        }
    }

    public class ScreenSaverPostProcessor : AssetPostprocessor
    {
        public static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            if (ScreenCreator.Instance == null)
                return;

            if (ScreenCreator.Instance.CachedAssetPaths == null)
                return;


            List<string> cachedPaths = ScreenCreator.Instance.CachedAssetPaths;
            for (int i = 0; i < cachedPaths.Count; i++)
            {
                string targetPath = cachedPaths[i].Replace("\\", "/");
                for (int j = 0; j < importedAssets.Length; j++)
                {
                    if (importedAssets[j] == targetPath)
                    {
                        GameObject go = AssetDatabase.LoadAssetAtPath(targetPath, typeof(GameObject)) as GameObject;
                        ScreenCreator.Instance.RegisterInUIServiceViewContainer(go);
                    }
                }
            }
        }
    }
}
