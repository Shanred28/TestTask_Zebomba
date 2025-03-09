using DG.Tweening;
using Infrastructure.Configs;
using Infrastructure.Services.Factory;
using UnityEngine;

namespace Infrastructure.Services.GameStateMachine.GameState
{
    public class LoadMainMenuGameState : IEnterableState, IExitableState, IState
    {
        private readonly ISceneLoader _sceneLoader;
        private readonly IGameFactory _gameFactory;
        private readonly IConfigsProvider _configsProvider;
    
        public LoadMainMenuGameState(ISceneLoader sceneLoader,IGameFactory gameFactory,IConfigsProvider configsProvider)
        {
            _sceneLoader = sceneLoader;
            _gameFactory = gameFactory;
            _configsProvider = configsProvider;
        }
    
        public void Enter()
        {
            Debug.Log("LoadMainMenuGameState.Enter");
            _sceneLoader.Load("MainMenuScene", () =>
            {
                InitializeScene();
            });
        }

        private void InitializeScene()
        {
            CreateUI();
            StartAnimationSpawner();
        }

        private void CreateUI()
        {
            _gameFactory.CreateGameObject(_configsProvider.GetSceneConfig("MainMenuScene").patchMainUI);
        }

        private void StartAnimationSpawner()
        {
            var prefabs = _configsProvider.GetSceneConfig("MainMenuScene").patchPrefabsInSceneCreate;
            
            foreach (var t in prefabs)
            {
                _gameFactory.CreateGameObject(t);
            }
        }

        public void Exit()
        {
            DOTween.KillAll();
        }
    }
}
