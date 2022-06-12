using System.Collections.Generic;
using System.Linq;
using Domain;
using Domain.DTO;
using Factories;
using Scriptable_Objects;
using Services;
using UnityEngine;
using Zenject;

namespace Controllers
{
    public class CardsDrawer : MonoBehaviour
    {
        private IDeckService _deckService;
        private ISessionService _sessionService;
        private Configuration _configuration;
        private CardsFactory _cardsFactory;
        
        [Inject]
        public void Construct(Configuration configuration, CardsFactory cardsFactory, IDeckService deckService, 
            ISessionService sessionService)
        {
            _cardsFactory = cardsFactory;
            _configuration = configuration;
            _deckService = deckService;
            _sessionService = sessionService;
            
            _sessionService.OnCardAddedToSession(RedrawCards);
        }

        private void RedrawCards(List<SessionPlayer> obj)
        {
            foreach (var sessionPlayer in obj)
            {
                var deck = _deckService.GetPlayerDeck(sessionPlayer.playerId);
                var card = _configuration.cardsInGame.FirstOrDefault(x => x.cardId == sessionPlayer.playedCardId);
                _cardsFactory.CreateCard(deck.selectedMapPoint, card);
            }
        }
    }
}