using System;
using UnityEngine;
using UnityEngine.UI;

namespace Birdgame
{
    public class RaycastBlocker : MonoBehaviour
    {
        [SerializeField] private Button _button;

        private Action _onButtonClick;

        private void Start()
        {
            _button.onClick.AddListener(ButtonClick);
        }

        public void Activate(Action onButtonClick = null)
        {
            _onButtonClick = onButtonClick;
            _button.interactable = onButtonClick != null;
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            _onButtonClick = null;
        }

        private void ButtonClick()
        {
            _onButtonClick?.Invoke();
        }
    }
}