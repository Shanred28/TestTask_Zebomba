using Infrastructure.Services.GameStateMachine;
using Infrastructure.Services.GameStateMachine.GameState;
using VContainer;
using VContainer.Unity;

namespace Infrastructure.Bootstraper
{
    public class GameBootstrapper : MonoBootstrapper, IStartable
    {
        private  IGameStateSwitcher _gameStateSwitcher;
        private  BootstrapGameState _bootstrapGameState;
        private LoadMainMenuGameState _loadMainMenuGameState;
        private ResultGameState _resultGameState;
        private LoadGameState _loadGameState;
        
        [Inject]
        public void Constructor(IGameStateSwitcher gameStateSwitcher, BootstrapGameState bootstrapGameState,LoadMainMenuGameState loadMainMenuGameState,LoadGameState loadGameState,ResultGameState resultGameState)
        {
            _gameStateSwitcher = gameStateSwitcher;
            _bootstrapGameState = bootstrapGameState;
            _loadMainMenuGameState = loadMainMenuGameState;
            _loadGameState = loadGameState;
            _resultGameState = resultGameState;
        }
        public override void OnBindResolved()
        {
            InitGameStateMachine();
        }
        
        void IStartable.Start()
        {
            OnBindResolved();
        }
        
        private void InitGameStateMachine()
        {
            _gameStateSwitcher.AddState(_bootstrapGameState);
            _gameStateSwitcher.AddState(_loadMainMenuGameState);
            _gameStateSwitcher.AddState(_loadGameState);
            _gameStateSwitcher.AddState(_resultGameState);
            
            _gameStateSwitcher.EnterState<BootstrapGameState>();
        }
    }
}
