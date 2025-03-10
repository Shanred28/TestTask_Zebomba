using System;
using System.Collections;
using Common.Timer.Source;
using DG.Tweening;
using GameEntity.Circle;
using Lean.Pool;
using SO;
using UI;
using UnityEngine;

namespace GameEntity.Grid
{
    public class CircleGridManager : MonoBehaviour
    {
        public event Action<EndGameAction> OnEndedGame;

        private const int GridSize = 3;

        [SerializeField] private float circleDiameter = 1f;
        [Header("Настройки башен и стен")] 
        [SerializeField] private  TowerLayoutConfigSO layoutConfig;
        [SerializeField] private ScoreManager scoreManager;
        [SerializeField] private ParticleSystem particleSystemForCircle;

        private GridController _gridController;
        private CheckMatchesLine _checkMatchesLine;
        private Camera _camera;
        private float _groundY;
        
        void Start()
        {
            _camera = Camera.main;
            TowerLayout layout = new TowerLayout(_camera, layoutConfig);
            float[] columnCenters = layout.ColumnCenters;
            _groundY = _camera.transform.position.y - _camera.orthographicSize + (circleDiameter / 2f);
            
            _gridController = new GridController(GridSize, circleDiameter, _groundY, columnCenters);
            _checkMatchesLine = new CheckMatchesLine(this, _gridController.GetGrid(), GridSize, GridSize);
        }

        public void AttachCircle(CircleDropper circle)
        {
            Vector3 targetPos = _gridController.GetTargetPosition(circle.gameObject, out int chosenColumn, out int row);
            
            if (chosenColumn == -1)
            {
                if (!_checkMatchesLine.HasMatches())
                {
                    OnEndedGame?.Invoke(EndGameAction.Result);
                }
                else
                {
                    StartCoroutine(CollapseColumns());
                    PendulumCircleSpawner.Instance.SpawnNewCircle();
                }
                return;
            }

            SnapToPositionTween(circle, targetPos, 1f, chosenColumn, row);
        }

        public void AddScore(int points,Vector3 targetPos)
        {
            scoreManager.AddScore(points,targetPos);
        }

        public void RemoveCircle(CircleDropper circ)
        {
            ParticleSystem part = LeanPool.Spawn(particleSystemForCircle,circ.transform.position, Quaternion.identity);
            particleSystemForCircle.Play();
            Timer.Register(this, part.main.duration, () =>
            {
                LeanPool.Despawn(part);
            });
            _gridController.RemoveCircle(circ);
            LeanPool.Despawn(circ.gameObject);
        }

        public IEnumerator CollapseColumns()
        {
            Sequence sequence = _gridController.CollapseColumnsSequence();

            if (sequence.Duration() > 0)
            {
                sequence.OnComplete(() =>
                {
                    _checkMatchesLine.CheckMatches();
                });
                yield return sequence.WaitForCompletion();
            }
            else
            {
                _checkMatchesLine.CheckMatches();
                yield return null;
            }
        }

        private void SnapToPositionTween(CircleDropper circle, Vector3 targetPos, float duration, int col, int row)
        {
            Rigidbody2D rb = circle.RbRigidbody2D;
            rb.linearVelocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Kinematic;
                
            circle.transform.DOMove(targetPos, duration)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    _gridController.PlaceCircle(col, row, circle);
                    _checkMatchesLine.CheckMatches();
                   
                   if (IsGridFull() && !_checkMatchesLine.HasMatches())
                   {
                       OnEndedGame?.Invoke(EndGameAction.Result);
                       return;
                   }
                   PendulumCircleSpawner.Instance.SpawnNewCircle();
                });
        }

        private bool IsGridFull()
        {
            CircleDropper[,] grid = _gridController.GetGrid();
            int cols = grid.GetLength(0);
            int rows = grid.GetLength(1);
            for (int col = 0; col < cols; col++)
            {
                for (int row = 0; row < rows; row++)
                {
                    if (grid[col, row] == null)
                        return false;
                }
            }
            return true;
        }
    }
}