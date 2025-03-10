using Infrastructure.Configs;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Infrastructure.Services.GameStateMachine.GameState
{
    public class BootstrapGameState : IEnterableState, IService, IState
    {
        private readonly IGameStateSwitcher _gameStateSwitcher;
        private readonly IConfigsProvider _configsProvider;
    
        public BootstrapGameState(IGameStateSwitcher gameStateSwitcher,IConfigsProvider configsProvider)
        {
            _gameStateSwitcher = gameStateSwitcher;
            _configsProvider = configsProvider;
        }
    
        public void Enter()
        {
            _configsProvider.Load();
            Application.targetFrameRate = 60;
        
            if (SceneManager.GetActiveScene().name == "BootstrapScene" || SceneManager.GetActiveScene().name == "MainMenuScene")
            {
                _gameStateSwitcher.EnterState<LoadMainMenuGameState>();
            }
        }
    }
}
