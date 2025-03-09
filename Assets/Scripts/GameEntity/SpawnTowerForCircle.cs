using SO;
using UnityEngine;

namespace GameEntity
{
    public class SpawnTowerForCircle : MonoBehaviour
    {
        private const int MaxRows = 3;
        
        [Header("Настройки зоны")]
        public GameObject towerZonePrefab;

        [Header("Настройки стенок и башен")] 
        public TowerLayoutConfigSO  layoutConfig;

        [Header("Настройки боковых стен")]
        public GameObject wallPrefab;

        void Start()
        {
            Camera cam = Camera.main;
            
            TowerLayout layout = new TowerLayout(cam, layoutConfig);
            
            SpawnZoneTowerForCircle(layout);
            
            var yBottom = DrawingSideWalls(cam, out var sideWallHeight, out var sideWallCenterY);
            
            LeftSideWall(layout, sideWallCenterY, sideWallHeight);
            
            RightSideWall(layout, sideWallCenterY, sideWallHeight);
            
            InternalWallsZones(layout, cam, yBottom);
        }

        private void InternalWallsZones(TowerLayout layout, Camera cam, float yBottom)
        {
            float internalWall1CenterX = layout.XLeft + layout.LeftWallThickness + layout.ZoneWidth +
                                         (layout.InternalWallThickness / 2f);
            Vector3 internalWall1Pos = new Vector3(internalWall1CenterX, layout.ZoneCenterY, 0);
            GameObject internalWall1 = Instantiate(wallPrefab, internalWall1Pos, Quaternion.identity);
            internalWall1.transform.localScale = new Vector3(layout.InternalWallThickness, layout.ZoneHeight, 1);
            internalWall1.name = "InternalWall_1";

            // Вторая внутренняя стена: между зоной 1 и зоной 2
            float internalWall2CenterX = layout.XLeft + layout.LeftWallThickness + 2f * layout.ZoneWidth +
                                         layout.InternalWallThickness + (layout.InternalWallThickness / 2f);
            Vector3 internalWall2Pos = new Vector3(internalWall2CenterX, layout.ZoneCenterY, 0);
            GameObject internalWall2 = Instantiate(wallPrefab, internalWall2Pos, Quaternion.identity);
            internalWall2.transform.localScale = new Vector3(layout.InternalWallThickness, layout.ZoneHeight, 1);
            internalWall2.name = "InternalWall_2";

            // Спавн нижней стены – на всю ширину экрана, под зоной
            Vector3 bottomWallPos =
                new Vector3(cam.transform.position.x, yBottom - (layoutConfig.wallThickness / 2f), 0);
            GameObject bottomWall = Instantiate(wallPrefab, bottomWallPos, Quaternion.identity);
            bottomWall.transform.localScale = new Vector3(layout.ScreenWidth, layoutConfig.wallThickness, 1);
            bottomWall.name = "BottomWall";
        }

        private void RightSideWall(TowerLayout layout, float sideWallCenterY, float sideWallHeight)
        {
            Vector3 rightWallPos = new Vector3(layout.XRight - (layout.RightWallThickness / 2f), sideWallCenterY, 0);
            GameObject rightWall = Instantiate(wallPrefab, rightWallPos, Quaternion.identity);
            rightWall.transform.localScale = new Vector3(layout.RightWallThickness, sideWallHeight, 1);
            rightWall.name = "RightLateralWall";
        }

        private void LeftSideWall(TowerLayout layout, float sideWallCenterY, float sideWallHeight)
        {
            Vector3 leftWallPos = new Vector3(layout.XLeft + (layout.LeftWallThickness / 2f), sideWallCenterY, 0);
            GameObject leftWall = Instantiate(wallPrefab, leftWallPos, Quaternion.identity);
            leftWall.transform.localScale = new Vector3(layout.LeftWallThickness, sideWallHeight, 1);
            leftWall.name = "LeftLateralWall";
        }

        private float DrawingSideWalls(Camera cam, out float sideWallHeight, out float sideWallCenterY)
        {
            float screenHeight = 2f * cam.orthographicSize;
            float yBottom = cam.transform.position.y - cam.orthographicSize;
            sideWallHeight = screenHeight * layoutConfig.sideWallHeightPercent;
            sideWallCenterY = yBottom + sideWallHeight / 2f;
            return yBottom;
        }

        private void SpawnZoneTowerForCircle(TowerLayout layout)
        {
            for (int i = 0; i < MaxRows; i++)
            {
                Vector3 zonePos = new Vector3(layout.ColumnCenters[i], layout.ZoneCenterY, 0);
                GameObject zoneObj = Instantiate(towerZonePrefab, zonePos, Quaternion.identity);
                zoneObj.transform.localScale = new Vector3(layout.ZoneWidth, layout.ZoneHeight, 1);
                zoneObj.name = "LowerZone_" + i;
            }
        }
    }
}
