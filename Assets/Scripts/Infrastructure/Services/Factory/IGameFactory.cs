using System.Threading.Tasks;
using UnityEngine;

namespace Infrastructure.Services.Factory
{
    public interface IGameFactory : IService
    {
        T Create<T>() where T : class;
        
        GameObject CreateGameObject(string assetPath, Transform parent = null, Vector3? position = null,
            Quaternion? rotation = null);

        T CreateGameObjectWithComponent<T>(string assetPath, Transform parent = null, Vector3? position = null,
            Quaternion? rotation = null) where T : Component;

        Task<T> CreateGameObjectWithComponentAsync<T>(string assetPath, Transform parent = null,
            Vector3? position = null, Quaternion? rotation = null) where T : Component;

        void ReleaseAsset(GameObject asset);
        void Despawn(GameObject gameObject);
    }
}
