using Common.Timer.Source;
using GameEntity.Circle;
using GameEntity.Grid;
using GameEntity.Spawners;
using UnityEngine;

namespace GameEntity
{
    public class PendulumCircleSpawner : SpawnerBase
    {
        public static PendulumCircleSpawner Instance { get; private set; }

        [SerializeField] private GameObject circlePrefab;
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private float spawnDelay = 0.5f;
        [SerializeField] private Pendulum circlePendulum;
        [SerializeField] private Transform pointRotate;
        [SerializeField] private CircleGridManager circleGridManager;

        private GameObject _currentCircle;
        private InputController _inputController;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        protected override void Initialize()
        {
            SpawnNewCircle();
        }

        public void SpawnNewCircle()
        {
            Timer.Register(this, spawnDelay, () => { SpawnCircle(); });
        }

        private void SpawnCircle()
        {
            _currentCircle = SpawnGameObject(spawnPoint.position, spawnPoint.rotation);
            _currentCircle.transform.localPosition = Vector3.zero;
            _currentCircle.transform.localRotation = Quaternion.identity;
            _currentCircle.GetComponent<CircleDropper>().Initialize(circlePendulum, pointRotate, circleGridManager);
        }
    }
}