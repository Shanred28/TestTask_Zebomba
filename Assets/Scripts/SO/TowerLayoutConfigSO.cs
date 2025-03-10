using UnityEngine;

namespace SO
{
    [CreateAssetMenu(fileName = "TowerLayoutConfig", menuName = "Tower/TowerLayoutConfig", order = 1)]
    public class TowerLayoutConfigSO : ScriptableObject
    {
        [Tooltip("Процент высоты экрана")]
        [Range(0f, 1f)]
        public float zoneHeightPercent = 0.3f;

        [Tooltip("Базовая толщина стены для боковых стен и для расчёта внутренних")]
        public float wallThickness = 0.1f;

        [Tooltip("Множитель для внутренних стен")]
        public float towerSizeMultiplier = 1.25f;

        [Tooltip("Процент высоты экрана для боковых стен")]
        [Range(0f, 1f)]
        public float sideWallHeightPercent = 0.5f;

        public float circleDiameter = 1f;
        public int towerCount = 3;
    }
}
