using System.Collections;
using GameEntity.Grid;
using UnityEngine;

namespace GameEntity.Circle
{
    public class CircleDropper : MonoBehaviour
    {
        public CircleColorType CircleColorType => _circleColorType;
        public Rigidbody2D RbRigidbody2D => rb;
    
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private SpriteRenderer spriteRenderer;

        [Header("Настройки импульса")]
        [SerializeField] private float impulseMultiplier = 0.02f;
        [SerializeField] private float landingVelocityThreshold = 0.001f;
        
        private CircleGridManager _circleGridManager;
        private Transform _pointRotation;
        private Pendulum _pendulumScript;
        private CircleColorType _circleColorType;
    
        public void Initialize(Pendulum pendulumScript,Transform pointRotation,CircleGridManager circleGridManager)
        {
            _pendulumScript = pendulumScript;
            _pointRotation = pointRotation;
            _circleGridManager = circleGridManager;
            SetColorType();
            rb.bodyType = RigidbodyType2D.Kinematic;
            InputController.OnDropClick +=  Drop;
        }

        private void OnDestroy()
        {
            InputController.OnDropClick -=  Drop;
        }
        
        private void SetColorType()
        {
            _circleColorType = CircleColor.GetRandomColor();
            spriteRenderer.color = CircleColor.GetColorByType(_circleColorType);
        }

        private void Drop()
        {
            InputController.OnDropClick -=  Drop;
            Transform parentTransform = transform.parent;
            if (parentTransform != null)
                transform.parent = null;
        
            rb.bodyType = RigidbodyType2D.Dynamic;
            Vector2 impulse = Vector2.zero;

            if (_pendulumScript != null && parentTransform != null)
            {
                Vector2 radius = (Vector2)transform.position - (Vector2)_pointRotation.position;
                Vector2 tangent = new Vector2(-radius.y, radius.x).normalized;
                float angularVelocity = _pendulumScript.CurrentAngularVelocity;
            
                impulse = tangent * angularVelocity * impulseMultiplier;
            }
        
            rb.AddForce(impulse, ForceMode2D.Impulse);
            StartCoroutine(WaitForLandingCoroutine());
        }
    
        private IEnumerator WaitForLandingCoroutine()
        {
            yield return new WaitUntil(() => rb.linearVelocity.magnitude < landingVelocityThreshold);
        
            Landed();
        }

        private void Landed() => _circleGridManager.AttachCircle(this);
    }
}
