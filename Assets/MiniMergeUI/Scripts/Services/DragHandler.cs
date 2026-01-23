using System;
using UnityEngine;

namespace MiniMergeUI.Services
{
    public class DragHandler : MonoBehaviour
    {
        public event Action<Vector2> DragStarted;
        public event Action<Vector2> DragOngoing;
        public event Action<Vector2> DragFinished;

        [SerializeField] private bool _sendOngoingEveryFrame = false;

        private bool _isDragging;
        private Vector2 _lastPos;
        private bool _hasLastPos;

        private int _activeTouchId = -1;

        private void Update()
        {
            GetPointerState(out var pos, out var down, out var held, out var up);

            if (down && !_isDragging)
                Begin(pos);

            if (!_isDragging)
                return;

            if (up)
            {
                End(pos);
                return;
            }

            if (!held && !down)
            {
                End(pos);
                return;
            }

            if (_sendOngoingEveryFrame || !_hasLastPos || (pos - _lastPos).sqrMagnitude > 0.01f)
            {
                DragOngoing?.Invoke(pos);
                _lastPos = pos;
                _hasLastPos = true;
            }
        }

        private void Begin(Vector2 pos)
        {
            _isDragging = true;
            _lastPos = pos;
            _hasLastPos = true;
            DragStarted?.Invoke(pos);
        }

        private void End(Vector2 pos)
        {
            _isDragging = false;
            _activeTouchId = -1;
            _hasLastPos = false;
            DragFinished?.Invoke(pos);
        }

        private void GetPointerState(out Vector2 pos, out bool down, out bool held, out bool up)
        {
            if (Input.touchCount > 0)
            {
                Touch t = GetActiveTouch();

                pos = t.position;
                down = t.phase == TouchPhase.Began;
                up = t.phase == TouchPhase.Ended || t.phase == TouchPhase.Canceled;

                held = down || t.phase == TouchPhase.Moved || t.phase == TouchPhase.Stationary;

                if (down) _activeTouchId = t.fingerId;
                if (up) _activeTouchId = -1;

                return;
            }

            pos = Input.mousePosition;
            down = Input.GetMouseButtonDown(0);
            held = Input.GetMouseButton(0);
            up = Input.GetMouseButtonUp(0);
        }

        private Touch GetActiveTouch()
        {
            if (_activeTouchId == -1)
                return Input.GetTouch(0);

            for (int i = 0; i < Input.touchCount; i++)
            {
                var t = Input.GetTouch(i);
                if (t.fingerId == _activeTouchId)
                    return t;
            }

            return Input.GetTouch(0);
        }
    }
}