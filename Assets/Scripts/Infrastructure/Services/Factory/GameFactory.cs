using System;
using System.IO;
using System.Threading.Tasks;
using Infrastructure.Services.AssetManager;
using Lean.Pool;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Infrastructure.Services.Factory
{
    public class GameFactory : IGameFactory
    {
        private readonly IObjectResolver _container;
        private readonly IAssetProvider _assetProvider;
        
        public GameFactory(IAssetProvider assetProvider, IObjectResolver container)
        {
            _assetProvider = assetProvider;
            _container = container;
        }

        public T Create<T>() where T : class
        {
            try
            {
                return _container.Resolve<T>();
            }
            catch (Exception ex)
            {
                Debug.LogError($"Ошибка при разрешении зависимости типа {typeof(T).Name}: {ex}");
                throw;
            }
        }

        public async Task<GameObject> CreateGameObjectAsync(string assetPath, Transform parent = null,
            Vector3? position = null, Quaternion? rotation = null)
        {
            if (string.IsNullOrEmpty(assetPath))
                throw new ArgumentNullException(nameof(assetPath), "Путь к ассету не может быть пустым");

            GameObject prefab = await _assetProvider.LoadAssetAsync<GameObject>(assetPath);

            if (prefab == null)
            {
                Debug.LogError($"Префаб не найден по пути: {assetPath}");
                throw new FileNotFoundException($"Префаб не найден по пути: {assetPath}");
            }

            Vector3 pos = position ?? Vector3.zero;
            Quaternion rot = rotation ?? Quaternion.identity;
            GameObject instance = UnityEngine.Object.Instantiate(prefab, pos, rot, parent);
            Injection(instance);
            return instance;
        }

        public GameObject CreateGameObject(string assetPath, Transform parent = null, Vector3? position = null,
            Quaternion? rotation = null)
        {
            if (string.IsNullOrEmpty(assetPath))
                throw new ArgumentNullException(nameof(assetPath), "Путь к ассету не может быть пустым");

            GameObject prefab = _assetProvider.GetPrefab<GameObject>(assetPath);
            if (prefab == null)
            {
                Debug.LogError($"Префаб не найден по пути: {assetPath}");
                throw new FileNotFoundException($"Префаб не найден по пути: {assetPath}");
            }

            Vector3 pos = position ?? Vector3.zero;
            Quaternion rot = rotation ?? Quaternion.identity;
            GameObject instance = LeanPool.Spawn(prefab, pos, rot, parent);
            Injection(instance);
            return instance;
        }

        /// <summary>
        /// Создает игровой объект и добавляет к нему компонент указанного типа.
        /// Если компонент уже существует, возвращает существующий.
        /// </summary>
        public T CreateGameObjectWithComponent<T>(string assetPath, Transform parent = null, Vector3? position = null,
            Quaternion? rotation = null) where T : Component
        {
            GameObject instance = CreateGameObject(assetPath, parent, position, rotation);
            T component = instance.GetComponent<T>() ?? instance.AddComponent<T>();
            return component;
        }

        /// <summary>
        /// Асинхронно создает игровой объект и добавляет к нему компонент указанного типа.
        /// Если компонент уже существует, возвращает имеющийся.
        /// </summary>
        public async Task<T> CreateGameObjectWithComponentAsync<T>(string assetPath, Transform parent = null,
            Vector3? position = null, Quaternion? rotation = null) where T : Component
        {
            GameObject instance = await CreateGameObjectAsync(assetPath, parent, position, rotation);
            T component = instance.GetComponent<T>() ?? instance.AddComponent<T>();
            return component;
        }

        public void ReleaseAsset(GameObject asset)
        {
            if (asset == null)
                return;

            _assetProvider.ReleaseAsset(asset);
        }

        public void Despawn(GameObject gameObject)
        {
            LeanPool.Despawn(gameObject);
        }

        private void Injection(GameObject gameObject)
        {
            _container.InjectGameObject(gameObject);
        }
    }
}