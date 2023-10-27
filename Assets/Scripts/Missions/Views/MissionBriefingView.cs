using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Birdgame.Missions.Views
{
    public class MissionBriefingView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _titleName;
        [SerializeField] private TextMeshProUGUI _description;
        [SerializeField] private Button _startButton;
        [SerializeField] private Canvas _canvas;

        private Action<string> _onButtonClick;
        private string _currentMissionId;

        private void Start()
        {
            _startButton.onClick.AddListener(OnButtonClick);
        }

        public void Show(string missionId, string titleText, string descriptionText, Action<string> onButtonClick)
        {
            _currentMissionId = missionId;
            _titleName.text = titleText;
            _description.text = descriptionText;
            _onButtonClick = onButtonClick;
            _canvas.enabled = true;
        }

        public void Hide()
        {
            _canvas.enabled = false;
        }

        private void OnButtonClick()
        {
            _onButtonClick?.Invoke(_currentMissionId);
        }
    }
}