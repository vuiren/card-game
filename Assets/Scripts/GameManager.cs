using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Controllers;
using Cysharp.Threading.Tasks;
using Domain;
using Factories;
using Scriptable_Objects;
using Services;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI statusText;
    private ITurnsService _turnsService;
    private HandsController _handsController;
    private CenterDeckController _centerDeckController;
    private ISessionService _sessionService;
    private IDeckService _deckService;
    private CardsFactory _cardsFactory;
    private IPlayerService _playerService;
    private IGameService _gameService;
    private Configuration _configuration;
    
    [Inject]
    public void Construct(Configuration configuration, ITurnsService turnsService, HandsController handsController, 
        CenterDeckController centerDeckController, ISessionService sessionService, IGameService gameService,
       IPlayerService playerService, IDeckService deckService, CardsFactory cardsFactory)
    {
        _configuration = configuration;
        _turnsService = turnsService;
        _handsController = handsController;
        _centerDeckController = centerDeckController;
        _sessionService = sessionService;
        _deckService = deckService;
        _cardsFactory = cardsFactory;
        _playerService = playerService;
        _gameService = gameService;
    }

    private async void Start()
    {
        statusText.text = "Waiting for players";
        await WaitForAllPlayers();
       // statusText.text = "Game Started";
        var players = _playerService.GetAllPlayers().ToArray();
        
        foreach (var player in players)
        {
            if (player.id == _configuration.playerId)
            {
                _deckService.AssignMainDeckToPlayer(player.id);
            }
            else
            {
                _deckService.AssignDeckToPlayer(player.id);
            }
        }

        if (_configuration.isHost)
        {
            _turnsService.SetTurnsOrder(new Queue<int>(players.Select(x => x.id)));
            _centerDeckController.CreateCenterDeck();
            await FakeDelay();
            foreach (var player in players)
            {
                var cards = _centerDeckController.GetCards(3);
                _handsController.CreateHand(player.id, cards);
            }

            _gameService.SetHostReady();
        }

        await WaitForHost();

        if (!_configuration.isHost)
        {
            _centerDeckController.SyncCenterDeck();
        }

        _handsController.CreateHand(_configuration.playerId, _handsController.GetHand(_configuration.playerId));
    }

    private async UniTask FakeDelay()
    {
        await UniTask.Delay(1000);
    }

    private async UniTask WaitForHost()
    {
        while (!_gameService.IsHostReady())
        {
            statusText.text = "Waiting for host";
            await UniTask.Delay(1000);
        }
    }

    private async UniTask WaitForAllPlayers()
    {
        while (_playerService.GetAllPlayers().Count() < _configuration.playersCount)
        {
            await UniTask.Delay(1000);
            statusText.text = $"Waiting for players: {_playerService.GetAllPlayers().Count()}/{_configuration.playersCount}";
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            MakeAStep();
        }
    }

    private void MakeAStep()
    {
        var playerId = _turnsService.CurrentTurn();

        if (playerId == -1)
        {
            _sessionService.GetSessionWinnerId();
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
            return;
        }

        var hand = _handsController.GetHand(playerId).ToArray();
        Debug.Log($"Hand: {hand}");
        
        var card = hand[Random.Range(0, hand.Length)];
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