using System.Collections.Generic;

namespace Birdgame.Missions.MissionLocks
{
    public class AlwaysTrueLock : IMissionLock
    {
        public bool CheckIfUnlocked(HashSet<string> completedMissions)
        {
            return true;
        }
    }
}