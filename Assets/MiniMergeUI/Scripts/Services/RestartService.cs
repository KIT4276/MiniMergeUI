using MiniMergeUI.Services;
using MiniMergeUI.Services.Factories;
using MiniMergeUI.View;
using System;
using Zenject;

public class RestartService : IInitializable, ILateDisposable
{
    private readonly GameCanvas _gameCanvas;
    private readonly ChipFactory _chipFactory;
    private readonly BoardState _boardState;

    public RestartService(GameCanvas gameCanvas, ChipFactory chipFactory, BoardState boardState)
    {
        _gameCanvas = gameCanvas;
        _chipFactory = chipFactory;
        _boardState = boardState;
    }

    public void Initialize()
    {
        _gameCanvas.RestartClicked += OnRestartClicked;
    }

    public void LateDispose()
    {
        _gameCanvas.RestartClicked -= OnRestartClicked;
    }

    private void OnRestartClicked()
    {
        _chipFactory.DespawnAll();
        _boardState.ClearBoard();
        _chipFactory.StartSpawn();
    }
}
