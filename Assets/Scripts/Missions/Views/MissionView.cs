using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Birdgame.Missions.Views
{
    public class MissionView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _titleText;
        [SerializeField] private TextMeshProUGUI _ourSideText;
        [SerializeField] private TextMeshProUGUI _theirSideText;
        [SerializeField] private TextMeshProUGUI _description;
        [SerializeField] private Button _button;
        [SerializeField] private Canvas _canvas;

        private Action<string> _onButtonClick;
        private string _currentMissionId;

        private void Start()
        {
            _button.onClick.AddListener(OnButtonClick);
        }

        public void Show(string missionId, string titleText, string descriptionText, string ourSide, string theirSide, Action<string> onButtonClick)
        {
            _currentMissionId = missionId;
            _titleText.text = titleText;
            _description.text = descriptionText;
            _ourSideText.text = ourSide;
            _theirSideText.text = theirSide;
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