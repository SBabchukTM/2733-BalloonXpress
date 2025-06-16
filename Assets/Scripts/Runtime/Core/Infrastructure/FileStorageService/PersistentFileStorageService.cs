using System;
using System.IO;
using UnityEngine;

namespace Core
{
    public sealed class PersistentFileStorageService : IFileStorageService
    {
        private readonly ILogger _logger;

        public PersistentFileStorageService(ILogger logger)
        {
            _logger = logger;
        }

        public void SaveText(string data, string filePath, string fileName)
        {
            if(string.IsNullOrEmpty(fileName))
            {
                _logger.Error("Failed save text, no file name");
                return;
            }
            
            if(string.IsNullOrEmpty(filePath))
            {
                _logger.Error("Failed save text, no file path");
                return;
            }

            if(string.IsNullOrEmpty(data))
            {
                _logger.Error("Failed save text, data is empty");
                return;
            }

            try
            {
                if(!Directory.Exists(filePath))
                    Directory.CreateDirectory(filePath);

                var path = Path.Combine(filePath, fileName);
                File.WriteAllText(path, data);
            }
            catch (Exception e)
            {
                _logger.Error($"Failed save text: {e}");
            }
        }

        public void SaveBytes(byte[] data, string filePath, string fileName)
        {
            if(string.IsNullOrEmpty(fileName))
            {
                _logger.Error("Failed save data, no file name");
                return;
            }
            
            if(string.IsNullOrEmpty(filePath))
            {
                _logger.Error("Failed save data, no file path");
                return;
            }

            if(data == null)
            {
                _logger.Error("Failed save data, data is empty");
                return;
            }

            var path = Path.Combine(filePath, fileName);

            try
            {
                if(!Directory.Exists(filePath))
                    Directory.CreateDirectory(filePath);

                File.WriteAllBytes(path, data);
                _logger.Log("Save data to: " + path);
            }
            catch (Exception e)
            {
                _logger.Error("Failed to save data to: " + path);
                _logger.Error("Error " + e.Message);
            }
        }

        public string LoadText(string filePath, string fileName)
        {
            if(string.IsNullOrEmpty(filePath))
            {
                _logger.Error("Failed load text, path is empty");
                return null;
            }

            if(string.IsNullOrEmpty(fileName))
            {
                _logger.Error("Failed load text, no file name");
                return null;
            }

            if(!Directory.Exists(Path.GetDirectoryName(filePath)))
            {
                _logger.Warning($"Directory not exist: {filePath}");
                return null;
            }

            var path = Path.Combine(filePath, fileName);

            if(!File.Exists(path))
            {
                _logger.Warning($"File not exist: {path}");
                return null;
            }

            try
            {
                return File.ReadAllText(path);
            }
            catch (Exception e)
            {
                _logger.Error($"Failed load text: {e}");
                return null;
            }
        }

        public byte[] LoadBytes(string filePath, string fileName)
        {
            if(string.IsNullOrEmpty(filePath))
            {
                _logger.Error("Failed load data, path is empty");
                return null;
            }

            if(string.IsNullOrEmpty(fileName))
            {
                _logger.Error("Failed load data, no file name");
                return null;
            }

            if(!Directory.Exists(Path.GetDirectoryName(filePath)))
            {
                _logger.Warning($"Directory not exist: {filePath}");
                return null;
            }

            var path = Path.Combine(filePath, fileName);

            if(!File.Exists(path))
            {
                _logger.Warning($"File not exist: {filePath}");
                return null;
            }

            byte[] bytes = null;

            try
            {
                bytes = File.ReadAllBytes(path);
                _logger.Log("<color=green>Loaded all data from: </color>" + path);
            }
            catch (Exception e)
            {
                _logger.Warning("Exception. Failed to load data from: " + path);
                _logger.Warning("Error: " + e.Message);
                return null;
            }

            return bytes;
        }
    }
}