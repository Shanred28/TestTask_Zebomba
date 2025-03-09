using System.Threading.Tasks;
using UnityEngine;

namespace Infrastructure.Services.AssetManager
{
    public interface IAssetProvider : IService
    {
        T GetPrefab<T>(string prefabPath) where T : Object;
        T LoadAsset<T>(string prefabPath) where T : Object;
        
        Task<T> LoadAssetAsync<T>(string assetPath) where T : Object;
        Task<T> InstantiateAsync<T>(string prefabPath) where T : Object;
        
        void ReleaseAsset(Object asset);

        void ClearCache();
    }
}
