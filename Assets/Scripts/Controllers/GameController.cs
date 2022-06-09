using System.Linq;
using Domain;
using Factories;
using Game_Code.Domain;
using Services;
using UnityEngine;
using Zenject;

namespace Controllers
{
    public class GameController : MonoBehaviour
    {
        private CardsFactory _cardsFactory;
        private IDeckService _deckService;
        private HandsController _handsController;
        private ISessionService _sessionService;
        private ITurnsService _turnsService;

        [Inject]
        public void Construct(CardsFactory cardsFactory, IDeckService deckService,HandsController handsController, ISessionService sessionService,
            ITurnsService turnsService )
        {
            _cardsFactory = cardsFactory;
            _deckService = deckService;
            _handsController = handsController;
            _sessionService = sessionService;
            _turnsService = turnsService;
        }
        
        public void MakeAStep(Player player, Card card)
        {
            var hand = _handsController.GetHand(player).ToArray();
            Debug.Log($"Hand: {hand}");
        
            var newHand = hand.ToList();
            newHand.Remove(card);
        
            Debug.Log($"New hand: {newHand}");

            var deck = _deckService.GetPlayerDeck(player);
            _deckService.ClearDeck(_deckService.GetPlayerDeck(player).actor.id);
            _cardsFactory.CreateCard(deck.selectedMapPoint, card.cardSheet);
            _handsController.SetHand(player, newHand.Select(x=>x.cardSheet));
            _sessionService.AddCardToSession(player.actor.id, card.cardSheet);
            _turnsService.NextTurn();
        }
    }
}