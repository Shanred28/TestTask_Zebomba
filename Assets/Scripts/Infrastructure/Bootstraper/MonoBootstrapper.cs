using UnityEngine;

namespace Infrastructure.Bootstraper
{
    public abstract class MonoBootstrapper : MonoBehaviour
    {
        public abstract void OnBindResolved();
    }
}
