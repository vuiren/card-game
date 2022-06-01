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

        private PlayerDeck[] _decks;
        private Queue<PlayerDeck> _decksQueue;


        [Inject]
        public void Construct(Configuration configuration, IHandsService handsService, CardsFactory cardsFactory)
        {
            _configuration = configuration;
            _handsService = handsService;
            _cardsFactory = cardsFactory;
            _decks = FindObjectsOfType<PlayerDeck>();
            _decksQueue = new Queue<PlayerDeck>(_decks);
        }

        public void SetHand(Player player, IEnumerable<CardSheet> cards)
        {
            var deck = _decksQueue.Dequeue();

            var hand = cards
                .Select(cardSheet => _cardsFactory.CreateCard(deck.hand, cardSheet))
                .ToList();

            deck.StructureHand();
            _handsService.SetHand(player, hand);
        }
        
        public void SetRandomHandForPlayer(Player player, int cardsCount)
        {
            var hand = new List<Card>();
            var deck = _decksQueue.Dequeue();
            
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