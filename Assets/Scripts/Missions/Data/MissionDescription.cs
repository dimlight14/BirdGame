using UnityEngine;

namespace Birdgame.Missions.Data
{
    [CreateAssetMenu(fileName = "MissionDescription", menuName = "Settings/MissionDescription")]
    public class MissionDescription : ScriptableObject
    {
        public string Title;
        [TextArea(5, 12)]
        public string IntroductoryDescription;
        [TextArea(5, 12)]
        public string MainDescription;
        public string OurSide;
        public string TheirSide;
    }
}