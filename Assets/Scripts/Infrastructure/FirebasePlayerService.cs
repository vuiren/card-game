using System;
using System.Collections.Generic;
using System.Linq;
using Domain.DTO;
using Firebase.Database;
using Scriptable_Objects;
using Services;
using UnityEngine;

namespace Infrastructure
{
    public class FirebasePlayerService : IPlayerService
    {
        private readonly Configuration _configuration;
        private readonly Dictionary<int, PlayerData> _players = new();
        private Action<List<PlayerData>> _onPlayerListChanged;

        public FirebasePlayerService(Configuration configuration)
        {
            _configuration = configuration;
            var playersRef = FirebaseDatabase.DefaultInstance.RootReference.Child(_configuration.gameId)
                .Child("players");
            playersRef.ValueChanged += HandleIt;
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

        public void RegisterPlayer(int id)
        {
            var playersRef = FirebaseDatabase.DefaultInstance.RootReference.Child(_configuration.gameId)
                .Child("players");

            var playerData = new PlayerData
            {
                id = id,
                betsCount = 0,
                winCount = 0
            };

            var jsonData = JsonUtility.ToJson(playerData);
            playersRef.Child("Player: " + id).SetRawJsonValueAsync(jsonData);
        }

        IEnumerable<PlayerData> IPlayerService.GetAllPlayers()
        {
            return _players.Values;
        }

        public int LocalPlayerId => _configuration.playerId;

        public void OnPlayerListChanged(Action<List<PlayerData>> players)
        {
            _onPlayerListChanged += players;
        }

        private void HandleIt(object sender, ValueChangedEventArgs e)
        {
            if (e.Snapshot == null) return;

            Debug.Log("Registering player");
            var players = e.Snapshot.Children;
            foreach (var player in players)
            {
                var playerData = JsonUtility.FromJson<PlayerData>(player.GetRawJsonValue());
                if (_players.ContainsKey(playerData.id))
                    _players[playerData.id] = playerData;
                else
                    _players.Add(playerData.id, playerData);
                Debug.Log("Registered player: " + JsonUtility.FromJson<PlayerData>(player.GetRawJsonValue()).name);
            }

            _onPlayerListChanged(_players.Values.ToList());
        }
    }
}