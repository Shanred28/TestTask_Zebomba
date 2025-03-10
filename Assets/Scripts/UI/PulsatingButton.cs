using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PulsatingButton : MonoBehaviour
    {
        [SerializeField] private Button animatedButton; 
        [SerializeField] private float pulseScale = 1.1f;   
        [SerializeField] private float pulseDuration = 0.5f; 

        private Vector3 _initialScale;

        private void Awake()
        {
            _initialScale = animatedButton.transform.localScale;
        }

        private void OnEnable()
        {
            StartPulse();
        }

        private void StartPulse()
        {
            animatedButton.transform
                .DOScale(_initialScale * pulseScale, pulseDuration)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutSine);
        }

        private void OnDisable()
        {
            animatedButton.transform.DOKill();
            animatedButton.transform.localScale = _initialScale;
        }
    }
}
