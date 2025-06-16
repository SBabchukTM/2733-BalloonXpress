using System;
using System.IO;
using System.IO.Compression;

namespace Core.Compressor
{
    public class ZipCompressor : BaseCompressor
    {
        public override string Compress(string data)
        {
            byte[] binaryData = System.Text.Encoding.UTF8.GetBytes(data);

            using (MemoryStream compressedStream = new MemoryStream())
            {
                using (GZipStream gZipStream = new GZipStream(compressedStream, CompressionMode.Compress))
                {
                    gZipStream.Write(binaryData, 0, binaryData.Length);
                }

                return Convert.ToBase64String(compressedStream.ToArray());
            }
        }

        public override string Decompress(string compressedData)
        {
            byte[] compressedBytes = Convert.FromBase64String(compressedData);

            using (MemoryStream compressedStream = new MemoryStream(compressedBytes))
            {
                using (GZipStream gZipStream = new GZipStream(compressedStream, CompressionMode.Decompress))
                {
                    using (MemoryStream decompressedStream = new MemoryStream())
                    {
                        gZipStream.CopyTo(decompressedStream);
                        decompressedStream.Position = 0;

                        using (StreamReader reader = new StreamReader(decompressedStream))
                        {
                            string originalString = reader.ReadToEnd();
                            return originalString;
                        }
                    }
                }
            }
        }
    }
}