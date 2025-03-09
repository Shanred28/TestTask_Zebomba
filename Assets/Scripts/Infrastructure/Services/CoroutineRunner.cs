using System.Collections;
using UnityEngine;

namespace Infrastructure.Services
{
    public class CoroutineRunner : ICoroutineRunner
    {
        
        private readonly MonoCoroutineRunner _monoCoroutineRunner;
        
        public CoroutineRunner()
        {
            GameObject coroutineRunnerGameObject = new GameObject("Coroutine Runner");
            _monoCoroutineRunner = coroutineRunnerGameObject.AddComponent<MonoCoroutineRunner>();
        
            GameObject.DontDestroyOnLoad(coroutineRunnerGameObject);
        }
        
        public Coroutine StartCoroutine(IEnumerator coroutine)
        {
            return _monoCoroutineRunner.StartCoroutine(coroutine);
        }
    }
}
