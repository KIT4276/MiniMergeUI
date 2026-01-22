using MiniMergeUI.Services.Factories;
using MiniMergeUI.View;
using System;
using UnityEngine;
using Zenject;

namespace MiniMergeUI.Services
{
    public class ChipsConductor : IInitializable, IDisposable
    {
        private readonly DragHandler _dragHandler;
        private readonly CellsConductor _cellsConductor;
        private readonly BoardState _boardState;
        private readonly ChipPlacer _chipPlacer;
        private readonly ChipRegistry _chipRegistry;

        private Chip _chosenChip;
        private Cell _fromCell;

        public ChipsConductor(DragHandler drag, CellsConductor cellsConductor, BoardState board,
                          ChipPlacer placer, ChipRegistry registry)
        {
            _dragHandler = drag;
            _cellsConductor = cellsConductor;
            _boardState = board;
            _chipPlacer = placer;
            _chipRegistry = registry;
        }

        public void Initialize()
        {
            _dragHandler.DragStarted += OnDragStarted;
            _dragHandler.DragOngoing += OnDragOngoing;
            _dragHandler.DragFinished += OnDragFinished;
        }

        public void Dispose()
        {
            _dragHandler.DragStarted -= OnDragStarted;
            _dragHandler.DragOngoing -= OnDragOngoing;
            _dragHandler.DragFinished -= OnDragFinished;
        }

        private void OnDragStarted(Vector2 screenPos)
        {
            CleanupDrag();

            if (!TryGetChipUnderPointer(screenPos, out var chip))
                return;

            _chosenChip = chip;

            _boardState.TryGetCell(_chosenChip, out _fromCell);
            _boardState.RemoveChip(_chosenChip);

            _chosenChip.BeginDrag(screenPos);

        }

        private void OnDragOngoing(Vector2 screenPos)
        {
            if (_chosenChip == null) return;
            _chosenChip.DragTo(screenPos);
        }

        private void OnDragFinished(Vector2 screenPos)
        {
            if (_chosenChip == null) return;

            _chosenChip.EndDrag();

            var targetCell = _cellsConductor.GetNearestCell(_chosenChip.RectTransform.anchoredPosition);

            if (targetCell == null)
            {
                _chipPlacer.Place(_chosenChip, _fromCell);
                CleanupDrag();
                return;
            }
            
            if (_boardState.TryGetOccupant(targetCell, out var occupant))
            {
                //TODO  заменить на CanMerge(...) ? DoMerge(...) : Return
                Debug.Log("проверяем, можно ли смёрджить");
                _chipPlacer.Place(_chosenChip, _fromCell);
            }
            else
            {
                _chipPlacer.Place(_chosenChip, targetCell);
            }

            CleanupDrag();
        }

        private void CleanupDrag()
        {
            _chosenChip = null;
            _fromCell = null;
        }

        private bool TryGetChipUnderPointer(Vector2 position, out Chip bestChip)
        {
            bestChip = null;
            int bestSibling = int.MinValue;

            var chips = _chipRegistry.ActiveChips;

            for (int i = 0; i < chips.Count; i++)
            {
                var chip = chips[i];
                if (chip == null || !chip.gameObject.activeInHierarchy) continue;

                //  лучше передавать камеру канваса, если не Overlay
                if (!RectTransformUtility.RectangleContainsScreenPoint(chip.RectTransform, position, null))
                    continue;

                int sibling = chip.RectTransform.GetSiblingIndex();
                if (sibling >= bestSibling)
                {
                    bestSibling = sibling;
                    bestChip = chip;
                }
            }

            return bestChip != null;
        }
    }
}