using DG.Tweening;
using Lean.Pool;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameEntity
{
    public class ScorePopup : MonoBehaviour
    {
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private float duration = 3f;
        [SerializeField] private Image popupImage;
        [SerializeField] private float moveUpDistance = 10f;
        [SerializeField] private float minScale = 1.1f;
        [SerializeField] private float durationScale = 0.5f;
        
        private readonly Color _defaultColorImage =Color.white;
        private readonly Color _defaultColorText =Color.red;
        private Sequence _sequence;

        private void OnDestroy()
        {
            _sequence?.Kill();
        }

        public void Initialize(int points)
        {
            {
                scoreText.text = points.ToString();
                _sequence= DOTween.Sequence();

                _sequence.Join(scoreText.DOFade(0f, duration).SetEase(Ease.Linear));
                _sequence.Join(popupImage.DOFade(0, duration).SetEase(Ease.Linear));
                _sequence.Join(transform.DOMoveY(transform.position.y + moveUpDistance, duration).SetEase(Ease.OutQuad));
    
                _sequence.OnComplete(() =>
                {
                    ResetDefault();
                    LeanPool.Despawn(gameObject);
                });

                // Запускаем анимацию пульсации отдельно, вне последовательности
                popupImage.rectTransform
                    .DOScale(minScale, durationScale)
                    .SetLoops(-1, LoopType.Yoyo);

            }
        }

        private void ResetDefault()
        {
            popupImage.color = _defaultColorImage;
            scoreText.color = _defaultColorText;
        }
    }
}
