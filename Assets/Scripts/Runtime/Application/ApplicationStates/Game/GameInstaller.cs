using UnityEngine;
using Zenject;

namespace Runtime.Game
{
    [CreateAssetMenu(fileName = "GameInstaller", menuName = "Installers/GameInstaller")]
    public class GameInstaller : ScriptableObjectInstaller<GameInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<AccountStateController>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameplayStateController>().AsSingle();
            Container.BindInterfacesAndSelfTo<LeaderboardStateController>().AsSingle();
            Container.BindInterfacesAndSelfTo<LevelSelectionStateController>().AsSingle();
            Container.Bind<MenuStateController>().AsSingle();
            Container.Bind<SettingsStateController>().AsSingle();
            Container.Bind<StartSettingsController>().AsSingle();
        }
    }
}