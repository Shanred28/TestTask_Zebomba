using SO;
using UnityEngine;

namespace GameEntity
{
    public class TowerLayout
    {
        public readonly float ZoneWidth;
        public readonly float ZoneHeight;
        public readonly float ZoneCenterY;
        public readonly float[] ColumnCenters;
        public readonly float LeftWallThickness;
        public readonly float InternalWallThickness;
        public readonly float RightWallThickness;
        
        public readonly float ScreenWidth;
        public readonly float XLeft;
        public readonly float XRight;

        public TowerLayout(Camera cam, TowerLayoutConfigSO  config)
        {
            float halfHeight = cam.orthographicSize;
            float halfWidth = cam.orthographicSize * cam.aspect;
            XLeft = cam.transform.position.x - halfWidth;
            XRight = cam.transform.position.x + halfWidth;
            ScreenWidth = XRight - XLeft;
            
            LeftWallThickness = config.wallThickness;
            RightWallThickness = config.wallThickness;
            
            InternalWallThickness = config.wallThickness * config.towerSizeMultiplier;
            
            float zonesAvailableWidth = ScreenWidth - (LeftWallThickness + RightWallThickness + 2f * InternalWallThickness);
            ZoneWidth = zonesAvailableWidth / 3f;
            
            ZoneHeight = 2f * cam.orthographicSize * config.zoneHeightPercent;
            float yBottom = cam.transform.position.y - cam.orthographicSize;
            ZoneCenterY = yBottom + ZoneHeight / 2f;
            
            ColumnCenters = new float[3];
            ColumnCenters[0] = XLeft + LeftWallThickness + (ZoneWidth / 2f);
            ColumnCenters[1] = XLeft + LeftWallThickness + ZoneWidth + InternalWallThickness + (ZoneWidth / 2f);
            ColumnCenters[2] = XLeft + LeftWallThickness + 2f * ZoneWidth + 2f * InternalWallThickness + (ZoneWidth / 2f);
        }
    }
}
