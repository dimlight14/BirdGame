using System;
using System.Collections.Generic;
using UnityEngine;

namespace Birdgame.Heroes.Views
{
    public class HeroesDeckView : MonoBehaviour
    {
        [SerializeField] private HeroCardView _heroCardViewReference;
        [SerializeField] private Transform _heroCardViewsParent;

        private Dictionary<HeroType, HeroCardView> _heroCardViews = new();

        public void CreateHeroCard(HeroType heroType, string heroName, int heroPower, Action<HeroType> selectHero)
        {
            var newCardView = CreateNewView();
            newCardView.SetVisuals(heroName, heroType, heroPower, selectHero);
            _heroCardViews.Add(heroType, newCardView);
        }

        public void UpdateHeroPower(HeroType heroType, int power)
        {
            if (_heroCardViews.ContainsKey(heroType))
            {
                _heroCardViews[heroType].SetPowerText(power);
            }
        }

        private HeroCardView CreateNewView()
        {
            return Instantiate(_heroCardViewReference, _heroCardViewsParent);
        }

        public void EnableButtons()
        {
            foreach (var cardView in _heroCardViews.Values)
            {
                cardView.SetButtonEnabled(true);
            }
        }

        public void DisableButtons()
        {
            foreach (var cardView in _heroCardViews.Values)
            {
                cardView.SetButtonEnabled(false);
            }
        }

        public void SetSelectionIndicationForHero(HeroType heroType)
        {
            _heroCardViews[heroType].SetSelectionIndication(true);
        }

        public void RemoveSelectionIndication()
        {
            foreach (var heroCardView in _heroCardViews.Values)
            {
                heroCardView.SetSelectionIndication(false);
            }
        }
    }
}