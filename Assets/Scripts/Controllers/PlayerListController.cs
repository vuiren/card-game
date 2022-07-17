using System.Collections.Generic;
using System.Linq;
using Domain.DTO;
using Scriptable_Objects;
using Services;
using TMPro;
using UnityEngine;
using Zenject;

namespace Controllers
{
    public class PlayerListController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI playersList, currentTurnText;
        private Coroutine _listUpdateRoutine;
        private IPlayerService _playerService;
        private Configuration _configuration;

        [Inject]
        public void Construct(Configuration configuration, IPlayerService playerService, ITurnsService turnsService)
        {
            _configuration = configuration;
            _playerService = playerService;
            _playerService.OnPlayerListChanged(UpdateList);

            turnsService.OnTurnChange(UpdateCurrentTurn);
        }

        public void UpdateCurrentTurn(int obj)
        {
            var player = _playerService.GetPlayer(obj);
            if (player == null)
            {
                currentTurnText.text = "Подводим результаты";
                currentTurnText.color = Color.white;
            }
            else
            {
                currentTurnText.text = $"Ход игрока: {player.name}";
                var color = _configuration.PlayerColors.FirstOrDefault(x => x.playerId == obj);
                if(color == null) return;
                currentTurnText.color = color.Color;
            }
        }

        private void UpdateList(List<PlayerData> obj)
        {
            playersList.text = "Игроки:\n";
            for (var index = 0; index < obj.Count; index++)
            {
                var playerData = obj[index];
                playersList.text += index + 1 + ". " + playerData.name + '\n';
                
            }
        }
    }
}