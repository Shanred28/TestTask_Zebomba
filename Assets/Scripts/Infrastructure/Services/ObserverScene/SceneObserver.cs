using DG.Tweening;
using Infrastructure.Services.GameStateMachine;
using Infrastructure.Services.GameStateMachine.GameState;
using UI;

namespace Infrastructure.Services.ObserverScene
{
    public class SceneObserver : ISceneObserver
    {
        private readonly IGameStateSwitcher _gameStateSwitcher;

        public SceneObserver(IGameStateSwitcher gameStateSwitcher)
        {
            _gameStateSwitcher = gameStateSwitcher;
        }

        public void ChangeState(EndGameAction endGameAction)
        {
            DOTween.KillAll();
            switch (endGameAction)
            {
                case EndGameAction.Restart:
                case EndGameAction.GoGame:
                    _gameStateSwitcher.EnterState<LoadGameState>();
                    break;
                case EndGameAction.GoMenu:
                    _gameStateSwitcher.EnterState<LoadMainMenuGameState>();
                    break;
                case EndGameAction.Result:
                    _gameStateSwitcher.EnterState<ResultGameState>();
                    break;
            }
        }
    }
}