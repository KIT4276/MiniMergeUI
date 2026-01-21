using UnityEngine;

namespace MiniMergeUI.Tests
{
    public  class Cell : MonoBehaviour
    {
       private RectTransform _rectTransform;
        public RectTransform RectTransform { get => _rectTransform; }

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }
    }
}