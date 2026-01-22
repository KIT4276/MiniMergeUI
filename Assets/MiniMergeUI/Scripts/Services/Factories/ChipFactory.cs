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
        private readonly ChipVisualLibrary _visuals;
        private readonly List<Cell> _freeCells = new();

        private int _chipStartCount = 3; // - TODO to configs

        private List<Chip> _chipsPpool = new();

        public ChipFactory(GameCanvas gameCanvas, GameObject chipPrefab, BoardState boardState,
                      ChipPlacer placer, ChipRegistry registry, ChipVisualLibrary visuals)
        {
            _gameCanvas = gameCanvas;
            _chipPrefab = chipPrefab;
            _boardState = boardState;
            _placer = placer;
            _chipRegistry = registry;
            _visuals = visuals;
        }

        public void Initialize()
        {
            StartSpawn();
        }

        public Chip Spawn(Cell cell, int level)
        {
            Chip chip = null;

            if (_chipsPpool.Count == 0)
            {
                var chipObj = Object.Instantiate(_chipPrefab, _gameCanvas.RectTransform, false);
                chip = chipObj.GetComponent<Chip>();

                chip.Init(_gameCanvas.RectTransform, _gameCanvas.Canvas);
            }
            else
            {
                chip = _chipsPpool[_chipsPpool.Count - 1];
                chip.gameObject.SetActive(true);
                _chipsPpool.Remove(chip);
            }

            chip.RectTransform.anchoredPosition = new Vector2(0, 0);
            var type = _visuals.GetRandomType();
            chip.SetIdentity(type, level, sprite: _visuals.GetSprite(type));

            _placer.Place(chip, cell);
            _chipRegistry.Add(chip);

            return chip;
        }

        public bool TrySpawnRandomEmpty(out Chip chip)
        {
            chip = null;

            _freeCells.Clear();
            var cells = _gameCanvas.Cells;
            for (int i = 0; i < cells.Length; i++)
            {
                if (!_boardState.IsOccupied(cells[i]))
                    _freeCells.Add(cells[i]);
            }

            if (_freeCells.Count == 0)
                return false;

            var cell = _freeCells[Random.Range(0, _freeCells.Count)];
            chip = Spawn(cell, 0);
            return true;
        }

        public void Despawn(Chip occupant)
        {
            occupant.gameObject.SetActive(false);
            _chipRegistry.Remove(occupant);
            _chipsPpool.Add(occupant);
        }

        private void StartSpawn()
        {
            Canvas.ForceUpdateCanvases();
            LayoutRebuilder.ForceRebuildLayoutImmediate(_gameCanvas.GridRoot);

            for (int i = 0; i < _chipStartCount; i++)
                TrySpawnRandomEmpty(out var chip);
        }
    }
}
