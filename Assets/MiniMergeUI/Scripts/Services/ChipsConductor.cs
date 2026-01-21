using MiniMergeUI.Services.Factories;
using MiniMergeUI.View;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace MiniMergeUI.Services
{
    public class ChipsConductor : IInitializable, IDisposable
    {
        private readonly DragHandler _dragController;
        private readonly ChipFactory _chipFactory;
        private readonly CellsConductor _cellsConductor;
        private Chip _chosenChip;

        public ChipsConductor(DragHandler dragController, ChipFactory chipFactory, CellsConductor cellsConductor)
        {
            _dragController = dragController;
            _chipFactory = chipFactory;
            _cellsConductor = cellsConductor;
        }

        public void Initialize()
        {
            _dragController.DragStarted += OnDragStarted;
            _dragController.DragOngoing += OnDragOngoing;
            _dragController.DragFinished += OnDragFinished;
        }

        public void Dispose()
        {
            _dragController.DragStarted -= OnDragStarted;
            _dragController.DragOngoing -= OnDragOngoing;
            _dragController.DragFinished -= OnDragFinished;
        }

        private void OnDragStarted(Vector2 screenPos)
        {
            _chosenChip = null;

            if (TryGetChipUnderPointer(screenPos, out Chip chip))
            {
                _chosenChip = chip;
                _chosenChip.BeginDrag(screenPos);
            }
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

            var cell = _cellsConductor.GetNearestCell(_chosenChip.RectTransform.anchoredPosition);
            _chosenChip.SnapTo(cell);

            _chosenChip = null;
        }

        private bool TryGetChipUnderPointer(Vector2 position, out Chip bestChip)
        {
            bestChip = null;
            int bestSibling = int.MinValue;

            IReadOnlyList<Chip> chips = _chipFactory.ActivePull;

            for (int i = 0; i < chips.Count; i++)
            {
                var chip = chips[i];
                if (chip == null || !chip.gameObject.activeInHierarchy) continue;

                RectTransform rectTransform = chip.RectTransform; 
                if (!RectTransformUtility.RectangleContainsScreenPoint(rectTransform, position, null))
                    continue;

                int sibling = rectTransform.GetSiblingIndex();
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