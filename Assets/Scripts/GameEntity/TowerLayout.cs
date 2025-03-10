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
        
        private readonly Camera _camera;
        private readonly float _circleDiameter;
        private readonly int _gridSize;
        
        public TowerLayout(Camera cam, TowerLayoutConfigSO config)
        {
            _camera = cam;
            
            float halfWidth = cam.orthographicSize * cam.aspect;
            XLeft = cam.transform.position.x - halfWidth;
            XRight = cam.transform.position.x + halfWidth;
            ScreenWidth = XRight - XLeft;
            
            float baseWallThickness = config.wallThickness;
            LeftWallThickness = baseWallThickness;
            RightWallThickness = baseWallThickness;
            InternalWallThickness = config.wallThickness * config.towerSizeMultiplier;
            
            _circleDiameter = config.circleDiameter;
            ZoneWidth = _circleDiameter;
            
            _gridSize = config.towerCount;
            
            float towersContentWidth = _gridSize * ZoneWidth + (_gridSize - 1) * InternalWallThickness;
            
            float remainingMargin = ScreenWidth - towersContentWidth;
            float leftMargin = remainingMargin / 2f;
            float rightMargin = leftMargin;
            
            LeftWallThickness = leftMargin;
            RightWallThickness = rightMargin;
            
            ColumnCenters = new float[_gridSize];
            ColumnCenters[0] = XLeft + LeftWallThickness + ZoneWidth / 2f;
            for (int i = 1; i < _gridSize; i++)
            {
                ColumnCenters[i] = ColumnCenters[i - 1] + ZoneWidth + InternalWallThickness;
            }
            
            float halfHeight = cam.orthographicSize;
            float yBottom = cam.transform.position.y - halfHeight;
            ZoneHeight = 2f * halfHeight * config.zoneHeightPercent;
            ZoneCenterY = yBottom + ZoneHeight / 2f;
        }
        
        public TowerLayout(Camera camera, float circleDiameter, int gridSize)
        {
            _camera = camera;
            _circleDiameter = circleDiameter;
            _gridSize = gridSize;

            float halfWidth = _camera.orthographicSize * _camera.aspect;
            XLeft = _camera.transform.position.x - halfWidth;
            XRight = _camera.transform.position.x + halfWidth;
            ScreenWidth = XRight - XLeft;
            
            LeftWallThickness = 0f;
            RightWallThickness = 0f;
            InternalWallThickness = 0f;

            ZoneWidth = _circleDiameter;
            ZoneHeight = _camera.orthographicSize;
            float yBottom = _camera.transform.position.y - _camera.orthographicSize;
            ZoneCenterY = yBottom + ZoneHeight / 2f;
            
            ColumnCenters = new float[_gridSize];
            float totalWidth = (_gridSize - 1) * _circleDiameter;
            float cameraCenterX = _camera.transform.position.x;
            float leftCenter = cameraCenterX - totalWidth / 2f;
            for (int i = 0; i < _gridSize; i++)
            {
                ColumnCenters[i] = leftCenter + i * _circleDiameter;
            }
        }
    }
}
