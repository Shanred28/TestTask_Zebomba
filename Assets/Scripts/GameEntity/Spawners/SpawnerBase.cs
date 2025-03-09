using Infrastructure.Services.Factory;
using UnityEngine;
using VContainer;

namespace GameEntity.Spawners
{
    public abstract class SpawnerBase : MonoBehaviour
    {
        [Header("Общий префаб для спауна")]
        [SerializeField] private string prefabPath;
        
        [SerializeField] private Transform spawnParent;

        private IGameFactory _gameFactory;
    
        [Inject]
        public void Construct(IGameFactory factory)
        {
            _gameFactory = factory;
            Initialize();
        }

        protected virtual void Initialize()
        {
            
        }

        protected GameObject SpawnGameObject(Vector3 position, Quaternion rotation)
        {
            if (_gameFactory == null)
            {
                Debug.LogError("GameFactory не инициализирован!");
                return null;
            }
            return _gameFactory.CreateGameObject(
                prefabPath,
                spawnParent != null ? spawnParent : transform,
                position,
                rotation
            );
        }
        
        protected GameObject SpawnVFX(string patchParticle, Vector3 position, Quaternion rotation)
        {
            if (_gameFactory == null)
            {
                Debug.LogError("GameFactory не инициализирован!");
                return null;
            }
            return _gameFactory.CreateGameObject(
                patchParticle,
                spawnParent != null ? spawnParent : transform,
                position,
                rotation
            );
        }

        protected void DespawnGameObject(GameObject gameObject)
        {
            _gameFactory.Despawn(gameObject);
        }
    }
}
