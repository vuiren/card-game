using System.Collections.Generic;
using System.Linq;
using Factories;
using Scriptable_Objects;
using Services;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Util;
using Zenject;

namespace Controllers
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI statusText;
        private BetsController _betsController;
        private CardsFactory _cardsFactory;
        private CenterDeckController _centerDeckController;
        private Configuration _configuration;
        private IDeckService _deckService;
        private IGameService _gameService;
        private HandsController _handsController;
        private IPlayerService _playerService;
        private IScoreService _scoreService;
        private ISessionService _sessionService;
        private ITurnsService _turnsService;

        [Inject]
        public void Construct(Configuration configuration, CenterDeckController centerDeckController,
            CardsFactory cardsFactory, IDeckService deckService, HandsController handsController,
            ISessionService sessionService, ITurnsService turnsService, IPlayerService playerService,
            IScoreService scoreService,
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
            if (_configuration.isHost)
                _scoreService.AddPointToPlayer(obj);
        }

        private void CheckIfWinCheck(int obj)
        {
            if (obj != -1) return;

            _sessionService.AnnounceSessionWinnerId();

            _sessionService.ClearCards();
            var players = _playerService.GetAllPlayers().ToArray();

            if (_configuration.isHost)
            {
                var playersQueue = new Queue<int>(_turnsService.GetLastTurnsOrder());
                var tempPlayer = playersQueue.Dequeue();
                playersQueue.Enqueue(tempPlayer);
                _turnsService.SetTurnsOrder(playersQueue);
            }


            foreach (var player1 in players)
            {
                var playerDeck = _deckService.GetPlayerDeck(player1.id);

                for (var i = 0; i < playerDeck.selectedMapPoint.childCount; i++)
                {
                    var child = playerDeck.selectedMapPoint.GetChild(i);
                    Destroy(child.GameObject());
                }
            }

            CheckForSessionEnd();
        }

        private async void CheckForSessionEnd()
        {
            var players = _playerService.GetAllPlayers().ToArray();

            if (players.Select(playerData => _handsController.GetHand(playerData.id)).Any(hand => hand.Any())) return;

            if (!_configuration.isHost) await Tasks.Delay();

            if (_configuration.isHost)
            {
                _gameService.SetHostReady(false);

                _centerDeckController.CreateCenterDeck();
                await Tasks.Delay();

                foreach (var player in players)
                {
                    var cards = _centerDeckController.GetCardsFromCenterDeck(_configuration.lastHandCount + 1);
                    _handsController.CreateHand(player.id, cards);
                    _betsController.MakeABet(player.id, -1);
                }

                _configuration.lastHandCount++;

                await Tasks.Delay();
                _gameService.SetHostReady(true);
                await Tasks.Delay();
            }

            if (!_configuration.isHost)
            {
                await Tasks.WaitForHost(_gameService.IsHostReady);

                _centerDeckController.SyncCenterDeck();
                var hand = _handsController.GetHand(_configuration.playerId);
                _handsController.CreateHand(_configuration.playerId, hand);
            }


            _betsController.ShowUI();
            statusText.text = "Делаем ставки, господа";

            await Tasks.WaitForBets(_playerService.GetAllPlayers, _betsController.GetPlayerBet);
            _betsController.HideUI();
        }

        public void MakeAStep(int playerId, CardSheet card)
        {
            var hand = _handsController.GetHand(playerId).ToArray();
            Debug.Log($"Hand: {hand}");

            var newHand = hand.ToList();
            newHand.Remove(card);

            Debug.Log($"New hand: {newHand}");

            _deckService.ClearPlayerDeck(playerId);
            _handsController.CreateHand(playerId, newHand.Select(x => x));
            _sessionService.AddCardToSession(playerId, card);
            _turnsService.NextTurn();
        }
    }
}