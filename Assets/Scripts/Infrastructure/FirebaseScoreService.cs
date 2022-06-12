using System;
using System.Collections.Generic;
using Domain.DTO;
using Firebase.Database;
using Scriptable_Objects;
using Services;
using UnityEngine;

namespace Infrastructure
{
    public class FirebaseScoreService: IScoreService
    {
        private readonly DatabaseReference _playersReference;
        private readonly Configuration _configuration;
        private readonly Dictionary<int, int> _playerPoints = new();
        private Action _onScoreChanged;

        public FirebaseScoreService(Configuration configuration)
        {
            _configuration = configuration;
            _playersReference = FirebaseDatabase.DefaultInstance.RootReference.Child(_configuration.gameId).Child("players");
            _playersReference.ValueChanged += HandleChange;
            
        }

        private void HandleChange(object sender, ValueChangedEventArgs e)
        {
            if(e.Snapshot?.Value == null) return;

            foreach (var child in e.Snapshot.Children)
            {
                var value = JsonUtility.FromJson<PlayerData>(child.GetRawJsonValue());

                if (_playerPoints.ContainsKey(value.id))
                {
                    _playerPoints[value.id] = value.winCount;
                }
                else
                {
                    _playerPoints.Add(value.id, value.winCount);
                }
            }
            _onScoreChanged?.Invoke();
        }

        public async void AddPointToPlayer(int playerId)
        {
            var value = await _playersReference.GetValueAsync();

            foreach (var child in value.Children)
            {
                var playerData = JsonUtility.FromJson<PlayerData>(child.GetRawJsonValue());
                if (playerData.id != playerId) continue;
                
                playerData.winCount++;
                await _playersReference.Child(playerData.name).SetRawJsonValueAsync(JsonUtility.ToJson(playerData));

                if (_playerPoints.ContainsKey(playerId))
                {
                    _playerPoints[playerId] = playerData.winCount;
                }
                else
                {
                    _playerPoints.Add(playerId, playerData.winCount);
                }
                
                return;
            }
        }

        public int GetPlayerPoints(int playerId)
        {
            return _playerPoints.ContainsKey(playerId) ? _playerPoints[playerId] : 0;
        }

        public void OnScoreChanged(Action action)
        {
            _onScoreChanged += action;
        }
    }
}