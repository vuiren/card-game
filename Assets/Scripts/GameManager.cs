using System.Collections.Generic;
using System.Linq;
using Controllers;
using Cysharp.Threading.Tasks;
using Factories;
using Scriptable_Objects;
using Services;
using TMPro;
using UnityEngine;
using Util;
using Zenject;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI statusText;
    private BetsController _betsController;
    private CenterDeckController _centerDeckController;
    private Configuration _configuration;
    private IDeckService _deckService;
    private IGameService _gameService;
    private HandsController _handsController;
    private IPlayerService _playerService;
    private ITurnsService _turnsService;
    private WaitingAnimationController _waitingAnimationController;
    private WeightsUIController _weightsUIController;
    private WeightsController _weightsController;
    private PlayerListController _playerListController;

    [Inject]
    public void Construct(Configuration configuration, ITurnsService turnsService, HandsController handsController,
        CenterDeckController centerDeckController, ISessionService sessionService, IGameService gameService,
        IPlayerService playerService, IDeckService deckService, CardsFactory cardsFactory,
        BetsController betsController, WaitingAnimationController waitingAnimationController,
        WeightsUIController weightsUIController, WeightsController weightsController, PlayerListController playerListController)
    {
        _betsController = betsController;
        _playerListController = playerListController;
        _waitingAnimationController = waitingAnimationController;
        _configuration = configuration;
        _turnsService = turnsService;
        _weightsController = weightsController;
        _weightsUIController = weightsUIController;
        _handsController = handsController;
        _centerDeckController = centerDeckController;
        _deckService = deckService;
        _playerService = playerService;
        _gameService = gameService;
    }
    
    private async void Start()
    {
        statusText.text = "Waiting for players";
        await WaitForAllPlayers();
        var players = _playerService.GetAllPlayers().ToArray();

        foreach (var player in players)
            if (player.id == _configuration.playerId)
                _deckService.AssignMainDeckToPlayer(player.id);
            else
                _deckService.AssignDeckToPlayer(player.id);

        if (_configuration.isHost)
        {
            var queue = new Queue<int>(players.Select(x => x.id));
            _turnsService.SetTurnsOrder(queue);
            _betsController.SetTurnsOrder(queue);
            _centerDeckController.CreateCenterDeck();
            await Tasks.Delay();
            foreach (var player in players)
            {
                _configuration.lastHandCount = 1;
                var cards = _centerDeckController.GetCardsFromCenterDeck(1);
                _handsController.CreateHand(player.id, cards);
                _betsController.MakeABet(player.id, 0);
            }

            _gameService.SetHostReady(true);
        }

        statusText.text = "Waiting for host";
        await Tasks.WaitForHost(_gameService.IsHostReady);

        if (!_configuration.isHost)
        {
            _centerDeckController.SyncCenterDeck();
            var hand = _handsController.GetHand(_configuration.playerId);
            _handsController.CreateHand(_configuration.playerId, hand);
        }

        _weightsController.SetRightWeight(_configuration.lastHandCount);
        _weightsUIController.ShowUI();
        statusText.text = "Делаем ставки, господа";
        await Tasks.Delay();
        await Tasks.WaitForBets(_playerService.GetAllPlayers, _betsController.BetsSet);
        _waitingAnimationController.HideWaitingAnimation();
        _weightsUIController.HideUI();
        _playerListController.UpdateCurrentTurn(_turnsService.CurrentTurn());
    }

    private void OnDestroy()
    {
        if (_configuration.isHost) _gameService.DeleteGameData();
    }

    private async UniTask WaitForAllPlayers()
    {
        while (_playerService.GetAllPlayers().Count() < _configuration.playersCount)
        {
            await UniTask.Delay(1000);
            statusText.text =
                $"Waiting for players: {_playerService.GetAllPlayers().Count()}/{_configuration.playersCount}";
        }
    }
}