using System;
using Birdgame.Heroes;
using Birdgame.Missions.Data;

namespace Birdgame.Missions
{
    public class MissionManager
    {
        public Action<string> OnMissionPreview;
        public Action<string> OnMissionStarted;
        public Action OnMissionAborted;
        public Action OnMissionEnded;
        public Action OnHeroSelected;

        private readonly HeroesDataManager _heroDataManager;
        private readonly MissionsDataManager _missionDataManager;

        private HeroType _selectedHero = HeroType.None;

        public MissionManager(HeroesDataManager heroDataManager, MissionsDataManager missionDataManager)
        {
            _heroDataManager = heroDataManager;
            _missionDataManager = missionDataManager;
        }

        public void PreviewMission(string missionId)
        {
            OnMissionPreview?.Invoke(missionId);
        }

        public void SelectHeroForMission(HeroType heroType)
        {
            _selectedHero = heroType;
            OnHeroSelected?.Invoke();
        }

        public void StartMission(string missionId)
        {
            if (_selectedHero == HeroType.None)
            {
                return;
            }

            OnMissionStarted?.Invoke(missionId);
        }

        public void AbortMission()
        {
            _selectedHero = HeroType.None;
            OnMissionAborted?.Invoke();
        }

        public void EndMission(string missionId)
        {
            _missionDataManager.SetMissionCompleted(missionId);
            ClaimMissionReward(missionId);
            _selectedHero = HeroType.None;
            OnMissionEnded?.Invoke();
        }

        private void ClaimMissionReward(string missionId)
        {
            var missionData = _missionDataManager.GetMissionData(missionId);

            foreach (var heroTypeUnlock in missionData.HeroUnlocks)
            {
                _heroDataManager.UnlockHero(heroTypeUnlock);
            }

            foreach (var missionPointReward in missionData.HeroPowerRewards)
            {
                _heroDataManager.ChangeHeroPower(
                    missionPointReward.HeroType == HeroType.AnyActive ? _selectedHero : missionPointReward.HeroType,
                    missionPointReward.PowerPoints
                );
            }
        }
    }
}