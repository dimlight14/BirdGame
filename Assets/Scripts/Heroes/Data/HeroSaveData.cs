using System;

namespace Birdgame.Heroes.Data
{
    [Serializable]
    public class HeroSaveData
    {
        public HeroType HeroType;
        public int PowerPoints;
        public bool IsUnlocked;
    }
}