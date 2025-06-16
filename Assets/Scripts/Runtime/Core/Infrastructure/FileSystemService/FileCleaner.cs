using System.IO;
using UnityEngine;

namespace Core
{
    public class FileCleaner : IFileCleaner
    {
        public bool TryCleanFolder(string folderPath)
        {
            if (!folderPath.Contains(UnityEngine.Application.persistentDataPath))
            {
                return false;
            }

            if (!Directory.Exists(folderPath))
            {
                return false;
            }

            ClearCache(new DirectoryInfo(folderPath));
            ClearEmptyDir(folderPath);
            return true;
        }
        
        private static void ClearCache(DirectoryInfo d) 
        {    
            FileInfo[] fis = d.GetFiles();
            for (int i = 0; i < fis.Length; i++)
            {
                File.Delete(fis[i].FullName);
            }
            
            DirectoryInfo[] dis = d.GetDirectories();
            for (int i = 0; i < dis.Length; i++)
            {
                ClearCache(dis[i]);
            }
        }
        
        private static void ClearEmptyDir(string startLocation)
        {
            foreach (var directory in Directory.GetDirectories(startLocation))
            {
                ClearEmptyDir(directory);
                if (Directory.GetFiles(directory).Length == 0 && 
                    Directory.GetDirectories(directory).Length == 0)
                {
                    Directory.Delete(directory, false);
                }
            }
        }

        public bool DestroyFolder(string filePath)
        {
            if (Directory.Exists(filePath))
            {
                Directory.Delete(filePath);
                return true;
            }

            return false;
        }
    }
}