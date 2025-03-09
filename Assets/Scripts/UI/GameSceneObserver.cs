using GameEntity.Grid;
using Infrastructure.Services.ObserverScene;
using UnityEngine;
using VContainer;

namespace UI
{
    public enum EndGameAction
    {
        GoMenu,
        GoGame,
        Restart,
        Result
    }

    public class GameSceneObserver : MonoBehaviour
    {
        private ISceneObserver _sceneObserver;

        [Inject]
        public void Construct(ISceneObserver sceneObserver)
        {
            _sceneObserver = sceneObserver;
        }

        private void Start()
        {
            FindAnyObjectByType<CircleGridManager>().OnEndedGame += (t) => GameEnded(t);
        }

        private void GameEnded(EndGameAction endGameAction)
        {
            _sceneObserver.ChangeState(endGameAction);
        }
    }
}
