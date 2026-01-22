using MiniMergeUI.View;

namespace MiniMergeUI.Services
{
    public class ChipPlacer
    {
        private readonly BoardState _board;

        public ChipPlacer(BoardState board) => 
            _board = board;

        public void Place(Chip chip, Cell cell)
        {
            if (chip == null || cell == null) return;

            chip.SnapTo(cell);
            _board.Place(cell, chip);
        }
    }
}
