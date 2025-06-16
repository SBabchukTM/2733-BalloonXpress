namespace Core
{
    public interface IFileCleaner
    {
        bool TryCleanFolder(string folderPath);
        bool DestroyFolder(string filePath);
    }
}