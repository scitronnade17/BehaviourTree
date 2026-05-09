using UnityEngine;
using Zenject;

public class ProjectInstaller : MonoInstaller, ICoroutineRunner
{
    [SerializeField] private LoadingCurtain loadingCurtainPrefab;
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<BootInitializeSystem>().AsSingle().NonLazy();

        Container.Bind<ICoroutineRunner>().FromInstance(this).AsSingle();
        Container.Bind<IDIService>().To<DIService>().AsSingle();
        Container.Bind<ISceneLoader>().To<SceneLoader>().AsSingle();
        Container.Bind<IInputService>().To<InputService>().AsSingle();

        Container.Bind<ILoadingCurtain>()
            .FromComponentInNewPrefab(loadingCurtainPrefab)
            .AsSingle()
            .NonLazy();

        BindSignalBus();
        BindGameStateMachine();
    }

    private void BindGameStateMachine()
    {
        Container.Bind<IStateFactory>().To<StateFactory>().AsSingle();
        Container.BindInterfacesAndSelfTo<GameStateMachine>().AsSingle();

        Container.BindInterfacesAndSelfTo<BootstrapState>().AsSingle();
        Container.BindInterfacesAndSelfTo<LoadLevelState>().AsSingle();
        Container.BindInterfacesAndSelfTo<LevelLoopState>().AsSingle();
    }

    private void BindSignalBus()
    {
        SignalBusInstaller.Install(Container);
        Container.Bind<IEventBus>().To<ZenjectEventBus>().AsSingle().NonLazy();
        //bind signals
    }
}