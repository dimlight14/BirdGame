using System;
using Birdgame.Heroes;

namespace Birdgame.Missions.Data
{
    [Serializable]
    public class MissionHeroReward
    {
        public HeroType HeroType;
        public int PowerPoints;
    }
}