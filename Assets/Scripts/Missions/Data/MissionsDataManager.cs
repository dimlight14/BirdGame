using System;
using System.Collections.Generic;

namespace Birdgame.Missions.Data
{
    public class MissionsDataManager
    {
        public event Action<string, MissionState> OnMissionStateChanged;

        private readonly MissionsDatabase _missionsDatabase;
        private readonly Dictionary<string, MissionProgressData> _missionsProgressData = new();
        private readonly MissionProgressDataFactory _progressDataFactory;

        private readonly HashSet<string> _completedMissions = new();
        private readonly List<string> _missionsToTemporarilyHide = new();
        public Dictionary<string, MissionProgressData> MissionsProgressData => _missionsProgressData;

        public MissionsDataManager(MissionsDatabase missionsDatabase)
        {
            _missionsDatabase = missionsDatabase;
            _progressDataFactory = new MissionProgressDataFactory();
        }

        public void StartGame()
        {
            PopulateMissionsList();
            TryUnlockMissions();
        }

        public MissionDescription GetMissionDescription(string missionId)
        {
            return _missionsDatabase.MissionsDataById[missionId].Description;
        }

        public MissionStaticData GetMissionData(string missionId)
        {
            return _missionsDatabase.MissionsDataById[missionId];
        }

        public void SetMissionCompleted(string missionId)
        {
            if (_missionsDatabase.MissionsDataById[missionId].MissionType == MissionType.DoubleMission)
            {
                SetMissionState(_missionsDatabase.MissionsDataById[missionId].DoubleMissionData.CompetingMission, MissionState.Discarded);
                RemoveTemporaryBlocks(_missionsDatabase.MissionsDataById[missionId].DoubleMissionData.CompetingMission);
            }

            SetMissionState(missionId, MissionState.Completed);
            RemoveTemporaryBlocks(missionId);
            _completedMissions.Add(missionId);
            TryUnlockMissions();
        }

        private void RemoveTemporaryBlocks(string blockingMissionId)
        {
            var blockingMissionData = _missionsDatabase.MissionsDataById[blockingMissionId];
            if (blockingMissionData.TemporarilyHides.Count == 0)
            {
                return;
            }

            foreach (var tempHiddenMission in blockingMissionData.TemporarilyHides)
            {
                if (_missionsProgressData[tempHiddenMission].MissionState == MissionState.TemporarilyHidden)
                {
                    UnlockMission(tempHiddenMission);
                }
            }
        }

        private void PopulateMissionsList()
        {
            foreach (var (missionId, missionData) in _missionsDatabase.MissionsDataById)
            {
                _missionsProgressData[missionId] = _progressDataFactory.Create(missionData);
            }
        }

        private void TryUnlockMissions()
        {
            _missionsToTemporarilyHide.Clear();
            foreach (var (missionId, progressData) in _missionsProgressData)
            {
                if (progressData.MissionState != MissionState.Hidden)
                {
                    continue;
                }

                if (progressData.MissionType == MissionType.DoubleMission)
                {
                    TryUnlockDoubleMission(progressData);
                    continue;
                }

                if (progressData.MissionLock.CheckIfUnlocked(_completedMissions))
                {
                    UnlockMission(missionId);
                }
            }

            TemporarilyHideMissions();
        }

        private void TemporarilyHideMissions()
        {
            foreach (var missionToBlock in _missionsToTemporarilyHide)
            {
                if (_missionsProgressData[missionToBlock].MissionState == MissionState.Active)
                {
                    SetMissionState(missionToBlock, MissionState.TemporarilyHidden);
                }
            }

            _missionsToTemporarilyHide.Clear();
        }

        private void UnlockMission(string missionId)
        {
            SetMissionState(missionId, MissionState.Active);
            if (_missionsDatabase.MissionsDataById[missionId].TemporarilyHides.Count > 0)
            {
                foreach (var blockMissionId in _missionsDatabase.MissionsDataById[missionId].TemporarilyHides)
                {
                    _missionsToTemporarilyHide.Add(blockMissionId);
                }
            }
        }

        private void TryUnlockDoubleMission(MissionProgressData progressData)
        {
            var staticData = _missionsDatabase.MissionsDataById[progressData.Id];
            if (staticData.DoubleMissionData.IsSubservient ||
                !progressData.MissionLock.CheckIfUnlocked(_completedMissions))
            {
                return;
            }

            UnlockMission(progressData.Id);
            UnlockMission(staticData.DoubleMissionData.CompetingMission);
        }

        private void SetMissionState(string missionId, MissionState newState)
        {
            _missionsProgressData[missionId].MissionState = newState;
            OnMissionStateChanged?.Invoke(missionId, newState);
        }
    }
}