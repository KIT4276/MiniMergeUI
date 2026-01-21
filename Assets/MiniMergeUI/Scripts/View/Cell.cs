using UnityEngine;

namespace MiniMergeUI.View
{
    public  class Cell : MonoBehaviour
    {
        public RectTransform RectTransform { get; private set; }


        private void Awake()
        {
            RectTransform = GetComponent<RectTransform>();
        }

    }
}