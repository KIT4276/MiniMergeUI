using UnityEngine;
using DG.Tweening;
using Zenject;
using System;
using UnityEngine.UIElements;
using Zenject.Asteroids;

namespace MiniMergeUI.Tests
{
    public class Test : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private Canvas _canvas;
        [SerializeField] private Cell[] _cells;

        private bool _isDraging;

        private RectTransform _parentRt;
        private Camera _uiCam;

        private Vector2 _indent;

        private void Awake()//Todo  Init(RectTransform parentRt, Canvas canvas)
        {
            _parentRt = (RectTransform)_rectTransform.parent;
            _uiCam = _canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : _canvas.worldCamera;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
                StartDrag();

            if (Input.GetMouseButtonUp(0))
                StopDrag();

            if (_isDraging)
                ToDrag();
        }

        private void ToDrag()
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _parentRt, Input.mousePosition, _uiCam, out var pointerLocal);

            _rectTransform.anchoredPosition = pointerLocal + _indent;
        }


        private void StartDrag()
        {
            _isDraging = true;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _parentRt, Input.mousePosition, _uiCam, out var pointerLocal);

            _indent = _rectTransform.anchoredPosition - pointerLocal;
        }

        private void StopDrag()
        {
            _isDraging = false;

            var cell = GetNearest(_rectTransform.anchoredPosition);
            PositionTheChip(cell);
        }

        private Cell GetNearest(Vector2 anchoredPosition)
        {
            Cell best = null;
            float bestSqrDist = float.PositiveInfinity;

            foreach (var cell in _cells)
            {
                Vector3 cellWorldCenter = cell.RectTransform.TransformPoint(cell.RectTransform.rect.center);
                Vector2 cellLocalCenter = (Vector2)_parentRt.InverseTransformPoint(cellWorldCenter);

                float sqrDist = (anchoredPosition - cellLocalCenter).sqrMagnitude;

                if (sqrDist < bestSqrDist)
                {
                    bestSqrDist = sqrDist;
                    best = cell;
                }
            }

            return best;
        }

        private void PositionTheChip(Cell cell)
        {
            Vector3 cellWorldCenter = cell.RectTransform.TransformPoint(cell.RectTransform.rect.center);
            Vector2 cellLocalCenter = (Vector2)_parentRt.InverseTransformPoint(cellWorldCenter);

            _rectTransform.DOAnchorPos(cellLocalCenter, 0.15f);

            _rectTransform.anchoredPosition = cellLocalCenter;
        }

    }
}
