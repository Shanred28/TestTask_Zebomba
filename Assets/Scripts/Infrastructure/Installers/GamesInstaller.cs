using Infrastructure.Bootstraper;
using Infrastructure.Configs;
using Infrastructure.Services;
using Infrastructure.Services.AssetManager;
using Infrastructure.Services.Factory;
using Infrastructure.Services.GameStateMachine;
using Infrastructure.Services.GameStateMachine.GameState;
using Infrastructure.Services.ObserverScene;
using VContainer;
using VContainer.Unity;

namespace Infrastructure.Installers
{
    public class GamesInstaller : ProjectContext.ProjectContext
    {
        protected override void RegisterGameServices(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<GameBootstrapper>();
            
            builder.Register<ICoroutineRunner, CoroutineRunner>(Lifetime.Singleton);
            builder.Register<ISceneLoader, SceneLoader>(Lifetime.Singleton);
            builder.Register<IAssetProvider, AssetProvider>(Lifetime.Singleton);
            builder.Register<IConfigsProvider, ConfigsProvider>(Lifetime.Singleton);
            builder.Register<IGameFactory, GameFactory>(Lifetime.Singleton);
            builder.Register<ISceneObserver, SceneObserver>(Lifetime.Singleton);
        }

        protected override void RegisterGameStateMachine(IContainerBuilder builder)
        {
            builder.Register<IGameStateSwitcher, GameStateMachine>(Lifetime.Singleton);
            builder.Register<BootstrapGameState>(Lifetime.Singleton);
            builder.Register<LoadMainMenuGameState>(Lifetime.Singleton);
            builder.Register<LoadGameState>(Lifetime.Singleton);
            builder.Register<ResultGameState>(Lifetime.Singleton);
        }
    }
}
