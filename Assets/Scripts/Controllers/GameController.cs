using System.Collections.Generic;
using System.Linq;
using Domain;
using Factories;
using Scriptable_Objects;
using Services;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

namespace Controllers
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI statusText;
        private CardsFactory _cardsFactory;
        private IDeckService _deckService;
        private HandsController _handsController;
        private ISessionService _sessionService;
        private ITurnsService _turnsService;
        private IPlayerService _playerService;

        [Inject]
        public void Construct(CardsFactory cardsFactory, IDeckService deckService,HandsController handsController, 
            ISessionService sessionService, ITurnsService turnsService, IPlayerService playerService )
        {
            _playerService = playerService;
            _cardsFactory = cardsFactory;
            _deckService = deckService;
            _handsController = handsController;
            _sessionService = sessionService;
            _turnsService = turnsService;
            _turnsService.OnTurnChange(CheckIfWinCheck);
        }

        private void CheckIfWinCheck(int obj)
        {
            if (obj == -1)
            {
                var sessionWinnerId = _sessionService.GetSessionWinnerId();

                _sessionService.ClearCards();
                var players = _playerService.GetAllPlayers().ToArray();

                var winner = players.FirstOrDefault(x => x.id == sessionWinnerId);
                statusText.text = $"Игрок: {winner.name} победил!";
                
                _turnsService.SetTurnsOrder(new Queue<int>(players.Select(x=>x.id)));

                foreach (var player1 in players)
                {
                    var playerDeck = _deckService.GetPlayerDeck(player1.id);

                    for (int i = 0; i < playerDeck.selectedMapPoint.childCount; i++)
                    {
                        var child = playerDeck.selectedMapPoint.GetChild(i);
                        Destroy(child.GameObject());
                    }
                }
            }
        }

        public void MakeAStep(int playerId, CardSheet card)
        {
            var hand = _handsController.GetHand(playerId).ToArray();
            Debug.Log($"Hand: {hand}");
        
            var newHand = hand.ToList();
            newHand.Remove(card);
        
            Debug.Log($"New hand: {newHand}");

            var deck = _deckService.GetPlayerDeck(playerId);
            _deckService.ClearPlayerDeck(playerId);
            _cardsFactory.CreateCard(deck.selectedMapPoint, card);
            _handsController.CreateHand(playerId, newHand.Select(x=>x));
            _sessionService.AddCardToSession(playerId, card);
            _turnsService.NextTurn();
        }
    }
}