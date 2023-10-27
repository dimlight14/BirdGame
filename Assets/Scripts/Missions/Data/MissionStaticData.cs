using System;
using System.Collections.Generic;
using Birdgame.Heroes;
using UnityEngine;

namespace Birdgame.Missions.Data
{
    [Serializable]
    public class MissionStaticData
    {
        public string MissionId;
        public MissionType MissionType;
        public List<string> SimpleUnlockConditions = new ();
        public List<EitherOrCondition> EitherOrUnlockConditions =new ();
        public List<HeroType> HeroUnlocks;
        public List<string> TemporarilyHides;
        public List<MissionHeroReward> HeroPowerRewards;
        public MissionDescription Description;
        public Vector2 MapPosition;
        public DoubleMissionData DoubleMissionData;
    }

    [Serializable]
    public class DoubleMissionData
    {
        public string CompetingMission;
        /*Одна из двойных миссий должна быть помечена как Subservient.
        У subservient миссий нет своего отображения на карте и залочек, они полагаются на основную миссию. */
        public bool IsSubservient;
    }
}