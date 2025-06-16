using Runtime.UI;
using System.Text.RegularExpressions;
using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using System.IO;
using Core.UI;
using UnityEditor.UIElements;
using System.Collections.Generic;

public class PopupCreator : EditorWindow
{
    public static PopupCreator Instance;

    private const string PopupSavePath = "Assets\\ProjectAssets\\Prefabs\\UI\\Popups";
    private const string PopupDataSavePath = "Assets\\Scripts\\Runtime\\Application\\UI\\Popup\\Data";
    private const string ClassSavePath = "Assets\\Scripts\\Runtime\\Application\\UI\\Popup";

    [SerializeField] private VisualTreeAsset m_VisualTreeAsset = default;
    [SerializeField] private GameObject PopupPrefab;
    [SerializeField] private TextAsset _scriptTemplate;
    [SerializeField] private TextAsset _scriptDataTemplate;
    [SerializeField] private UiServiceViewContainer _uiServiceViewContainer;
    [SerializeField] private MonoScript _constPopupsNamesFile;

    private List<PopupData> _popupsDataList = new();

    private ListView _popupDataListView;
    private ListView _prefabsListView;

    private List<GameObject> _prefabs = new();

    private bool _createdNewPopup = false;

    [SerializeField] private List<PopupData> _validPopups = new();

    public List<string> CachedAssetPaths;

    [MenuItem("Tools/Popup Creator")]
    public static void ShowExample()
    {
        PopupCreator wnd = GetWindow<PopupCreator>();
        wnd.titleContent = new GUIContent("Popup Creator");
    }

    public void CreateGUI()
    {
        _createdNewPopup = false;

        rootVisualElement.Add(m_VisualTreeAsset.Instantiate());
        FindFields();
    }

    private void OnEnable()
    {
        Instance = this;
        AssemblyReloadEvents.afterAssemblyReload += AssemblyReloadEvents_afterAssemblyReload;
    }

    private void OnDisable()
    {
        Instance = null;
        AssemblyReloadEvents.afterAssemblyReload -= AssemblyReloadEvents_afterAssemblyReload;
    }


    private void FindFields()
    {
        _popupsDataList = new();
        _popupsDataList.Add(new());

        _popupDataListView = rootVisualElement.Q<ListView>("PopupNamesList");
        _prefabsListView = rootVisualElement.Q<ListView>("PrefabsList");

        _prefabs = FindAllPopups();

        _prefabsListView.makeItem = () => new ObjectField();
        _prefabsListView.bindItem = BindGOItem;
        _prefabsListView.itemsSource = _prefabs;

        _popupDataListView.itemsSource = _popupsDataList;
        _popupDataListView.makeItem = () => new CustomPopupDataElement();
        _popupDataListView.bindItem = BindPopupDataElement;

        rootVisualElement.Q<UnityEngine.UIElements.Button>("CreatePopupsButton").clicked += CreatePopup;
    }

    private List<GameObject> FindAllPopups()
    {
        List<GameObject> popupPrefabs = new List<GameObject>();

        string[] prefabGuids = AssetDatabase.FindAssets("t:Prefab");

        foreach (string guid in prefabGuids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

            if (prefab == null)
                continue;

            if (prefab.GetComponent<BasePopup>() != null)
            {
                popupPrefabs.Add(prefab);
                continue;
            }
        }

        return popupPrefabs;
    }

    private void BindPopupDataElement(VisualElement visualElement, int index)
    {
        var customElement = (CustomPopupDataElement)visualElement;

        var data = _popupsDataList[index] ??= new PopupData();

        customElement.TextField.value = _popupsDataList[index].Name;
        customElement.Toggle.value = _popupsDataList[index].CreateData;

        // Handle changes to the TextField and Toggle
        customElement.TextField.RegisterValueChangedCallback(evt =>
        {
            _popupsDataList[index].Name = evt.newValue;
        });

        customElement.Toggle.RegisterValueChangedCallback(evt =>
        {
            _popupsDataList[index].CreateData = evt.newValue;
        });
    }

    private void BindGOItem(VisualElement element, int index)
    {
        var objectField = (ObjectField)element;
        objectField.value = _prefabs[index];
    }

