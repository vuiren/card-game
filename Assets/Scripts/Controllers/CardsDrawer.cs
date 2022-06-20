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
    /// <summary>
    ///     Рисует карты, которые выбрали игроки
    /// </summary>
    public class CardsDrawer : MonoBehaviour
    {
        private CardsFactory _cardsFactory;
        private ICenterDeckService _centerDeckService;
        private Configuration _configuration;
        private IDeckService _deckService;
        private ISessionService _sessionService;
        private LocalPlayerCardsController _localPlayerCardsController;

        [Inject]
        public void Construct(Configuration configuration, CardsFactory cardsFactory, IDeckService deckService,
            ISessionService sessionService, ICenterDeckService centerDeckService, LocalPlayerCardsController localPlayerCardsController)
        {
            _centerDeckService = centerDeckService;
            _cardsFactory = cardsFactory;
            _configuration = configuration;
            _deckService = deckService;
            _sessionService = sessionService;
            _localPlayerCardsController = localPlayerCardsController;

            _sessionService.OnCardAddedToSession(RedrawCards);
        }

        private void RedrawCards(List<SessionPlayer> obj)
        {
            foreach (var sessionPlayer in obj)
            {
                var deck = _deckService.GetPlayerDeck(sessionPlayer.playerId);
                var card = _configuration.cardsInGame.FirstOrDefault(x => x.cardId == sessionPlayer.playedCardId);
                _cardsFactory.CreateCard(deck.selectedMapPoint, card, sessionPlayer.playerId, true);
            }

            DrawLocalPlayerCards(_localPlayerCardsController.GetLocalPlayerCards());
        }

        private void DrawLocalPlayerCards(IEnumerable<Card> cards)
        {
            foreach (var card in cards) card.SetCardActive(false);

            var cardsInSession = _sessionService.GetCardsInSession();

            if (cardsInSession.Count == 0)
            {
                foreach (var card in cards) card.SetCardActive(true);
                return;
            }

            var firstCard = _configuration.cardsInGame.FirstOrDefault(x => x.cardId == cardsInSession[0].playedCardId);
            if (!firstCard)
            {
                Debug.LogError($"Error, first card in session not found, card id: '{firstCard}'");
                return;
            }

            var anyWithSameSuit = cards.Any(x => x.cardSheet.cardSuit == firstCard.cardSuit);
            if (anyWithSameSuit)
            {
                var sameSuitCards = cards.Where(x => x.cardSheet.cardSuit == firstCard.cardSuit);
                foreach (var sameSuitCard in sameSuitCards) sameSuitCard.SetCardActive(true);
                return;
            }

            var trumpCard = _centerDeckService.GetTrumpCard();
            if (trumpCard == null)
            {
                Debug.LogError("No trump card found");
                return;
            }

            var anyWithTrumpSuit = cards.Any(x => x.cardSheet.cardSuit == trumpCard.cardSuit);
            if (anyWithTrumpSuit)
            {
                var sameSuitCards = cards.Where(x => x.cardSheet.cardSuit == trumpCard.cardSuit);
                foreach (var sameSuitCard in sameSuitCards) sameSuitCard.SetCardActive(true);
                return;
            }

            foreach (var card in cards) card.SetCardActive(true);
        }
    }
}