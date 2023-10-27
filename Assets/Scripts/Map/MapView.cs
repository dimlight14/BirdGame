using System;
using System.Collections.Generic;
using Birdgame.Missions.Data;
using UnityEngine;

namespace Birdgame.Missions.Map
{
    public class MapView : MonoBehaviour
    {
        [SerializeField] private MissionPointView _missionPointViewReference;
        [SerializeField] private Transform _pointsParent;

        private readonly Dictionary<string, MissionPointView> _missionPointViews = new();

        public void CreateMissionPoint(string missionId, MissionState missionState, Vector2 position, Action<string> onMissionClicked)
        {
            var newPoint = CreateNewPointView();
            newPoint.gameObject.GetComponent<RectTransform>().anchoredPosition = position;
            newPoint.SetVisuals(missionId, missionState, onMissionClicked);
            _missionPointViews.Add(missionId, newPoint);
        }

        public bool HasPointForMission(string missionId)
        {
            return _missionPointViews.ContainsKey(missionId);
        }

        public void ChangeMissionState(string missionId, MissionState newState)
        {
            _missionPointViews[missionId].ChangeState(newState);
        }

        private MissionPointView CreateNewPointView()
        {
            return Instantiate(_missionPointViewReference, _pointsParent);
        }
    }
}