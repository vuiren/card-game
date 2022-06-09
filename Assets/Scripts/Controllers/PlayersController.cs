using System.Collections;
using System.Collections.Generic;
using Game_Code.Domain;
using Services;
using UnityEngine;
using Zenject;

namespace Controllers
{
    public class PlayersController : MonoBehaviour
    {
        [SerializeField] private GameObject playerPrefab;
        private IPlayerService _playerService;
        private IActorsIdService _actorsIdService;
        private GameObject _playersParent;
        
        [Inject]
        public void Construct(IPlayerService playerService, IActorsIdService actorsIdService)
        {
            _playerService = playerService;
            _actorsIdService = actorsIdService;
            
            _playersParent = new GameObject("Players");
        }
        
        public void CreatePlayer()
        {
            Debug.Log("Creating player");
            var playerInstance = Instantiate(playerPrefab, _playersParent.transform);
            var player = playerInstance.GetComponent<Player>();
            player.actor.id = _actorsIdService.GetNewId();
            _playerService.RegisterPlayer(player);
        }

        public Player GetPlayer(int id)
        {
            return _playerService.GetPlayer(id);
        }

        public IEnumerable<Player> GetAllPlayers()
        {
            return _playerService.GetAllPlayers();
        }
    }
}