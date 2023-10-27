using Birdgame.Missions.MissionLocks;

namespace Birdgame.Missions.Data
{
    public class MissionProgressData
    {
        public string Id;
        public MissionState MissionState;
        public MissionType MissionType;
        public IMissionLock MissionLock;
    }
}