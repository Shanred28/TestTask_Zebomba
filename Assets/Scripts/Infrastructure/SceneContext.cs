using Infrastructure.ProjectContext;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Infrastructure
{
    public class SceneContext : LifetimeScope
    {
        protected override void Awake()
        {
            ProjectContextFactory.TryCreate();
            base.Awake();
        }
        
        protected override void Configure(IContainerBuilder builder)
        {
            RegisterLevelServices(builder);
            RegisterLevelStateMachine(builder);
        }

        protected virtual void RegisterLevelServices(IContainerBuilder builder)
        {
            
        }

        protected virtual void RegisterLevelStateMachine(IContainerBuilder builder)
        {
            
        }
    }
}
