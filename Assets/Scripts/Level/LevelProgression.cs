using System;
using Platformer.Drop;

namespace Platformer.Level{
    [Serializable]
    public class LevelProgression{
        public DropType RequiredDropType;
        public int RequiredDropQuantity;

        public bool CheckGateOpenCondition(DropType dropType, int numberOfDropsCollected) => dropType == RequiredDropType && numberOfDropsCollected >= RequiredDropQuantity;
    }
}