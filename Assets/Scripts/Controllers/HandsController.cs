using System.Collections.Generic;
using System.Linq;
using Domain;
using Extensions;
using Factories;
using Game_Code.Domain;
using Scriptable_Objects;
using Services;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Controllers
{
    public class HandsController : MonoBehaviour
    {
        [SerializeField] private GameObject cardPrefab;
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

        public void SetHand(Player player, IEnumerable<CardSheet> cards)
        {
            var deck = _deckService.GetPlayerDeck(player);
            _deckService.ClearDeck(deck.actor.id);
            
            var hand = cards
                .Select(cardSheet => _cardsFactory.CreateCard(deck.hand, cardSheet))
                .ToList();

            deck.SetCards(hand.ToArray());
            deck.StructureHand();
            _handsService.SetHand(player, hand);
        }
        
        public void SetRandomHandForPlayer(Player player, int cardsCount)
        {
            var hand = new List<Card>();
            var deck = _deckService.GetPlayerDeck(player);
            
            for (var i = 0; i < cardsCount; i++)
            {
                var random = Random.Range(0, _configuration.cardsInGame.Length);
                var cardSheet = _configuration.cardsInGame[random];
                var card = _cardsFactory.CreateCard(deck.hand, cardSheet);
                hand.Add(card);
            }
            
            deck.StructureHand();
            _handsService.SetHand(player, hand);
        }

        public IEnumerable<Card> GetHand(Player player)
        {
            return _handsService.GetHand(player);
        }
    }
}