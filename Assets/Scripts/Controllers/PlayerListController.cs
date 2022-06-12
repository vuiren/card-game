using System.Collections.Generic;
using Domain;
using Domain.DTO;
using Services;
using TMPro;
using UnityEngine;
using Zenject;

namespace Controllers
{
    public class PlayerListController: MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI playersList, currentTurnText;
        private IPlayerService _playerService;
        private Coroutine _listUpdateRoutine;
        
        [Inject]
        public void Construct(IPlayerService playerService, ITurnsService turnsService)
        {
            _playerService = playerService;
            _playerService.OnPlayerListChanged(UpdateList);
            
            turnsService.OnTurnChange(UpdateCurrentTurn);
        }

        private void UpdateCurrentTurn(int obj)
        {
            var player = _playerService.GetPlayer(obj);
            if (player == null)
            {
                currentTurnText.text = "Подводим результаты";
            }
            else
            {
                currentTurnText.text = $"Ход игрока: {player.name}";
            }
        }

        private void UpdateList(List<PlayerData> obj)
        {
            playersList.text = "Игроки:\n";
            for (var index = 0; index < obj.Count; index++)
            {
                var playerData = obj[index];
                playersList.text += (index + 1) + ". " + playerData.name + '\n';
            }
        }
    }
}