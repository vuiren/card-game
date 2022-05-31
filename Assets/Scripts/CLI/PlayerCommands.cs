using Controllers;
using QFSW.QC;
using Services;
using UnityEngine;
using Zenject;

namespace CLI
{
    public class PlayerCommands : MonoBehaviour
    {
        private IPlayerService _playerService;
        private PlayersController _playersController;

        [Inject]
        public void Construct(IPlayerService playerService, PlayersController playersController)
        {
            _playerService = playerService;
            _playersController = playersController;
        }

        [Command("player.create")]
        public void CreatePlayer()
        {
            _playersController.CreatePlayer();
            Debug.Log("Player created");
        }

        [Command("players.getPlayer")]
        public void GetPlayer(int id)
        {
            var player = _playerService.GetPlayer(id);
            if(!player) return;
            
            Debug.Log($"Player found: '{player}'");
        }

        [Command("players.printAll")]
        public void PrintAll()
        {
            var lastId = 0;

            while (_playerService.GetPlayer(lastId))
            {
                lastId++;
            }
        }
    }
}