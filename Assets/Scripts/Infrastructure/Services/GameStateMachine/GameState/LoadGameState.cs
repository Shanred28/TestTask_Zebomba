using Infrastructure.Configs;
using Infrastructure.Services.Factory;

namespace Infrastructure.Services.GameStateMachine.GameState
{
    public class LoadGameState : IEnterableState, IState
    {
        private readonly ISceneLoader _sceneLoader;
        private readonly IGameFactory _gameFactory;
        private readonly IConfigsProvider _configsProvider;
        
        public LoadGameState(ISceneLoader sceneLoader,IGameFactory gameFactory,IConfigsProvider configsProvider)
        {
            _sceneLoader = sceneLoader;
            _gameFactory = gameFactory;
            _configsProvider = configsProvider;
        }
        
        public void Enter()
        {
            _sceneLoader.Load("GameScene", () =>
            {
                InitializeScene();
            });
        }
        
        private void InitializeScene()
        {
            SpawnGameEntity();
        }

        private void SpawnGameEntity()
        {
            var prefabs = _configsProvider.GetSceneConfig("GameScene").patchPrefabsInSceneCreate;
            foreach (var t in prefabs)
            {
                _gameFactory.CreateGameObject(t);
            }
        }
    }
}
