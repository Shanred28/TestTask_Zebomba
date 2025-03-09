using GameEntity;
using Infrastructure.Services.ObserverScene;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace UI
{
    public class ResultUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text resultScoreText;
        [SerializeField] private Button newGameButton;
        [SerializeField] private Button backMenuButton;
        
        private ISceneObserver _sceneObserver;
        
        [Inject]
        public void Construct(ISceneObserver sceneObserver)
        {
            _sceneObserver = sceneObserver;
        }

        private void Start()
        {
            newGameButton.onClick.AddListener(OnClickedNewGame);
            backMenuButton.onClick.AddListener(OnBackMenuButtonClicked);
            resultScoreText.text = ScoreManager.Instance.CurrentScore.ToString();
        }

        private void OnDestroy()
        {
            newGameButton.onClick.RemoveListener(OnClickedNewGame);
            backMenuButton.onClick.RemoveListener(OnBackMenuButtonClicked);
        }

        private void OnClickedNewGame()
        {
            _sceneObserver.ChangeState(EndGameAction.Restart);
        }

        private void OnBackMenuButtonClicked()
        {
            _sceneObserver.ChangeState(EndGameAction.GoMenu);
        }
    }
}
