using UnityEngine;

namespace Infrastructure.Configs
{
    public enum SceneTypes
    {
        MainMenu,
        MainGameScene
    }
    [CreateAssetMenu(fileName = "SceneConfig", menuName = "Configs/SceneConfig", order = 2)]
    public class SceneConfig : ScriptableObject
    {
        public string sceneName;
        public SceneTypes sceneType;
        public string[] patchPrefabsInSceneCreate;
        public string patchMainUI;
    }
}
