using Domain;
using Scriptable_Objects;
using UnityEngine;
using Zenject;

namespace Factories
{
    public class CardsFactory
    {
        private readonly Configuration _configuration;

        public CardsFactory(Configuration configuration)
        {
            _configuration = configuration;
        }

        
        public Card CreateCard(Transform parent, CardSheet cardSheet)
        {
            var card = Object.Instantiate(_configuration.cardPrefab, parent).GetComponent<Card>();
            card.cardSheet = cardSheet;
            card.UpdateCard();
            return card;
        }
    }
}