using MiniMergeUI.Services.Factories;
using MiniMergeUI.View;
using System;
using Zenject;

namespace MiniMergeUI.Services
{
    public class SpawnConductor : IInitializable, IDisposable
    {
        private readonly GameCanvas _gameCanvas;
        private readonly ChipFactory _chipFactory;

        public SpawnConductor(GameCanvas gameCanvas, ChipFactory chipFactory)
        {
            _gameCanvas = gameCanvas;
            _chipFactory = chipFactory;
        }

        public void Initialize() => 
            _gameCanvas.SpawnClicked += OnSpawnClicked;

        public void Dispose() => 
            _gameCanvas.SpawnClicked -= OnSpawnClicked;

        private void OnSpawnClicked()
        {
            if (!_chipFactory.TrySpawnRandomEmpty(out _))
            {
                _gameCanvas.ShowMessage();
            }
        }
    }
}