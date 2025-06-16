using Core;
using UnityEditor;
using UnityEngine;

namespace Tools
{
    public class DevelopingTools : MonoBehaviour
    {
        [MenuItem("Tools/Open persistant data folder")]
        static void OpenPersistantDataFolder()
        {
            UnityEngine.Application.OpenURL(UnityEngine.Application.persistentDataPath);
        }
        
        [MenuItem("Tools/Clear cache")]
        private static void ClearCache()
        {
            var path = UnityEngine.Application.persistentDataPath;
            var fileCleaner = new FileCleaner();
            fileCleaner.TryCleanFolder(path);
        }
    }
}
