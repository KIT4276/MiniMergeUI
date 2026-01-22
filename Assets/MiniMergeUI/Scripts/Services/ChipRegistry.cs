using MiniMergeUI.View;
using System.Collections.Generic;

namespace MiniMergeUI.Services
{
    public class ChipRegistry
    {
        private readonly List<Chip> _activeChips = new();
        public IReadOnlyList<Chip> ActiveChips => _activeChips;

        public void Add(Chip chip)
        {
            if (chip != null && !_activeChips.Contains(chip))
                _activeChips.Add(chip);
        }

        public void Remove(Chip chip) => 
            _activeChips.Remove(chip);
    }
}
