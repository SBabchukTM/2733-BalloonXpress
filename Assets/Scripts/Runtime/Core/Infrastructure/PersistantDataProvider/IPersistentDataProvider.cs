using Core.Compressor;

namespace Core
{
    public interface IPersistentDataProvider
    {
        T Load<T>(string path, string fileName, ISerializationProvider serializationProvider = null, BaseCompressor compressor = null) where T : class;

        bool Save<T>(T data, string path, string fileName, ISerializationProvider serializationProvider = null, BaseCompressor compressor = null) where T : class;
    }
}