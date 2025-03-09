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
        
        private readonly Color _defaultColorImage =Color.white;
        private readonly Color _defaultColorText =Color.red;

        public void Initialize(int points)
        {
            scoreText.text = points.ToString();
            scoreText.DOFade(0f, duration).SetEase(Ease.Linear);
            popupImage.DOFade(0, duration).SetEase(Ease.Linear);
            popupImage.rectTransform
                .DOScale(1.1f, 0.5f)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutSine);
            
            transform.DOMoveY(transform.position.y + moveUpDistance, duration)
                .SetEase(Ease.OutQuad)
                .OnComplete(() =>
                {
                    ResetDefault();
                    LeanPool.Despawn(gameObject);
                });
        }

        private void ResetDefault()
        {
            popupImage.color = _defaultColorImage;
            scoreText.color = _defaultColorText;
        }
    }
}
