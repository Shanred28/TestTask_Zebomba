using Infrastructure.Services.ObserverScene;
using UnityEngine;
using VContainer;

namespace UI
{
    public class MainMenuSceneObserver : MonoBehaviour
    {
        private ISceneObserver _sceneObserver;

        [Inject]
        public void Construct(ISceneObserver sceneObserver)
        {
            _sceneObserver = sceneObserver;
        }

        public void OnPlayButtonPressed()
        {
            _sceneObserver.ChangeState(EndGameAction.GoGame);
        }
    }
}
