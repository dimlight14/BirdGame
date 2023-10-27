using System.Collections.Generic;

namespace Birdgame.Missions.MissionLocks
{
    public interface IMissionLock
    {
        bool CheckIfUnlocked(HashSet<string> completedMissions);
    }
}