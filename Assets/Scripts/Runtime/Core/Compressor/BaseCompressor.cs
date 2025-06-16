namespace Core.Compressor
{
    public abstract class BaseCompressor
    {
        public abstract string Compress(string data);
        public abstract string Decompress(string data);
    }
}