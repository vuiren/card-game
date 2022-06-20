using System;
using System.Collections.Generic;
using System.Linq;
using Domain.DTO;
using UnityEngine;

namespace Services
{
    public interface IPlayerService
    {
        int LocalPlayerId { get; }
        void RegisterPlayer(int id);
        PlayerData GetPlayer(int id);
        IEnumerable<PlayerData> GetAllPlayers();
        void OnPlayerListChanged(Action<List<PlayerData>> players);
    }

    public class PlayerService : IPlayerService
    {
        private readonly Dictionary<int, PlayerData> _players = new();

        public void RegisterPlayer(int id)
        {
            Debug.Log($"Registering player {id}");
            if (_players.ContainsKey(id))
            {
                Debug.LogWarning($"Player with id: '{id}' already registered");
                return;
            }

            _players.Add(id, new PlayerData { id = id });
        }

        public PlayerData GetPlayer(int id)
        {
            if (_players.ContainsKey(id))
            {
                Debug.Log($"Found player: '{_players[id]}'");
                return _players[id];
            }

            Debug.LogWarning($"Player with id: '{id}' not found");
            return null;
        }

        public IEnumerable<PlayerData> GetAllPlayers()
        {
            return _players.Values.ToArray();
        }

        public int LocalPlayerId { get; }

        public void OnPlayerListChanged(Action<List<PlayerData>> players)
        {
        }
    }
}