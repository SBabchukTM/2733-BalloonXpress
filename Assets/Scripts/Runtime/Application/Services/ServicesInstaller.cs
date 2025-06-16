using Core;
using Runtime.IAP;
using Runtime.Services.Audio;
using Runtime.UI;
using Runtime.Services.Network;
using Runtime.Services.ApplicationState;
using Runtime.Services.UserData;
using Core.Compressor;
using Core.Factory;
using Core.Services.Audio;
using Core.Services.ScreenOrientation;
using UnityEngine;
using Zenject;
using ILogger = Core.ILogger;

namespace Runtime.Services
{
    [CreateAssetMenu(fileName = "ServicesInstaller", menuName = "Installers/ServicesInstaller")]
    public class ServicesInstaller : ScriptableObjectInstaller<ServicesInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<IUiService>().To<UiService>().AsSingle();
            Container.Bind<IAssetProvider>().To<AssetProvider>().AsSingle();
            Container.Bind<IPersistentDataProvider>().To<PersistantDataProvider>().AsSingle();
            Container.Bind<ISettingProvider>().To<SettingsProvider>().AsSingle();
            Container.Bind<ILogger>().To<SimpleLogger>().AsSingle();
            Container.Bind<IFileStorageService>().To<PersistentFileStorageService>().AsSingle();
            Container.Bind<IFileCleaner>().To<FileCleaner>().AsSingle();
            Container.Bind<ISerializationProvider>().To<JsonSerializationProvider>().AsSingle();
            Container.Bind<IAudioService>().To<AudioService>().AsSingle();
            Container.Bind<INetworkConnectionService>().To<NetworkConnectionService>().AsSingle();
            Container.Bind<BaseCompressor>().To<ZipCompressor>().AsSingle();
            Container.Bind<IIAPService>().To<IAPServiceMock>().AsSingle();
            Container.Bind<GameObjectFactory>().AsSingle();
            Container.Bind<ApplicationStateService>().AsSingle();
            Container.Bind<UserDataService>().AsSingle();
            Container.Bind<ServerProvider>().AsSingle();
            Container.BindInterfacesAndSelfTo<ScreenOrientationAlertController>().AsSingle();
        }
    }
}