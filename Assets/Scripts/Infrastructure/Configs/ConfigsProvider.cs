using System.Linq;
using UnityEngine;

namespace Infrastructure.Configs
{
    public class ConfigsProvider : IConfigsProvider
    {
        private const string ScenesConfigsPath = "Configs/Scene";
        
        private SceneConfig[] _configsSceneList;
        
        public void Load()
        {
            _configsSceneList = Resources.LoadAll<SceneConfig>(ScenesConfigsPath).ToArray();
        }

        public SceneConfig GetSceneConfig(string nameScene)
        {
            foreach (var sceneConfig in _configsSceneList)
            {
                if (nameScene == sceneConfig.sceneName)
                {
                    return  sceneConfig;
                }
            }
            
            return null;
        }
    }
}
