using System;
using System.Collections;
using Infrastructure.Services.GameStateMachine;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Infrastructure.Services
{
    public class SceneLoader : ISceneLoader
    {
        private readonly ICoroutineRunner _coroutineRunner;
        
        public SceneLoader(ICoroutineRunner coroutineRunner)
        {
            _coroutineRunner = coroutineRunner;
        }
        public void Load(string sceneName, Action onLoaded = null)
        {
            if (SceneManager.GetActiveScene().name == sceneName)
            {
                SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
            }
            
            _coroutineRunner.StartCoroutine(LoadAsync(sceneName, onLoaded));
        }
        
        private IEnumerator LoadAsync(string sceneName, Action onLoaded)
        {
            if (SceneManager.GetActiveScene().name == sceneName)
            {
                yield return null;
                onLoaded?.Invoke();
                yield break;
            }
        
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);

            while (asyncOperation.isDone == false)
            {
                yield return null;
            }
        
            onLoaded?.Invoke();
        }
    }
}
