using System;
using UnityEngine;

namespace MiniMergeUI.Services
{
    public class DragHandler : MonoBehaviour
    {
        public event Action<Vector2> DragStarted;
        public event Action<Vector2> DragFinished;
        public event Action<Vector2> DragOngoing;

        private Vector3 _lastMousePosition;


        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
                StartDrag();

            if (Input.GetMouseButtonUp(0))
                StopDrag();

            DragIfMouseMoved();
        }

        private void StartDrag()
        {
            DragStarted?.Invoke(Input.mousePosition);
            _lastMousePosition = Input.mousePosition;
        }

        private void StopDrag()
        {
            DragFinished?.Invoke(Input.mousePosition);
        }

        private void DragIfMouseMoved()
        {
            if (Input.mousePosition != _lastMousePosition)
            {
                DragOngoing?.Invoke(Input.mousePosition);
                _lastMousePosition = Input.mousePosition;
            }
        }
    }
}