using System;
using System.Collections.Generic;
using Birdgame.Heroes.Data;

namespace Birdgame.Heroes
{
    public class HeroesDataManager
    {
        public event Action<HeroType, int> OnHeroPointsChanged;
        public event Action<HeroType> OnHeroUnlocked;

        private readonly HeroesSettings _heroesSettings;
        public Dictionary<HeroType, HeroSaveData> HeroSaveData { get; } = new();

        public HeroesDataManager(HeroesSettings heroesSettings)
        {
            _heroesSettings = heroesSettings;
        }

        public void StartGame()
        {
            OnFirstLoading();
        }

        public HeroStaticData GetHeroStaticData(HeroType heroType)
        {
            return _heroesSettings.HeroStaticDataByType[heroType];
        }

        public void ChangeHeroPower(HeroType heroType, int points)
        {
            if (heroType is HeroType.AnyActive or HeroType.None)
            {
                return;
            }

            if (points > 0 && HeroSaveData[heroType].IsUnlocked == false)
            {
                return;
            }

            HeroSaveData[heroType].PowerPoints += points;
            OnHeroPointsChanged?.Invoke(heroType, HeroSaveData[heroType].PowerPoints);
        }

        public void UnlockHero(HeroType heroType)
        {
            HeroSaveData[heroType].IsUnlocked = true;
            OnHeroUnlocked?.Invoke(heroType);
        }

        private void OnFirstLoading()
        {
            PopulateSaveDataList();
            UnlockHero(HeroType.Hawk);
        }

        private void PopulateSaveDataList()
        {
            foreach (var heroStaticData in _heroesSettings.HeroStaticDataByType.Values)
            {
                var newSaveData = new HeroSaveData()
                {
                    HeroType = heroStaticData.HeroType,
                    IsUnlocked = false,
                    PowerPoints = heroStaticData.StartingPowerPoints
                };
                HeroSaveData.Add(newSaveData.HeroType, newSaveData);
            }
        }
    }
}