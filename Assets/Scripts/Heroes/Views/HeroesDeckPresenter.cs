using Birdgame.Missions;

namespace Birdgame.Heroes.Views
{
    public class HeroesDeckPresenter
    {
        private readonly HeroesDeckView _heroesDeckView;
        private readonly HeroesDataManager _heroesDataManager;
        private readonly MissionManager _missionManager;
        
        private bool _heroSelectionMode = false;

        public HeroesDeckPresenter(HeroesDeckView heroesDeckView, HeroesDataManager heroesDataManager, MissionManager missionManager)
        {
            _heroesDeckView = heroesDeckView;
            _heroesDataManager = heroesDataManager;
            _missionManager = missionManager;

            _heroesDataManager.OnHeroUnlocked += CreateHeroCard;
            _heroesDataManager.OnHeroPointsChanged += UpdateHeroPower;
            _missionManager.OnMissionPreview += EnterHeroSelection;
            _missionManager.OnMissionStarted += ExitHeroSelection;
            _missionManager.OnMissionAborted += ExitHeroSelection;
        }

        private void UpdateHeroPower(HeroType heroType, int newPower)
        {
            _heroesDeckView.UpdateHeroPower(heroType, newPower);
        }

        private void EnterHeroSelection(string missionId)
        {
            _heroSelectionMode = true;
            _heroesDeckView.EnableButtons();
        }

        private void ExitHeroSelection(string missionId )
        {
            ExitHeroSelection();
        }

        private void ExitHeroSelection()
        {
            _heroSelectionMode = false;
            _heroesDeckView.RemoveSelectionIndication();
            _heroesDeckView.DisableButtons();
        }

        private void SelectHero(HeroType heroType)
        {
            if (!_heroSelectionMode)
            {
                return;
            }

            _heroesDeckView.RemoveSelectionIndication();
            _heroesDeckView.SetSelectionIndicationForHero(heroType);
            _missionManager.SelectHeroForMission(heroType);
        }

        private void CreateHeroCard(HeroType heroType)
        {
            var saveData = _heroesDataManager.HeroSaveData[heroType];
            _heroesDeckView.CreateHeroCard(heroType, _heroesDataManager.GetHeroStaticData(heroType).Name, saveData.PowerPoints, SelectHero);
        }
    }
}
