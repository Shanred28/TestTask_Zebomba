using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Infrastructure.Services.AssetManager
{
    public class AssetProvider : IAssetProvider
    {
        private readonly Dictionary<string, Object> _prefabCache = new();
        
        public T GetPrefab<T>(string prefabPath) where T : Object
        {
            if (string.IsNullOrEmpty(prefabPath))
                throw new ArgumentException("Путь к префабу не может быть пустым", nameof(prefabPath));

            if (_prefabCache.TryGetValue(prefabPath, out Object cached))
            {
                T asset = cached as T;
                if (asset != null)
                    return asset;
                else
                    Debug.LogWarning($"Найден кэш по пути {prefabPath}, но его тип не соответствует ожидаемому типу {typeof(T).Name}.");
            }

            T loadedAsset = Resources.Load<T>(prefabPath);
            if (loadedAsset == null)
            {
                Debug.LogError($"Префаб не найден по пути: {prefabPath}");
                throw new Exception($"Префаб не найден по пути: {prefabPath}");
            }

            _prefabCache[prefabPath] = loadedAsset;
            return loadedAsset;
        }

        public T LoadAsset<T>(string assetPath) where T : Object
        {
            T prefab = GetPrefab<T>(assetPath);
            return Object.Instantiate(prefab);
        }

        public async Task<T> LoadAssetAsync<T>(string assetPath) where T : Object
        {
            if (string.IsNullOrEmpty(assetPath))
                throw new ArgumentException("Путь к ассету не может быть пустым", nameof(assetPath));

            if (_prefabCache.TryGetValue(assetPath, out UnityEngine.Object cached))
            {
                T asset = cached as T;
                if (asset != null)
                    return asset;
                else
                    Debug.LogWarning($"Найден кэш по пути {assetPath}, но его тип не соответствует ожидаемому типу {typeof(T).Name}.");
            }

            ResourceRequest request = Resources.LoadAsync<T>(assetPath);
            while (!request.isDone)
            {
                await Task.Yield();
            }

            T loadedAsset = request.asset as T;
            if (loadedAsset == null)
            {
                Debug.LogError($"Не удалось загрузить ассет по пути: {assetPath}");
                throw new Exception($"Не удалось загрузить ассет по пути: {assetPath}");
            }

            _prefabCache[assetPath] = loadedAsset;
            return loadedAsset;
        }

        public async Task<T> InstantiateAsync<T>(string assetPath) where T : Object
        {
            T asset = await LoadAssetAsync<T>(assetPath);
            return Object.Instantiate(asset);
        }

        public void ReleaseAsset(Object asset)
        {
            if (asset == null)
            {
                Debug.LogWarning("Попытка освободить null ассет.");
                return;
            }

            Resources.UnloadAsset(asset);
        }

        public void ClearCache()
        {
            _prefabCache.Clear();
        }
    }
}