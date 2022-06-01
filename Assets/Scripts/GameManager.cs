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

    [Inject]
    public void Construct(ITurnsService turnsService, HandsController handsController, PlayersController playersController)
    {
        _playersController = playersController;
        _turnsService = turnsService;
        _handsController = handsController;
    }

    private void Start()
    {
        _playersController.CreatePlayer();
        _playersController.CreatePlayer();
        _playersController.CreatePlayer();
        _playersController.CreatePlayer();

        var players = _playersController.GetAllPlayers().ToArray();
        _turnsService.SetTurnsOrder(new Queue<Player>(players));

        foreach (var player in players)
        {
            _handsController.SetRandomHandForPlayer(player, 3);
        }
        
        _turnsService.NextTurn();
    }
}