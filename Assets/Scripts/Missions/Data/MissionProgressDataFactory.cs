using System.Collections.Generic;
using Birdgame.Missions.MissionLocks;

namespace Birdgame.Missions.Data
{
    public class MissionProgressDataFactory
    {
        public MissionProgressData Create(MissionStaticData missionStaticData)
        {
            var newData = new MissionProgressData()
            {
                Id = missionStaticData.MissionId,
                MissionState = MissionState.Hidden,
                MissionType = missionStaticData.MissionType
            };
            if (missionStaticData.SimpleUnlockConditions.Count == 0 && missionStaticData.EitherOrUnlockConditions.Count == 0)
            {
                newData.MissionLock = new AlwaysTrueLock();
                return newData;
            }

            var locks = new List<IMissionLock>(missionStaticData.SimpleUnlockConditions.Count + missionStaticData.EitherOrUnlockConditions.Count);
            foreach (var simpleLock in missionStaticData.SimpleUnlockConditions)
            {
                locks.Add(new SimpleLock(simpleLock));
            }

            foreach (var doubleStirng in missionStaticData.EitherOrUnlockConditions)
            {
                locks.Add(new DoubleLock(doubleStirng.ConditionA, doubleStirng.ConditionB));
            }

            newData.MissionLock = new MissionLocksConatainer(locks);
            return newData;
        }
    }
}