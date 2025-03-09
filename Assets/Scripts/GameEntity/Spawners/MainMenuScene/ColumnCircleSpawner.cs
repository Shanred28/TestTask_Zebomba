using System.Collections.Generic;
using Common.Timer.Source;
using DG.Tweening;
using GameEntity.Circle;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameEntity.Spawners.MainMenuScene
{
    public enum ColumnOrientation
    {
        Vertical,
        Horizontal,
        DiagonalLeft,
        DiagonalRight
    }

    public class ColumnCircleSpawner : SpawnerBase
    {
        private const int CIRCLES_IN_LINE = 3;

        [SerializeField] private float fallSpeed = 5f;
        [SerializeField] private float delayBetweenCircles = 0.5f;
        [SerializeField] private string particlePrefabPath;

        private float _circleDiameter = 1f;
        private float _spawnY;
        private float _minX, _maxX, _minY, _maxY;
        private float _lowerBound, _adjustedUpperBound;
        private readonly List<GameObject> _spawnedCircles = new List<GameObject>();

        private Sequence _spawnSequence;

        private void OnDestroy()
        {
            _spawnSequence?.Kill();
        }

        protected override void Initialize()
        {
            GameObject temp = SpawnGameObject(Vector3.zero, Quaternion.identity);

            {
                SpriteRenderer sr = temp.GetComponentInChildren<SpriteRenderer>();
                if (sr != null)
                    _circleDiameter = sr.bounds.size.x;
                DespawnGameObject(temp);
            }

            CalculationsVariables();
            StackCircles();
        }

        private void CalculationsVariables()
        {
            Camera cam = Camera.main;

            float halfWidth = cam.orthographicSize * cam.aspect;
            float halfHeight = cam.orthographicSize;
            _minX = cam.transform.position.x - halfWidth;
            _maxX = cam.transform.position.x + halfWidth;
            _minY = cam.transform.position.y - halfHeight;
            _maxY = cam.transform.position.y + halfHeight;

            _spawnY = cam.transform.position.y + cam.orthographicSize + 1f;

            _lowerBound = cam.transform.position.y - cam.orthographicSize + _circleDiameter / 2f;
            float upperBound = cam.transform.position.y + cam.orthographicSize - _circleDiameter / 2f;
            _adjustedUpperBound = upperBound - 2 * _circleDiameter;
        }

        private void StartSpawn()
        {
            StackCircles();
        }

        private void StackCircles()
        {
            var startPos = LineSelection(out var baseTarget, out var offset);

            SequenceAnimationLine(baseTarget, offset, startPos);
        }

        private void SequenceAnimationLine(Vector3 baseTarget, Vector3 offset, Vector3 startPos)
        {
            _spawnSequence = DOTween.Sequence();

            var colorCircle = CircleColor.GetRandomColor();
            var colorCircleColor = CircleColor.GetColorByType(colorCircle);

            for (int i = 0; i < CIRCLES_IN_LINE; i++)
            {
                Vector3 targetPos = baseTarget + i * offset;
                float moveDistance = Vector3.Distance(startPos, targetPos);
                float moveDuration = moveDistance / fallSpeed;

                _spawnSequence.AppendCallback(() =>
                {
                    GameObject circle = SpawnGameObject(startPos, Quaternion.identity);
                    circle.GetComponent<SpriteRenderer>().color = colorCircleColor;
                    _spawnedCircles.Add(circle);
                    circle.transform.DOMove(targetPos, moveDuration).SetEase(Ease.Linear);
                });

                _spawnSequence.AppendInterval(moveDuration + delayBetweenCircles);
            }

            _spawnSequence.AppendInterval(0.3f);
            _spawnSequence.OnComplete(() => { StartAnimation(); });
        }

        private Vector3 LineSelection(out Vector3 baseTarget, out Vector3 offset)
        {
            ColumnOrientation orientation =
                (ColumnOrientation)Random.Range(0, System.Enum.GetValues(typeof(ColumnOrientation)).Length);
            Vector3 startPos = Vector3.zero;
            baseTarget = Vector3.zero;
            offset = Vector3.zero;

            switch (orientation)
            {
                case ColumnOrientation.Vertical:
                {
                    float randomX = Random.Range(_minX, _maxX);
                    startPos = new Vector3(randomX, _spawnY, 0);
                    float groundY = Random.Range(_lowerBound, _adjustedUpperBound);
                    baseTarget = new Vector3(randomX, groundY, 0);
                    offset = new Vector3(0, _circleDiameter, 0);
                    break;
                }
                case ColumnOrientation.Horizontal:
                {
                    float randomY = Random.Range(_minY, _maxY);
                    startPos = new Vector3(_maxX + _circleDiameter, randomY, 0);
                    float groundX = Random.Range(_minX, _maxX - _circleDiameter);
                    baseTarget = new Vector3(groundX, randomY, 0);
                    offset = new Vector3(-_circleDiameter, 0, 0);
                    break;
                }

                case ColumnOrientation.DiagonalLeft:
                {
                    startPos = new Vector3(_maxX + _circleDiameter, _spawnY, 0);
                    float safeMinX = _minX + 2 * _circleDiameter;
                    float safeMaxX = _maxX - _circleDiameter;
                    float diagLeftX = Random.Range(safeMinX, safeMaxX);
                    float safeMinY = _minY + 2 * _circleDiameter;
                    float safeMaxY = _maxY;
                    float diagLeftY = Random.Range(safeMinY, safeMaxY);
                    baseTarget = new Vector3(diagLeftX, diagLeftY, 0);
                    offset = new Vector3(-_circleDiameter, -_circleDiameter, 0);
                    break;
                }
                case ColumnOrientation.DiagonalRight:
                {
                    startPos = new Vector3(_minX - _circleDiameter, _spawnY, 0);
                    float safeMinX = _minX;
                    float safeMaxX = _maxX - 2 * _circleDiameter;
                    float diagRightX = Random.Range(safeMinX, safeMaxX);
                    float safeMinY = _minY + 2 * _circleDiameter;
                    float safeMaxY = _maxY;
                    float diagRightY = Random.Range(safeMinY, safeMaxY);
                    baseTarget = new Vector3(diagRightX, diagRightY, 0);
                    offset = new Vector3(_circleDiameter, -_circleDiameter, 0);
                    break;
                }
            }

            return startPos;
        }

        private void StartAnimation()
        {
            int count = _spawnedCircles.Count;
            if (count == 0)
                return; 
            
            List<GameObject> particles = new List<GameObject>(count);
            for (int i = 0; i < count; i++)
            {
                particles.Add(SpawnVFX(particlePrefabPath, _spawnedCircles[i].transform.position, _spawnedCircles[i].transform.rotation));
            }
            
            ParticleSystem particleSystem = particles[0]?.GetComponent<ParticleSystem>();
            float particleDuration = (particleSystem != null) ? particleSystem.main.duration : 0f;
            
            Timer.Register(this, particleDuration, () =>
            {
                for (int i = 0; i < particles.Count; i++)
                {
                    DespawnGameObject(_spawnedCircles[i]);
                    DespawnGameObject(particles[i]);
                }
                _spawnedCircles.Clear();
                StartSpawn();
            });
        }
    }
}
