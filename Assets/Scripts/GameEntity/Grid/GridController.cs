using DG.Tweening;
using GameEntity.Circle;
using UnityEngine;

namespace GameEntity.Grid
{
    public class GridController
    {
        private readonly int _gridSize;
        private readonly float _circleDiameter;
        private readonly float _groundY;
        private readonly float[] _columnCenters;
        private readonly int[] _columnCounts;
        
        private readonly CircleDropper[,] _grid;

        public GridController(int gridSize, float circleDiameter, float groundY, float[] columnCenters)
        {
            _gridSize = gridSize;
            _circleDiameter = circleDiameter;
            _groundY = groundY;
            _columnCenters = columnCenters;

            _columnCounts = new int[_gridSize];
            _grid = new CircleDropper[_gridSize, _gridSize];
        }
        
        public Vector3 GetTargetPosition(GameObject circle, out int chosenColumn, out int row)
        {
            float circleX = circle.transform.position.x;
            chosenColumn = 0;
            float minDistance = Mathf.Abs(circleX - _columnCenters[0]);
            
            for (int i = 1; i < _gridSize; i++)
            {
                float distance = Mathf.Abs(circleX - _columnCenters[i]);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    chosenColumn = i;
                }
            }
            
            if (_columnCounts[chosenColumn] >= _gridSize)
            {
                int alternative = -1;
                float bestDistance = float.MaxValue;
                for (int i = 0; i < _gridSize; i++)
                {
                    if (_columnCounts[i] >= _gridSize) continue;
                    
                    float d = Mathf.Abs(circleX - _columnCenters[i]);
                    if (d < bestDistance)
                    {
                        bestDistance = d;
                        alternative = i;
                    }
                }

                if (alternative != -1)
                {
                    chosenColumn = alternative;
                }
                else
                {
                    chosenColumn = -1;
                    row = -1;
                    return Vector3.zero;
                }
            }

            row = _columnCounts[chosenColumn];
            float targetX = _columnCenters[chosenColumn];
            float targetY = _groundY + row * _circleDiameter;
            return new Vector3(targetX, targetY, 0);
        }
        
        public void PlaceCircle(int col, int row, CircleDropper circle)
        {
            _grid[col, row] = circle;
            _columnCounts[col]++;
        }
        
        public void RemoveCircle(CircleDropper circle)
        {
            for (int col = 0; col < _gridSize; col++)
            {
                for (int row = 0; row < _gridSize; row++)
                {
                    if (_grid[col, row] == circle)
                    {
                        _grid[col, row] = null;
                        _columnCounts[col]--;
                        return;
                    }
                }
            }
        }
        
        public Sequence CollapseColumnsSequence()
        {
            Sequence sequence = DOTween.Sequence();
            for (int col = 0; col < _gridSize; col++)
            {
                int targetRow = 0;
                for (int row = 0; row < _gridSize; row++)
                {
                    CircleDropper circle = _grid[col, row];
                    if (circle)
                    {
                        if (row != targetRow)
                        {
                            _grid[col, targetRow] = circle;
                            _grid[col, row] = null;
                            Vector3 targetPos = new Vector3(
                                _columnCenters[col],
                                _groundY + targetRow * _circleDiameter,
                                0);
                            sequence.Join(circle.transform.DOMove(targetPos, 1f).SetEase(Ease.Linear));
                        }

                        targetRow++;
                    }
                }

                _columnCounts[col] = targetRow;
            }

            return sequence;
        }
        
        public CircleDropper[,] GetGrid() => _grid;
    }
}
