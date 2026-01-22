using System;
using UnityEngine;
using UnityEngine.UI;

namespace MiniMergeUI.View
{
    public class GameCanvas : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private RectTransform _gridRoot;
        [SerializeField] private Canvas _canvas;
        [SerializeField] private Cell[] _cells;
        [Space]
        [SerializeField] private Button _spawnButton;

        public Canvas Canvas { get => _canvas; }
        public RectTransform RectTransform { get => _rectTransform; }
        public RectTransform GridRoot { get => _gridRoot; }
        public Cell[] Cells { get => _cells; }

        public event Action SpawnClicked;

        private void Awake()
        {
            if (_spawnButton != null)
                _spawnButton.onClick.AddListener(OnSpawnClicked);
        }

        private void OnSpawnClicked()
        {
            SpawnClicked?.Invoke();
        }

        private void OnDestroy()
        {
            if (_spawnButton != null)
                _spawnButton.onClick.RemoveListener(OnSpawnClicked);
        }
    }
}
