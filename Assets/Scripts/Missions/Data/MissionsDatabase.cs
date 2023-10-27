using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Birdgame.Missions.Data
{
    [CreateAssetMenu(fileName = "MissionsDatabase", menuName = "Settings/MissionsDatabase")]
    public class MissionsDatabase : ScriptableObject
    {
        [Header("Миссии можно править здесь, но лучше через редактор (BirdGame/Mission Graph).")]
        [SerializeField] private List<MissionStaticData> _missionsData;

        private Dictionary<string, MissionStaticData> _missionsDataById;
        public Dictionary<string, MissionStaticData> MissionsDataById
        {
            get
            {
                if (_missionsDataById == null)
                {
                    _missionsDataById = _missionsData.ToDictionary(data => data.MissionId);
                }

                return _missionsDataById;
            }
        }

#if UNITY_EDITOR
        public void SaveDataFromEditor(List<MissionStaticData> missionsData)
        {
            _missionsData = missionsData;
        }
#endif
    }
}