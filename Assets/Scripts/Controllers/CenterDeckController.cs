using System.Collections.Generic;
using System.Linq;
using Domain;
using Extensions;
using Factories;
using Scriptable_Objects;
using Services;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

namespace Controllers
{
    public class CenterDeckController : MonoBehaviour
    {
        private CenterDeck _centerDeck;
        private Configuration _configuration;
        private List<CardSheet> _cardsInGame;
        private CardsFactory _cardsFactory;
        private ICenterDeckService _centerDeckService;

        [Inject]
        public void Construct(Configuration configuration, CardsFactory cardsFactory,
            ICenterDeckService centerDeckService)
        {
            _configuration = configuration;
            _centerDeck = FindObjectOfType<CenterDeck>();
            _cardsFactory = cardsFactory;
            _centerDeckService = centerDeckService;
        }

        public void CreateCenterDeck()
        {
            _cardsInGame = _configuration.cardsInGame.ToList();
            _cardsInGame.Shuffle();

            _centerDeckService.SetCards(_cardsInGame);
            var card = GetCards(1)[0];
            var cardInstance = _cardsFactory.CreateCard(_centerDeck.trumpCardRoot, card);
            _centerDeckService.SetTrumpCard(cardInstance.GetComponent<Card>().cardSheet.cardId);
        }

        public void SyncCenterDeck()
        {
            var card = _centerDeckService.GetTrumpCard();
            var cardInstance = _cardsFactory.CreateCard(_centerDeck.trumpCardRoot, card);
            _centerDeckService.SetTrumpCard(cardInstance.GetComponent<Card>().cardSheet.cardId);
        }

        public CardSheet[] GetCards(int cardsCount)
        {
            return _centerDeckService.GetCards(cardsCount);
        }
    }
}