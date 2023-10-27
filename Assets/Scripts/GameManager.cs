using Birdgame.Heroes;
using Birdgame.Missions.Data;

namespace Birdgame
{
    public class GameManager
    {
        private readonly HeroesDataManager _heroesDataManager;
        private readonly MissionsDataManager _missionsDataManager;
        
        public GameManager(HeroesDataManager heroesDataManager, MissionsDataManager missionsDataManager)
        {
            _heroesDataManager = heroesDataManager;
            _missionsDataManager = missionsDataManager;
        }
        
        public void StartGame()
        {
            _heroesDataManager.StartGame();
            _missionsDataManager.StartGame();
        }
    }
}