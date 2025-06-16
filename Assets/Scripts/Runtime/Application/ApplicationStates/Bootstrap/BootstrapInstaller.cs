using Core.StateMachine;
using UnityEngine;
using Zenject;

namespace Runtime.GameStateMachine
{
    [CreateAssetMenu(fileName = "BootstrapInstaller", menuName = "Installers/BootstrapInstaller")]
    public class BootstrapInstaller : ScriptableObjectInstaller<BootstrapInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<Bootstrapper>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<BootstrapState>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameState>().AsSingle();
            Container.Bind<StateMachine>().AsTransient();

            Container.Bind<AudioSettingsBootstrapController>().AsSingle();
            Container.Bind<UserDataStateChangeController>().AsSingle();
        }
    }
}