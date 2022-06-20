using Controllers;
using Domain;
using Scriptable_Objects;
using Services;
using UnityEngine;
using Zenject;

namespace Factories
{
    public class CardsFactory
    {
        private readonly Configuration _configuration;
        private readonly DiContainer _diContainer;

        public CardsFactory(Configuration configuration, DiContainer container)
        {
            _configuration = configuration;
            _diContainer = container;
        }


        public Card CreateCard(Transform parent, CardSheet cardSheet, int ownerId, bool active)
        {
            var card = Object.Instantiate(_configuration.cardPrefab, parent).GetComponent<Card>();
            card.Construct(ownerId, active, _diContainer.Resolve<GameController>(),
                _diContainer.Resolve<ITurnsService>(), _diContainer.Resolve<IPlayerService>());
            card.cardSheet = cardSheet;
            card.UpdateCard();

            return card;
        }
    }
}