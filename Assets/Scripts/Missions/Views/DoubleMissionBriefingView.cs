using UnityEngine;

namespace Birdgame.Missions.Views
{
    public class DoubleMissionBriefingView : MonoBehaviour
    {
        [SerializeField] private MissionBriefingView _missionAView;
        [SerializeField] private MissionBriefingView _missionBView;

        public MissionBriefingView MissionAView => _missionAView;
        public MissionBriefingView MissionBView =>_missionBView;

        public void Hide()
        {
            _missionAView.Hide();
            _missionBView.Hide();
        }
    }
}