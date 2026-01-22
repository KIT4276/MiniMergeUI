using UnityEngine;
using UnityEngine.UI;

namespace MiniMergeUI.View
{
    public class Chip : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private Image _icon;
        [SerializeField] private VisualEffects _visualEffects;

        private RectTransform _space; 
        private Camera _uiCam;
        private Vector2 _indent;

        public RectTransform RectTransform =>
            _rectTransform;

        public ChipType Type { get; private set; }
        public int Level { get; private set; }

        public void Init(RectTransform space, Canvas canvas)
        {
            _space = space;
            _uiCam = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera;
            _rectTransform.SetParent(space, false);
        }

        public void SetIdentity(ChipType type, int level,  Sprite sprite)
        {
            Type = type;
            Level = level;
            if (_icon != null) 
                _icon.sprite = sprite;
            if(Level == 0)
                _icon.color = Color.white;
            else if (Level == 1)
                _icon.color = Color.yellow;
            else if (Level == 2)
                _icon.color = Color.red;
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

        public void EndDrag() => 
            _visualEffects.PlayEndDragPop();

        public void SnapTo(Cell cell)
        {
            var world = cell.RectTransform.TransformPoint(cell.RectTransform.rect.center);
            var local = (Vector2)_space.InverseTransformPoint(world);
            _rectTransform.anchoredPosition = local;
        }

        public void Merge()
            => _visualEffects.PlayMergePop();
    }

    public enum ChipType
    {
        Planet,
        SpaceShip,
        SpaceStation,
        Spiral,
        Telescope,
        Warp
    }
}
