using System;
using System.Collections.Generic;
using Core;
using Core.Services.Audio;
using Core.Services.ScreenOrientation;
using Cysharp.Threading.Tasks;

namespace Runtime.Services
{
    public class SettingsProvider : ISettingProvider
    {
        private readonly IAssetProvider _assetProvider;

        private Dictionary<Type, BaseSettings> _settings = new Dictionary<Type, BaseSettings>();

        public SettingsProvider(IAssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
        }

        public async UniTask Initialize()
        {
            var screenOrientationConfig = await _assetProvider.Load<ScreenOrientationConfig>(ConstConfigs.ScreenOrientationConfig);
            var audioConfig = await _assetProvider.Load<AudioConfig>(ConstConfigs.AudioConfig);

            Set(screenOrientationConfig);
            Set(audioConfig);
        }

        public T Get<T>() where T : BaseSettings
        {
            if (_settings.ContainsKey(typeof(T)))
            {
                var setting = _settings[typeof(T)];
                return setting as T;
            }

            throw new Exception("No setting found");
        }

        public void Set(BaseSettings config)
        {
            if (_settings.ContainsKey(config.GetType()))
                return;

            _settings.Add(config.GetType(), config);
        }
    }
}