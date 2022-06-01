using System.Collections.Generic;
using Domain;
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

        private PlayerDeck[] _decks;
        private Queue<PlayerDeck> _decksQueue;
        
        [Inject]
        public void Construct(Configuration configuration, IHandsService handsService)
        {
            _configuration = configuration;
            _handsService = handsService;
            _decks = FindObjectsOfType<PlayerDeck>();
            _decksQueue = new Queue<PlayerDeck>(_decks);
        }

        public void SetRandomHandForPlayer(Player player, int cardsCount)
        {
            var hand = new List<Card>();
            var deck = _decksQueue.Dequeue();
            Debug.Log(deck);
            for (var i = 0; i < cardsCount; i++)
            {
                var random = Random.Range(0, _configuration.cardsInGame.Length);
                var cardSheet = _configuration.cardsInGame[random];
                var card = Instantiate(cardPrefab, deck.hand).GetComponent<Card>();
                card.cardSheet = cardSheet;
                card.UpdateCard();
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