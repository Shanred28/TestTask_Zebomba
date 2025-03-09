using System.Collections.Generic;
using GameEntity.Circle;
using UnityEngine;

namespace GameEntity.Grid
{
    public class CheckMatchesLine 
    {
        private class LineMatch
        {
            public readonly CircleColorType Color;
            public readonly List<CircleDropper> Circles;

            public LineMatch(CircleColorType color, List<CircleDropper> circles)
            {
                Color = color;
                Circles = circles;
            }
        }
        
        private readonly CircleGridManager _circleGridManager;
        private readonly CircleDropper[,] _grid;

        private readonly int _rows;
        private readonly int _cols; 

        public CheckMatchesLine(CircleGridManager circleGridManager,CircleDropper[,] grid,int rows,int cols)
        {
            _circleGridManager = circleGridManager;
            _grid = grid;
            _rows = rows;
            _cols = cols;
        }

        public void CheckMatches()
        {
            List<LineMatch> lineMatches = new List<LineMatch>();
            int totalScore = 0;
                
            CheckHorizontalMatches(lineMatches);
            CheckVerticalMatches(lineMatches);
            CheckDiagonalMatches(lineMatches);

            if (lineMatches.Count > 0)
            {
                foreach (LineMatch match in lineMatches)
                {
                    totalScore += GetScoreForColor(match.Color);
                }
                
                if (lineMatches.Count >= 2)
                {
                    totalScore += 50;
                }
                
                _circleGridManager.AddScore(totalScore,OverallCenter(lineMatches));
                Debug.Log($"Начислено {totalScore} очков за {lineMatches.Count} линий.");

                HashSet<CircleDropper> circlesToRemove = new HashSet<CircleDropper>();
                foreach (LineMatch match in lineMatches)
                {
                    foreach (CircleDropper circ in match.Circles)
                    {
                        circlesToRemove.Add(circ);
                    }
                }

                foreach (CircleDropper circ in circlesToRemove)
                {
                    _circleGridManager.RemoveCircle(circ);
                }
                _circleGridManager.StartCoroutine(_circleGridManager.CollapseColumns());
            }
        }

        private static Vector3 OverallCenter(List<LineMatch> lineMatches)
        {
            Vector3 overallCenter = Vector3.zero;
            int count = 0;
            foreach (LineMatch match in lineMatches)
            {
                Vector3 center = Vector3.zero;
                foreach (CircleDropper circ in match.Circles)
                {
                    center += circ.transform.position;
                }
                center /= match.Circles.Count;
                overallCenter += center;
                count++;
            }
            overallCenter /= count;
            return overallCenter;
        }

        private int GetScoreForColor(CircleColorType color)
        {
            switch (color)
            {
                case CircleColorType.Red: return 30;
                case CircleColorType.Yellow: return 20;
                case CircleColorType.Green: return 10;
                default: return 0;
            }
        }

        private void CheckDiagonalMatches(List<LineMatch> lineMatches)
        {
            Vector2Int[] diag1 = {
                new(0, 0),
                new(1, 1),
                new(2, 2)
            };
            CheckLine(diag1, lineMatches);
            
            Vector2Int[] diag2 = {
                new(2, 0),
                new(1, 1),
                new(0, 2)
            };
            CheckLine(diag2, lineMatches);
        }

        private void CheckVerticalMatches(List<LineMatch> lineMatches)
        {
            for (int col = 0; col < _cols; col++)
            {
                Vector2Int[] indices = {
                    new(col, 0),
                    new(col, 1),
                    new(col, 2)
                };
                CheckLine(indices, lineMatches);
            }
        }

        private void CheckHorizontalMatches(List<LineMatch> lineMatches)
        {
            for (int row = 0; row < _rows; row++)
            {
                Vector2Int[] indices = {
                    new(0, row),
                    new(1, row),
                    new(2, row)
                };
                CheckLine(indices, lineMatches);
            }
        }

        private void CheckLine(Vector2Int[] indices, List<LineMatch> lineMatches)
        {
            CircleDropper first = _grid[indices[0].x, indices[0].y];
            if (first == null)
                return;

            CircleColorType color = first.CircleColorType;
            for (int i = 1; i < indices.Length; i++)
            {
                int col = indices[i].x;
                int row = indices[i].y;
                CircleDropper current = _grid[col, row];
                if (current == null || current.CircleColorType != color)
                    return;
            }
            
            List<CircleDropper> circles = new List<CircleDropper>();
            for (int i = 0; i < indices.Length; i++)
            {
                int col = indices[i].x;
                int row = indices[i].y;
                circles.Add(_grid[col, row]);
            }
            lineMatches.Add(new LineMatch(color, circles));
        }
    }
}
