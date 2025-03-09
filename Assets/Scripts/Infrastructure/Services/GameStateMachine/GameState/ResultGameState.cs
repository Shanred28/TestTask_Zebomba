using Infrastructure.Configs;
using Infrastructure.Services.Factory;

namespace Infrastructure.Services.GameStateMachine.GameState
{
    public class ResultGameState : IEnterableState, IState
    {
        private readonly IGameFactory _gameFactory;
        private readonly IConfigsProvider _configsProvider;
        
        public ResultGameState(IGameFactory gameFactory,IConfigsProvider configsProvider)
        {
            _gameFactory = gameFactory;
            _configsProvider = configsProvider;
        }
        
        public void Enter()
        {
            SpawnGameEntity();
        }
        
        private void SpawnGameEntity()
        {
            _gameFactory.CreateGameObject(_configsProvider.GetSceneConfig("ResultGame").patchMainUI);
        }
    }
}
