using System.Collections.Generic;
using Birdgame.Heroes;
using Birdgame.Missions.Data;
using UnityEngine;

namespace Birdgame.Editor
{
    public class SingleMissionNodeData : ScriptableObject
    {
        public string Id;
        public List<HeroType> HeroUnlocks = new ();
        public List<MissionHeroReward> HeroPowerRewards = new();
        public List<string> TemporarilyHides = new();
        public Vector2 MapPosition;
        public MissionDescription Description;
    }
}