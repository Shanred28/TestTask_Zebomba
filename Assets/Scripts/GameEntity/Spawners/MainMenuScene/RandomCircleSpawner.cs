using Common.Timer.Source;
using DG.Tweening;
using UnityEngine;

namespace GameEntity.Spawners.MainMenuScene
{
    public class RandomCircleSpawner : SpawnerBase
    {
        [Header("Настройки Random Spawner")]
        [Tooltip("Интервал между спавнами в секундах")]
        public float spawnInterval = 1.5f;

        [Tooltip("Скорость падения (единиц в секунду)")]
        public float fallSpeed = 5f;

        [Tooltip("Длительность появления (масштабирование от 0 до 1)")]
        public float appearDuration = 0.3f;

        [Tooltip("Длительность исчезания")] public float fadeDuration = 0.3f;

        private float _minX, _maxX, _minY, _maxY;
        private float _spawnYOffset = 0.5f;

        protected override void Initialize()
        {
            Camera cam = Camera.main;
            if (cam == null)
            {
                Debug.LogError("Основная камера не найдена!");
                return;
            }
            
            float halfHeight = cam.orthographicSize;
            float halfWidth = halfHeight * cam.aspect;
            _minX = cam.transform.position.x - halfWidth;
            _maxX = cam.transform.position.x + halfWidth;
            _maxY = cam.transform.position.y + halfHeight;
            _minY = cam.transform.position.y - halfHeight;

            SpawnLoop();
        }
        
        private void SpawnLoop()
        {
            Timer.Register(this, spawnInterval, () =>
            {
                SpawnCircle();
            }).isLooped = true;
        }

        private void SpawnCircle()
        {
            float spawnX = Random.Range(_minX, _maxX);
            float spawnY = _maxY + _spawnYOffset;
            Vector3 spawnPos = new Vector3(spawnX, spawnY, 0);
            
            GameObject circle = SpawnGameObject(spawnPos, Quaternion.identity);
            
            Sequence seq = DOTween.Sequence();
            
            circle.transform.localScale = Vector3.zero;
            seq.Append(circle.transform.DOScale(1f, appearDuration));
            
            float targetY = Random.Range(_minY, _maxY);
            Vector3 targetPos = new Vector3(spawnX, targetY, 0);
            
            float distance = spawnY - targetY;
            float moveDuration = distance / fallSpeed;
            seq.Append(circle.transform.DOMove(targetPos, moveDuration).SetEase(Ease.Linear));
            
            SpriteRenderer sr = circle.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                seq.Append(sr.DOFade(0f, fadeDuration));
            }
            else
            {
                seq.Append(circle.transform.DOScale(0f, fadeDuration));
            }
            
            seq.OnComplete(() =>
            {
                if (sr != null)
                {
                    Color col = sr.color;
                    col.a = 1f;
                    sr.color = col;
                }

                DespawnGameObject(circle);
            });
        }
    }
}
