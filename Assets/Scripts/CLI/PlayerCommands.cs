using QFSW.QC;
using Services;
using UnityEngine;
using Zenject;

namespace CLI
{
    public class PlayerCommands : MonoBehaviour
    {
        private IPlayerService _playerService;

        [Inject]
        public void Construct(IPlayerService playerService)
        {
            _playerService = playerService;
        }

        [Command("players.getPlayer")]
        public void GetPlayer(int id)
        {
            var player = _playerService.GetPlayer(id);
            if (player == null) return;

            Debug.Log($"Player found: '{player}'");
        }

        [Command("players.printAll")]
        public void PrintAll()
        {
            var players = _playerService.GetAllPlayers();

            foreach (var player in players) Debug.Log($"Player: '{player}'");
        }
    }
}