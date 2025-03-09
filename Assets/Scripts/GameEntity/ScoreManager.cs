using Lean.Pool;
using UnityEngine;

namespace GameEntity
{
    public class ScoreManager : MonoBehaviour
    {
        public static ScoreManager Instance { get; private set; }
        public int CurrentScore { get; private set; }

        [Tooltip("Префаб попапа для отображения набранных очков (с компонентом ScorePopup)")]
        public ScorePopup scorePopupPrefab;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        public void AddScore(int points, Vector3 worldPosition)
        {
            CurrentScore += points;
            Debug.Log("Total Score: " + CurrentScore);

            ShowScorePopup(points, worldPosition);
        }
        
        private void ShowScorePopup(int points, Vector3 worldPosition)
        {
            ScorePopup popupObj = LeanPool.Spawn(scorePopupPrefab, worldPosition, Quaternion.identity);
            popupObj.Initialize(points);
        }
    }
}
