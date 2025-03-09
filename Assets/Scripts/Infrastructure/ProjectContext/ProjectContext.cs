using VContainer;
using VContainer.Unity;

namespace Infrastructure.ProjectContext
{
    public class ProjectContext : LifetimeScope
    {
        public static bool Initialized => _instance != null;
        
        private static ProjectContext _instance;

        protected override void Awake()
        {
            base.Awake();
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
                return;
            }

            Destroy(gameObject);
        }
        
        protected override void Configure(IContainerBuilder builder)
        {
            RegisterGameServices(builder);
            RegisterGameStateMachine(builder);
        }

        protected virtual void RegisterGameServices(IContainerBuilder builder)
        {
        }
        
        protected virtual void RegisterGameStateMachine(IContainerBuilder builder)
        {
        }
    }
}
