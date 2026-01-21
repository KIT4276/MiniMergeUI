using MiniMergeUI.View;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace MiniMergeUI.Services.Factories
{
    public class ChipFactory : IInitializable
    {
        private readonly GameCanvas _gameCanvas;
        private readonly GameObject _chipPrefab;
        private readonly BoardState _boardState;
        private List<GameObject> _pull;

        private readonly List<Cell> _freeCells = new();

        private int _chipStartCount = 3; // - TODO to configs

        public List<Chip> ActivePull { get; private set; }

        public ChipFactory(GameCanvas gameCanvas, GameObject chipPrefab, BoardState boardState)
        {
            _gameCanvas = gameCanvas;
            _chipPrefab = chipPrefab;
            _boardState = boardState;
            ActivePull = new();
        }

        public void Initialize()
        {
            StartSpawn();
        }

        private void StartSpawn()
        {
            Canvas.ForceUpdateCanvases();
            LayoutRebuilder.ForceRebuildLayoutImmediate(_gameCanvas.GridRoot);

            _freeCells.Clear();
            var cells = _gameCanvas.Cells;
            for (int i = 0; i < cells.Length; i++)
            {
                if(!_boardState.IsOccupied(cells[i]))
                    _freeCells.Add(cells[i]);
            }

            int toSpawn = Mathf.Min(_chipStartCount, _freeCells.Count);

            for (int i = 0; i < toSpawn; i++)
            {
                int idx = Random.Range(0, _freeCells.Count);
                Cell cell = _freeCells[idx];
                _freeCells[idx] = _freeCells[^1];
                _freeCells.RemoveAt(_freeCells.Count - 1);

                Spawn(cell);
            }
        }

        private Chip Spawn(Cell cell)
        {
            GameObject chipObj = GameObject.Instantiate(_chipPrefab, Vector3.zero, Quaternion.identity);
            chipObj.transform.parent = _gameCanvas.RectTransform;
            Chip chip = chipObj.GetComponent<Chip>();
            ActivePull.Add(chip);
            chip.Init(_gameCanvas.RectTransform, _gameCanvas.Canvas);

            chip.SnapTo(_gameCanvas.Cells[Random.Range(0, _gameCanvas.Cells.Length)]);// делать через ChipsConductor
            _boardState.Place(cell, chip);// делать через ChipsConductor

            return chip;
        }
    }
}
