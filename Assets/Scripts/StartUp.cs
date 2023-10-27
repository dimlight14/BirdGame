using Birdgame.Heroes;
using Birdgame.Heroes.Data;
using Birdgame.Heroes.Views;
using Birdgame.Missions;
using Birdgame.Missions.Data;
using Birdgame.Missions.Map;
using Birdgame.Missions.Views;
using UnityEngine;

namespace Birdgame
{
    public class StartUp : MonoBehaviour
    {
        [SerializeField] private HeroesSettings _heroesSettings;
        [SerializeField] private HeroesDeckView _heroesDeckView;
        [SerializeField] private MissionsDatabase _missionsDatabase;
        [SerializeField] private MapView _mapView;
        [SerializeField] private MissionView _missionView;
        [SerializeField] private MissionBriefingView _briefingView;
        [SerializeField] private DoubleMissionBriefingView _doubleMissionBriefingView;
        [SerializeField] private RaycastBlocker _raycastBlocker;

        private GameManager _gameManager;

        private void Start()
        {
            var heroDataManager = new HeroesDataManager(_heroesSettings);
            var missionDataManager = new MissionsDataManager(_missionsDatabase);
            
            var missionManager = new MissionManager(heroDataManager,missionDataManager);
            var mapManager = new MapManager(missionDataManager, _mapView, missionManager);
            var deckPresenter = new HeroesDeckPresenter(_heroesDeckView, heroDataManager, missionManager);

            var missionPresenter = new MissionsPresenter(missionManager, missionDataManager, _missionView, _briefingView, _doubleMissionBriefingView, _raycastBlocker);

            _gameManager = new GameManager(heroDataManager, missionDataManager);
            _gameManager.StartGame();
        }
    }
}