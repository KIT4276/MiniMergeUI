using MiniMergeUI.View;
using System;
using System.Collections.Generic;

namespace MiniMergeUI.Services
{
    public class BoardState
    {
        private readonly Dictionary<Cell, Chip> _cellToChip = new();
        private readonly Dictionary<Chip, Cell> _chipToCell = new();

        public bool IsOccupied(Cell cell) => 
            cell != null && _cellToChip.ContainsKey(cell);

        public bool TryGetOccupant(Cell cell, out Chip chip) =>
            _cellToChip.TryGetValue(cell, out chip);

        public bool TryGetCell(Chip chip, out Cell cell) =>
            _chipToCell.TryGetValue(chip, out cell);

        public void Place(Cell cell, Chip chip)
        {
            if (cell == null || chip == null) return;

            if (_cellToChip.TryGetValue(cell, out var prevChip))
                _chipToCell.Remove(prevChip);

            if (_chipToCell.TryGetValue(chip, out var prevCell))
                _cellToChip.Remove(prevCell);

            _cellToChip[cell] = chip;
            _chipToCell[chip] = cell;
        }

        public void RemoveChip(Chip chip)
        {
            if (chip == null) return;
            if (_chipToCell.TryGetValue(chip, out var cell))
            {
                _chipToCell.Remove(chip);
                _cellToChip.Remove(cell);
            }
        }

        public void ClearBoard()
        {
            _chipToCell.Clear();
            _cellToChip.Clear();
        }
    }
}
