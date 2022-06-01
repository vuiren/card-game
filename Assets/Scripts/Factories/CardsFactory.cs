using Domain;
using Scriptable_Objects;
using UnityEngine;
using Zenject;

namespace Factories
{
    public class CardsFactory: MonoBehaviour
    {
        private Configuration _configuration;
        
        [Inject]
        public void Construct(Configuration configuration)
        {
            _configuration = configuration;
        }
        
        public Card CreateCard(Transform parent, CardSheet cardSheet)
        {
            var card = Instantiate(_configuration.cardPrefab, parent).GetComponent<Card>();
            card.cardSheet = cardSheet;
            card.UpdateCard();
            return card;
        }
    }
}