using Infrastructure.Services;

namespace Infrastructure.Configs
{
    public interface IConfigsProvider : IService
    {
        void Load();
        public SceneConfig GetSceneConfig(string nameScene);
    }
}
