using System.Collections.Generic;

namespace Birdgame.Missions.MissionLocks
{
    public class MissionLocksConatainer : IMissionLock
    {
        private readonly List<IMissionLock> _missionLocks;

        public MissionLocksConatainer(List<IMissionLock> missionLocks)
        {
            _missionLocks = missionLocks;
        }

        public bool CheckIfUnlocked(HashSet<string> completedMissions)
        {
            foreach (var missionLock in _missionLocks)
            {
                if (!missionLock.CheckIfUnlocked(completedMissions))
                {
                    return false;
                }
            }

            return true;
        }
    }
}