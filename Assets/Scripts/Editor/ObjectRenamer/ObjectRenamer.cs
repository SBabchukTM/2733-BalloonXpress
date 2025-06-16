using Tools.ObjectRenamer;
using UnityEditor;

namespace Editor.ObjectRenamer
{
    public class ObjectRenamer
    {
        [MenuItem("Tools/Object Renamer")]
        public static void CreatePopup()
        {
            ObjectRenamerWindow.InitWindow();
        }
    }
}