using DG.Tweening;
using UnityEngine;

namespace MiniMergeUI.View
{
    public class Chip : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectTransform;

        private RectTransform _space;  // где считаем local (обычно GameCanvas.RectTransform или DragRoot)
        private Camera _uiCam;
        private Vector2 _indent;

        public RectTransform RectTransform =>
            _rectTransform;

        public void Init(RectTransform space, Canvas canvas)
        {
            _space = space;
            _uiCam = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera;
            _rectTransform.SetParent(space, false);
        }

        public void BeginDrag(Vector2 screenPos)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_space, screenPos, _uiCam, out var local);
            _indent = _rectTransform.anchoredPosition - local;
        }

        public void DragTo(Vector2 screenPos)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_space, screenPos, _uiCam, out var local);
            _rectTransform.anchoredPosition = local + _indent;
        }

        public void EndDrag()
        {
        }

        public void SnapTo(Cell cell)
        {
            var world = cell.RectTransform.TransformPoint(cell.RectTransform.rect.center);
            var local = (Vector2)_space.InverseTransformPoint(world);
            _rectTransform.DOKill();
            _rectTransform.DOAnchorPos(local, 0.15f);
            //_rectTransform.anchoredPosition = local;
        }
    }
}
