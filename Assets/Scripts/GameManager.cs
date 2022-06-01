using System;
using System.Collections.Generic;
using System.Linq;
using Controllers;
using Game_Code.Domain;
using Services;
using UnityEngine;
using Zenject;

public class GameManager : MonoBehaviour
{
    private PlayersController _playersController;
    private ITurnsService _turnsService;
    private HandsController _handsController;
    private CenterDeckController _centerDeckController;
    
    [Inject]
    public void Construct(ITurnsService turnsService, HandsController handsController, 
        CenterDeckController centerDeckController, PlayersController playersController)
    {
        _playersController = playersController;
        _turnsService = turnsService;
        _handsController = handsController;
        _centerDeckController = centerDeckController;
    }

    private void Start()
    {
        _playersController.CreatePlayer();
        _playersController.CreatePlayer();
        _playersController.CreatePlayer();
        _playersController.CreatePlayer();

        var players = _playersController.GetAllPlayers().ToArray();
        _turnsService.SetTurnsOrder(new Queue<Player>(players));

        _centerDeckController.CreateCenterDeck();
        
        foreach (var player in players)
        {
            var cards = _centerDeckController.GetCards(3);
            _handsController.SetHand(player, cards);
        }
        
        _turnsService.NextTurn();
    }
}