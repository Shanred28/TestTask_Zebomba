using UnityEngine;

namespace Infrastructure.ProjectContext
{
    public class ProjectContextFactory : MonoBehaviour
    {
        private const string ProjectContextResourcePath = "Prefabs/ProjectContext";
        
        public static void TryCreate()
        {
            if(ProjectContext.Initialized) return;
            
            ProjectContext prefab = TryGetPrefab();
            
            if (prefab != null)
            {
                Instantiate(prefab);
            }
        }

        private static ProjectContext TryGetPrefab()
        {
            var prefabs = Resources.LoadAll<ProjectContext>(ProjectContextResourcePath);
            
            if (prefabs.Length > 0)
            {
                return prefabs[0];
            }
            
            return null;
        }
    }
}
