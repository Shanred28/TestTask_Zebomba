using DG.Tweening;
using UnityEngine;

namespace GameEntity
{
    public class Pendulum : MonoBehaviour
    {
        [SerializeField] private float amplitude = 30f;
        [SerializeField] private float frequency = 2f;

        public float CurrentAngularVelocity { get; private set; }
        private float _prevAngle;
        private Tween _pendulumTween;

        private void Start()
        {
            transform.rotation = Quaternion.Euler(0f, 0f, -amplitude);
            _prevAngle = -amplitude;
            
            float halfCycle = Mathf.PI / frequency;
            
            _pendulumTween = transform.DORotate(new Vector3(0f, 0f, amplitude), halfCycle, RotateMode.Fast)
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo)
                .OnUpdate(UpdateAngularVelocity);
        }

        private void OnDestroy()
        {
            _pendulumTween?.Kill();
        }

        private void UpdateAngularVelocity()
        {
            float currentAngle = transform.eulerAngles.z;
            float signedCurrentAngle = (currentAngle > 180f) ? currentAngle - 360f : currentAngle;
            
            float deltaAngle = Mathf.DeltaAngle(_prevAngle, signedCurrentAngle);
            CurrentAngularVelocity = deltaAngle / Time.deltaTime;

            _prevAngle = signedCurrentAngle;
        }
    }
}
