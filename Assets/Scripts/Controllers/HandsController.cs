using System.Collections.Generic;
using System.Linq;
using Domain;
using Extensions;
using Factories;
using Scriptable_Objects;
using Services;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Controllers
{
    public class HandsController : MonoBehaviour
    {
        private Configuration _configuration;
        private IHandsService _handsService;
        private CardsFactory _cardsFactory;
        private IDeckService _deckService;
        
        [Inject]
        public void Construct(Configuration configuration, IHandsService handsService, 
            CardsFactory cardsFactory, IDeckService deckService)
        {
            _configuration = configuration;
            _handsService = handsService;
            _cardsFactory = cardsFactory;
            _deckService = deckService;
        }

        public void CreateHand(int playerId, IEnumerable<CardSheet> cards)
        {
            var deck = _deckService.GetPlayerDeck(playerId);
            _deckService.ClearPlayerDeck(playerId);
            
            var hand = cards
                .Select(cardSheet => _cardsFactory.CreateCard(deck.hand, cardSheet, playerId))
                .ToList();

            deck.SetCards(hand.ToArray());
            deck.StructureHand();
            _handsService.SetHand(playerId, hand.Select(x=>x.cardSheet));
        }
        
        public void SetRandomHandForPlayer(int playerId, int cardsCount)
        {
            var hand = new List<Card>();
            var deck = _deckService.GetPlayerDeck(playerId);
            
            for (var i = 0; i < cardsCount; i++)
            {
                var random = Random.Range(0, _configuration.cardsInGame.Length);
                var cardSheet = _configuration.cardsInGame[random];
                var card = _cardsFactory.CreateCard(deck.hand, cardSheet, playerId);
                hand.Add(card);
            }
            
            deck.StructureHand();
            _handsService.SetHand(playerId, hand.Select(x=>x.cardSheet));
        }

        public IEnumerable<CardSheet> GetHand(int playerId)
        {
            return _handsService.GetHand(playerId);
        }
    }
}