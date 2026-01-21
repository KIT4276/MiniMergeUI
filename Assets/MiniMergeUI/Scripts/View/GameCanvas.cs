using UnityEngine;

namespace MiniMergeUI.View
{
    public class GameCanvas : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private RectTransform _gridRoot;
        [SerializeField] private Canvas _canvas;
        [SerializeField] private Cell[] _cells;

        public Canvas Canvas { get => _canvas; }
        public RectTransform RectTransform { get => _rectTransform; }
        public RectTransform GridRoot { get => _gridRoot; }
        public Cell[] Cells { get => _cells; }
    }
}
