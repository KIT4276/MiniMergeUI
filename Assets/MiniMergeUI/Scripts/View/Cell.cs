using UnityEngine;

namespace MiniMergeUI.View
{
    public  class Cell : MonoBehaviour
    {
       private RectTransform _rectTransform;
        public RectTransform RectTransform { get => _rectTransform; }

        public bool IsOccupied { get; private set; }

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            IsOccupied = false;
        }

        public void Occupy()=>
            IsOccupied = true;

        public void Release() =>
            IsOccupied = false;
    }
}