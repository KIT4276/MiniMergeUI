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
        private readonly ChipPlacer _placer;
        private readonly ChipRegistry _chipRegistry;

        private readonly List<Cell> _freeCells = new();

        private int _chipStartCount = 3; // - TODO to configs

        public ChipFactory(GameCanvas gameCanvas, GameObject chipPrefab, BoardState boardState,
                      ChipPlacer placer, ChipRegistry registry)
        {
            _gameCanvas = gameCanvas;
            _chipPrefab = chipPrefab;
            _boardState = boardState;
            _placer = placer;
            _chipRegistry = registry;
        }

        public void Initialize()
        {
            StartSpawn();
        }

        public Chip Spawn(Cell cell)
        {
            
            var chipObj = Object.Instantiate(_chipPrefab, _gameCanvas.RectTransform, false);
            var chip = chipObj.GetComponent<Chip>();

            chip.Init(_gameCanvas.RectTransform, _gameCanvas.Canvas);

            _placer.Place(chip, cell);      
            _chipRegistry.Add(chip);           

            return chip;
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
    }
}
