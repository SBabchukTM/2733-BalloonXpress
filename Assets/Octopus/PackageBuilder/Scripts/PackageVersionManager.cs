using System.IO;
using UnityEngine;

namespace Octopus.PackageBuilder
{
    public class PackageVersionManager : MonoBehaviour
    {
        public string versionFilePath = "Assets/Octopus/PackageBuilder/Version.txt";
        
        public string initialVersion = "1.0.0";
        
        public enum VersionType
        {
            Major,
            Minor,
            Patch
        }

        public string GetCurrentVersion()
        {
            if (File.Exists(versionFilePath))
            {
                return File.ReadAllText(versionFilePath);
            }
            else
            {
                Debug.LogError("Version file not found!");
                File.WriteAllText(versionFilePath, initialVersion);
                return initialVersion;
            }
        }
        
        public string IncreaseVersion(VersionType type)
        {
            var currentVersion = GetCurrentVersion();
            var versionComponents = currentVersion.Split('.');
            var major = int.Parse(versionComponents[0]);
            var minor = int.Parse(versionComponents[1]);
            var patch = int.Parse(versionComponents[2]);

            switch (type)
            {
                case VersionType.Major:
                    major++;
                    minor = 0;
                    patch = 0;
                    break;
                case VersionType.Minor:
                    minor++;
                    patch = 0;
                    break;
                case VersionType.Patch:
                    patch++;
                    break;
            }

            var newVersion = $"{major}.{minor}.{patch}";

            return newVersion;
        }

        public void UpdateVersion(string newVersion)
        {
            if (File.Exists(versionFilePath))
            {
                File.WriteAllText(versionFilePath, newVersion);
                Debug.Log("Version updated to: " + newVersion);
            }
            else
            {
                Debug.LogError("Version file not found!");
            }
        }
    }
}