using MiniMergeUI.Services.Factories;
using MiniMergeUI.View;

namespace MiniMergeUI.Services
{
    public class MergeServices
    {
        private readonly ChipFactory _chipFactory;
        private readonly BoardState _boardState;

        public MergeServices(ChipFactory chipFactory, BoardState boardState)
        {
            _chipFactory = chipFactory;
            _boardState = boardState;
        }

        public void Merge(Chip _chosenChip, Chip occupant)
        {
            if (!_boardState.TryGetCell(_chosenChip, out var cell)) return;

            var type = occupant.Type;
            var newChip = _chipFactory.Spawn(cell, _chosenChip.Level + 1, type);
            newChip.Merge();

            _chipFactory.Despawn(occupant);
            _boardState.RemoveChip(occupant);

            _chipFactory.Despawn(_chosenChip);
        }
    }
}