using System;

namespace Infrastructure.Services.GameStateMachine
{
    public interface ISceneLoader
    {
        public void Load(string sceneName, Action onLoaded = null);
    }
}
