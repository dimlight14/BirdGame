using System.Collections.Generic;
using Birdgame.Heroes;
using Birdgame.Missions.Data;
using UnityEngine;

namespace Birdgame.Editor
{
    public class DoubleMissionNodeData : ScriptableObject
    {
        [Header("Mission a")]
        public string MissionAId;
        public List<HeroType> MissionAHeroUnlocks = new ();
        public List<MissionHeroReward> MissionAHeroPowerRewards = new();
        public MissionDescription MissionADescription;
        
        [Header("Mission b")]
        [Space(15)]
        public string MissionBId;
        public List<HeroType> MissionBHeroUnlocks = new();
        public List<MissionHeroReward> MissionBHeroPowerRewards = new();
        public MissionDescription MissionBDescription;

        [Header("Shared Data")]
        [Space(15)]
        public List<string> TemporarilyHides = new();
        public Vector2 MapPosition;
    }
}