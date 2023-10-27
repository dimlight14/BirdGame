using Birdgame.Missions.Data;

namespace Birdgame.Missions.Views
{
    public class MissionsPresenter
    {
        private readonly MissionManager _missionManager;
        private readonly MissionsDataManager _missionsDataManager;
        private readonly MissionBriefingView _briefingView;
        private readonly DoubleMissionBriefingView _doubleMissionBriefingView;
        private readonly MissionView _missionView;
        private readonly RaycastBlocker _raycastBlocker;

        public MissionsPresenter(
            MissionManager missionManager,
            MissionsDataManager missionsDataManager,
            MissionView missionView,
            MissionBriefingView briefingView,
            DoubleMissionBriefingView doubleMissionBriefingView,
            RaycastBlocker raycastBlocker
        )
        {
            _missionManager = missionManager;
            _missionsDataManager = missionsDataManager;
            _missionView = missionView;
            _briefingView = briefingView;
            _doubleMissionBriefingView = doubleMissionBriefingView;
            _raycastBlocker = raycastBlocker;

            _missionManager.OnMissionPreview += OpenBriefing;
            _missionManager.OnMissionStarted += OpenMainMissionView;
            _missionManager.OnMissionEnded += HideMissionView;
        }

        private void OpenBriefing(string missionId)
        {
            var staticData = _missionsDataManager.GetMissionData(missionId);
            var descriptionData = _missionsDataManager.GetMissionDescription(missionId);
            _raycastBlocker.Activate(AbortMission);
            if (staticData.MissionType == MissionType.SingleMission)
            {
                _briefingView.Show(missionId, descriptionData.Title, descriptionData.IntroductoryDescription, PreviewButtonClicked);
            }
            else
            {
                _doubleMissionBriefingView.MissionAView.Show(missionId, descriptionData.Title, descriptionData.IntroductoryDescription, PreviewButtonClicked);
                var competingDescription = _missionsDataManager.GetMissionDescription(staticData.DoubleMissionData.CompetingMission);
                _doubleMissionBriefingView.MissionBView.Show(staticData.DoubleMissionData.CompetingMission, competingDescription.Title, competingDescription.IntroductoryDescription, PreviewButtonClicked);
            }
        }

        private void OpenMainMissionView(string missionId)
        {
            _raycastBlocker.Activate();
            _briefingView.Hide();
            _doubleMissionBriefingView.Hide();
            var descriptionData = _missionsDataManager.GetMissionDescription(missionId);
            _missionView.Show(missionId, descriptionData.Title, descriptionData.MainDescription, descriptionData.OurSide, descriptionData.TheirSide, MainMissionViewButtonClicked);
        }

        private void PreviewButtonClicked(string missionId)
        {
            _missionManager.StartMission(missionId);
        }

        private void MainMissionViewButtonClicked(string missionId)
        {
            _missionManager.EndMission(missionId);
        }

        private void AbortMission()
        {
            _raycastBlocker.Hide();
            _briefingView.Hide();
            _doubleMissionBriefingView.Hide();
            _missionManager.AbortMission();
        }

        private void HideMissionView()
        {
            _missionView.Hide();
            _raycastBlocker.Hide();
        }
    }
}