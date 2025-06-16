namespace Core
{
    public interface IFileStorageService
    {
        void SaveText(string data, string filePath, string fileName);
        void SaveBytes(byte[] data, string filePath, string fileName);
        string LoadText(string filePath, string fileName);
        byte[] LoadBytes(string filePath, string fileName);
    }
}