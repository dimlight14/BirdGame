using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Birdgame.Heroes.Views
{
    public class HeroCardView : MonoBehaviour
    {
        private const string POWER_TEXT_FIRST_PART = "Сила: ";
        
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _powerText;
        [SerializeField] private Button _button;
        [SerializeField] private GameObject _selectedImage;

        private Action<HeroType> _selectHeroAction;
        private HeroType _heroType;

        private void Start()
        {
            _button.onClick.AddListener(OnClick);
            _button.interactable = false;
        }

        public void SetVisuals(string heroName, HeroType heroType, int heroPower, Action<HeroType> selectHero)
        {
            _nameText.text = heroName;
            _heroType = heroType;
            _selectHeroAction = selectHero;
            SetPowerText(heroPower);
            _selectedImage.SetActive(false);
        }
        public void SetPowerText(int power)
        {
            _powerText.text = POWER_TEXT_FIRST_PART + power.ToString();
        }

        public void SetButtonEnabled(bool isEnabled)
        {
            _button.interactable = isEnabled;
        }
        

        private void OnClick()
        {
            _selectHeroAction?.Invoke(_heroType);
        }

        public void SetSelectionIndication(bool on)
        {
            _selectedImage.SetActive(on);
        }
    }
}