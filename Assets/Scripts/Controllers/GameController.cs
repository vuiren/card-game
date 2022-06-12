using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
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
        private IScoreService _scoreService;
        private IGameService _gameService;
        private Configuration _configuration;
        private CenterDeckController _centerDeckController;
        private BetsController _betsController;

        [Inject]
        public void Construct(Configuration configuration, CenterDeckController centerDeckController, CardsFactory cardsFactory, IDeckService deckService,HandsController handsController, 
            ISessionService sessionService, ITurnsService turnsService, IPlayerService playerService, IScoreService scoreService,
            IGameService gameService, BetsController betsController)
        {
            _centerDeckController = centerDeckController;
            _gameService = gameService;
            _betsController = betsController;
            _scoreService = scoreService;
            _playerService = playerService;
            _configuration = configuration;
            _cardsFactory = cardsFactory;
            _deckService = deckService;
            _handsController = handsController;
            _sessionService = sessionService;
            _turnsService = turnsService;
            _turnsService.OnTurnChange(CheckIfWinCheck);
            _sessionService.OnSessionWinnerAnnounced(AddPointToPlayer);
        }

        private void AddPointToPlayer(int obj)
        {
            _scoreService.AddPointToPlayer(obj);
        }

        private void CheckIfWinCheck(int obj)
        {
            if (obj != -1) return;
            _sessionService.AnnounceSessionWinnerId();

            _sessionService.ClearCards();
            var players = _playerService.GetAllPlayers().ToArray();

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
            
            CheckForSessionEnd();
        }
        
        private async UniTask FakeDelay()
        {
            await UniTask.Delay(1000);
        }

        private async void CheckForSessionEnd()
        {
            var players = _playerService.GetAllPlayers().ToArray();

            if (players.Select(playerData => _handsController.GetHand(playerData.id)).Any(hand => hand.Any()))
            {
                return;
            }

            if (!_configuration.isHost)
            {
                await FakeDelay();
            }

            _gameService.SetHostReady(false);

            if (_configuration.isHost)
            {
                foreach (var player in players)
                {
                    var cards = _centerDeckController.GetCards(_configuration.lastHandCount + 1);
                    _handsController.CreateHand(player.id, cards);
                }
                
                _configuration.lastHandCount++;

                await FakeDelay();
                _gameService.SetHostReady(true);
                await FakeDelay();
            }

            if (!_configuration.isHost)
                await WaitForHost();

            _handsController.CreateHand(_configuration.playerId, _handsController.GetHand(_configuration.playerId));

            _betsController.ShowUI();
        }

        private async UniTask WaitForHost()
        {
            while (!_gameService.IsHostReady())
            {
                statusText.text = "Waiting for host";
                await UniTask.Delay(1000);
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
            _cardsFactory.CreateCard(deck.selectedMapPoint, card, playerId);
            _handsController.CreateHand(playerId, newHand.Select(x=>x));
            _sessionService.AddCardToSession(playerId, card);
            _turnsService.NextTurn();
        }
    }
}