using System.Collections.Generic;

namespace Birdgame.Missions.MissionLocks
{
    public class DoubleLock : IMissionLock
    {
        private readonly string _missionAId;
        private readonly string _missionBId;

        public DoubleLock(string missionA, string missionB)
        {
            _missionAId = missionA;
            _missionBId = missionB;
        }

        public bool CheckIfUnlocked(HashSet<string> completedMissions)
        {
            return completedMissions.Contains(_missionAId) || completedMissions.Contains(_missionBId);
        }
    }
}