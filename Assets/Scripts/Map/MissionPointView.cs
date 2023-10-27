using System;
using Birdgame.Missions.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Birdgame.Missions.Map
{
    public class MissionPointView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _missionNumber;
        [SerializeField] private Button _button;

        private Action<string> _onClickedAction;
        private string _missionId;

        private void Start()
        {
            _button.onClick.AddListener(OnClick);
        }

        public void SetVisuals(string missionId, MissionState missionState, Action<string> onClickedAction)
        {
            _missionId = missionId;
            _onClickedAction = onClickedAction;
            _missionNumber.text = missionId.Substring(0, 1);
            ChangeState(missionState);
        }

        public void ChangeState(MissionState missionState)
        {
            _button.interactable = missionState == MissionState.Active;
        }

        private void OnClick()
        {
            _onClickedAction?.Invoke(_missionId);
        }
    }
}