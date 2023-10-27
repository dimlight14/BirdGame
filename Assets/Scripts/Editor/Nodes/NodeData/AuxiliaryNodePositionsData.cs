using System.Collections.Generic;
using UnityEngine;

namespace Birdgame.Editor
{
    [CreateAssetMenu(fileName = "AuxiliaryNodePositionData", menuName = "Settings/AuxiliaryNodePositionData")]
    public class AuxiliaryNodePositionsData : ScriptableObject
    {
        public List<NodePositionData> NodePositionsData;
    }
}