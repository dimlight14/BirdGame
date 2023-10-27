using System.Collections.Generic;

namespace Birdgame.Missions.MissionLocks
{
    public class SimpleLock : IMissionLock
    {
        private readonly string _missionId;

        public SimpleLock(string missionId)
        {
            _missionId = missionId;
        }

        public bool CheckIfUnlocked(HashSet<string> completedMissions)
        {
            return completedMissions.Contains(_missionId);
        }
    }
}