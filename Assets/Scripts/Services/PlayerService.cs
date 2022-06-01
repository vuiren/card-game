using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game_Code.Domain;
using UnityEngine;

namespace Services
{
    public interface IPlayerService
    {
        void RegisterPlayer(Player player);
        Player GetPlayer(int id);
        IEnumerable<Player> GetAllPlayers();
    }
    
    public class PlayerService:MonoBehaviour, IPlayerService
    {
        private readonly Dictionary<int, Player> _players = new();
        
        public void RegisterPlayer(Player player)
        {
            Debug.Log($"Registering player {player}");
            if (_players.ContainsKey(player.actor.id))
            {
                Debug.LogWarning($"Player with id: '{player.actor.id}' already registered");
                return;
            }

            _players.Add(player.actor.id, player);
        }

        public Player GetPlayer(int id)
        {
            if (_players.ContainsKey(id))
            {
                Debug.Log($"Found player: '{_players[id]}'");
                return _players[id];
            }

            Debug.LogWarning($"Player with id: '{id}' not found");
            return null;
        }

        public IEnumerable<Player> GetAllPlayers()
        {
            return _players.Values.ToArray();
        }
    }
}