using System.Collections.Generic;
using System.Linq;
using Controllers;
using Domain;
using Factories;
using Game_Code.Domain;
using Scriptable_Objects;
using Services;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    private PlayersController _playersController;
    private ITurnsService _turnsService;
    private HandsController _handsController;
    private CenterDeckController _centerDeckController;
    private ISessionService _sessionService;
    private IDeckService _deckService;
    private CardsFactory _cardsFactory;
    
    [Inject]
    public void Construct(ITurnsService turnsService, HandsController handsController, 
        CenterDeckController centerDeckController, PlayersController playersController, 
        ISessionService sessionService, IDeckService deckService, CardsFactory cardsFactory)
    {
        _playersController = playersController;
        _turnsService = turnsService;
        _handsController = handsController;
        _centerDeckController = centerDeckController;
        _sessionService = sessionService;
        _deckService = deckService;
        _cardsFactory = cardsFactory;
    }

    private void Start()
    {
        _playersController.CreatePlayer(true);
        _playersController.CreatePlayer();
        _playersController.CreatePlayer();
        _playersController.CreatePlayer();

        var players = _playersController.GetAllPlayers().ToArray();
        _turnsService.SetTurnsOrder(new Queue<Player>(players));

        _centerDeckController.CreateCenterDeck();
        
        foreach (var player in players)
        {
            var cards = _centerDeckController.GetCards(3);
            _deckService.AssignDeckToPlayer(player);
            _handsController.SetHand(player, cards);
        }
        
        _turnsService.NextTurn();
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
        var player = _turnsService.CurrentTurn();

        if (!player)
        {
            _sessionService.GetSessionWinner();
            _sessionService.ClearCards();
            var players = _playersController.GetAllPlayers();
            _turnsService.SetTurnsOrder(new Queue<Player>(players));
            _turnsService.NextTurn();

            foreach (var player1 in players)
            {
                var playerDeck = _deckService.GetPlayerDeck(player1);

                for (int i = 0; i < playerDeck.selectedMapPoint.childCount; i++)
                {
                    var child = playerDeck.selectedMapPoint.GetChild(i);
                    Destroy(child.GameObject());
                }
            }
            return;
        }
        
        var hand = _handsController.GetHand(player).ToArray();
        Debug.Log($"Hand: {hand}");
        
        var card = hand[Random.Range(0, hand.Length)];
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