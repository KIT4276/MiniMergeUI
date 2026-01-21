using MiniMergeUI.View;
using System.Collections.Generic;
using UnityEngine;

namespace MiniMergeUI.Services
{
    public class BoardState
    {
        private Dictionary<Cell, Chip> _map = new();

        public bool IsOccupied(Cell cell) =>
            _map.ContainsKey(cell);

        public bool TryGetOccupant(Cell cell, out Chip chip) =>
            _map.TryGetValue(cell, out chip);

        public void Place(Cell cell, Chip chip) => 
            _map[cell] = chip;

        public void Clear(Cell cell) => 
            _map.Remove(cell);

        public void Move(Cell from, Cell to, Chip chip)
        {
            _map.Remove(from);
            _map[to] = chip;
        }
    }
}
