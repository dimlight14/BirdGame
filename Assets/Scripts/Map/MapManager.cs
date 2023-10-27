using Birdgame.Missions.Data;

namespace Birdgame.Missions.Map
{
    public class MapManager
    {
        private readonly MissionsDataManager _missionsDataManager;
        private readonly MapView _mapView;
        private readonly MissionManager _missionManager;

        public MapManager(MissionsDataManager missionsDataManager, MapView mapView, MissionManager missionManager)
        {
            _missionsDataManager = missionsDataManager;
            _mapView = mapView;
            _missionManager = missionManager;

            _missionsDataManager.OnMissionStateChanged += ChangeMissionState;
        }

        private void MissionClicked(string missionId)
        {
            if (!_missionsDataManager.MissionsProgressData.TryGetValue(missionId, out var missionProgressData))
            {
                return;
            }

            if (missionProgressData.MissionState == MissionState.Active)
            {
                _missionManager.PreviewMission(missionId);
            }
        }

        private void ChangeMissionState(string missionId, MissionState newState)
        {
            var staticData = _missionsDataManager.GetMissionData(missionId);
            if (staticData.MissionType == MissionType.DoubleMission && staticData.DoubleMissionData.IsSubservient)
            {
                return;
            }
            
            if (_mapView.HasPointForMission(missionId))
            {
                _mapView.ChangeMissionState(missionId, newState);
            }
            else
            {
                _mapView.CreateMissionPoint(missionId, newState, staticData.MapPosition, MissionClicked);
            }
        }
    }
}