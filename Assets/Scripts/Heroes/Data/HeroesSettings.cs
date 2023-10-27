using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Birdgame.Heroes.Data
{
    [CreateAssetMenu(fileName = "HeroesSettings", menuName = "Settings/HeroesSettings")]
    public class HeroesSettings : ScriptableObject
    {
        [SerializeField] private List<HeroStaticData> _heroStaticData;
        
        private Dictionary<HeroType,HeroStaticData> _heroStaticDataByType;

        public Dictionary<HeroType, HeroStaticData> HeroStaticDataByType
        {
            get
            {
                if (_heroStaticDataByType == null)
                {
                    _heroStaticDataByType = _heroStaticData.ToDictionary(data => data.HeroType);
                }
                return _heroStaticDataByType;
            }
        }
    }
}