    private void CreatePopup()
    {
        _validPopups = new ();

        for (int i = 0; i < _popupsDataList.Count; i++)
        {
            if (ClassNameValidator.IsValid(_popupsDataList[i].Name))
                _validPopups.Add(_popupsDataList[i]);
        }

        for(int i = 0; i < _validPopups.Count; i++)
        {
            CreatePopupClass(_validPopups[i].Name);
            if (_validPopups[i].CreateData)
                CreatePopupDataClass(_validPopups[i].Name);
        }
    }

    private void CreatePopupClass(string name)
    {
        string templateContent = _scriptTemplate.text;
        string classContent = templateContent.Replace("#POPUPNAME#", name);

        string classFilePath = Path.Combine(ClassSavePath, name + ".cs");

        Directory.CreateDirectory(ClassSavePath);

        File.WriteAllText(classFilePath, classContent);

        AssetDatabase.Refresh();

        _createdNewPopup = true;
    }
    
    private void CreatePopupDataClass(string name)
    {
        string templateContent = _scriptDataTemplate.text;
        string classContent = templateContent.Replace("#POPUPNAME#", name);

        string classFilePath = Path.Combine(PopupDataSavePath, name + "Data.cs");

        Directory.CreateDirectory(PopupDataSavePath);

        File.WriteAllText(classFilePath, classContent);

        AssetDatabase.Refresh();
    }

    private void AssemblyReloadEvents_afterAssemblyReload()
    {
        if (!_createdNewPopup)
            return;

        _createdNewPopup = false;

        for(int i = 0; i < _validPopups.Count;i++)
        {
            CreateNewPrefab(_validPopups[i].Name);
            AddConstToFile(_validPopups[i].Name);
        }
    }

    private void CreateNewPrefab(string name)
    {
        var prefabInstance = PrefabUtility.InstantiatePrefab(PopupPrefab) as GameObject;
        Type classType = FindTypeInAssemblies($"Core.UI.{name}");

        string prefabSavePath = Path.Combine(PopupSavePath, name + ".prefab");

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

        BasePopup basePopupComponent = prefabAsset.GetComponent<BasePopup>();

        SerializedProperty popupsPrefabProperty = serializedUiServiceViewContainer.FindProperty("_popupsPrefab");

        popupsPrefabProperty.arraySize++;
        popupsPrefabProperty.GetArrayElementAtIndex(popupsPrefabProperty.arraySize - 1).objectReferenceValue = basePopupComponent;

        serializedUiServiceViewContainer.ApplyModifiedProperties();
    }

    private void AddConstToFile(string className)
    {
        string filePath = AssetDatabase.GetAssetPath(_constPopupsNamesFile);

        string fileContent = File.ReadAllText(filePath);

        string constLine = $"        public const string {className} = \"{className}\";";

        string pattern = @"public\s+static\s+class\s+ConstPopups\s*\{([^}]*)\}";

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
            Debug.LogError("Failed to locate ConstPopups class in the file.");
        }
    }

    public class PopupSaverPostProcessor : AssetPostprocessor
    {
        public static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            if (PopupCreator.Instance == null)
                return;

            if (PopupCreator.Instance.CachedAssetPaths == null)
                return;

            List<string> cachedPaths = PopupCreator.Instance.CachedAssetPaths;
            for (int i = 0; i < cachedPaths.Count; i++)
            {
                string targetPath = cachedPaths[i].Replace("\\", "/");
                for (int j = 0; j < importedAssets.Length; j++)
                {
                    if (importedAssets[j] == targetPath)
                    {
                        GameObject go = AssetDatabase.LoadAssetAtPath(targetPath, typeof(GameObject)) as GameObject;
                        PopupCreator.Instance.RegisterInUIServiceViewContainer(go);
                    }
                }
            }
        }
    }

    [Serializable]
    public class PopupData
    {
        public string Name;
        public bool CreateData;

        public PopupData()
        {
            Name = "";
            CreateData = false;
        }
    }

    public class CustomPopupDataElement : VisualElement
    {
        public TextField TextField { get; private set; }
        public Toggle Toggle { get; private set; }

        public string PopupName => TextField.text;
        public bool CreateData => Toggle.value;

        public CustomPopupDataElement()
        {
            style.flexDirection = FlexDirection.Row;
            style.alignItems = Align.FlexStart;

            TextField = new TextField { style = 
                { 
                    width = 390 
                } 
            };
            Add(TextField);

            Label label= new Label("Create Data?");
            Add(label);

            Toggle = new Toggle();
            Add(Toggle);
        }
    }
}
