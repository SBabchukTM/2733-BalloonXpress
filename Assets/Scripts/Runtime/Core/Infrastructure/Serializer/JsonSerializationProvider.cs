using System;
using UnityEngine;

namespace Core
{
    public class JsonSerializationProvider : ISerializationProvider
    {
        protected readonly ILogger _logger;


        public JsonSerializationProvider(ILogger logger)
        {
            _logger = logger;
        }

        public T Deserialize<T>(string text) where T : class
        {
            try
            {
                var result = JsonUtility.FromJson<T>(text);

                return result;
            }
            catch (Exception exception)
            {
                _logger.Error($"{exception.GetType()}: Could not parse JSON {text}. Exception: {exception.Message}");
                return default;
            }
        }

        public string Serialize<T>(T obj) where T : class
        {
            try
            {
                var result = JsonUtility.ToJson(obj);

                return result;
            }
            catch (Exception exception)
            {
                _logger.Error(
                    $"{exception.GetType()}: Could not serialize object {typeof(T)}. Exception: {exception.Message}");
                return default;
            }
        }
    }
}