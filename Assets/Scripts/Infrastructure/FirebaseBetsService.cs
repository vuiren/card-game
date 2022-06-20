using System;
using System.Collections.Generic;
using Domain.DTO;
using Firebase.Database;
using Scriptable_Objects;
using Services;
using UnityEngine;

namespace Infrastructure
{
    public class FirebaseBetsService : IBetsService
    {
        private readonly Configuration _configuration;
        private readonly Dictionary<int, int> _playerBets = new();
        private readonly DatabaseReference _playersReference;
        private Action _onBetChanged;

        public FirebaseBetsService(Configuration configuration)
        {
            _configuration = configuration;
            _playersReference = FirebaseDatabase.DefaultInstance.RootReference.Child(_configuration.gameId)
                .Child("players");
            _playersReference.ValueChanged += HandleChange;
        }

        public async void MakeABet(int playerId, int bet)
        {
            var value = await _playersReference.GetValueAsync();
            foreach (var child in value.Children)
            {
                var playerData = JsonUtility.FromJson<PlayerData>(child.GetRawJsonValue());

                if (playerData.id != playerId) continue;

                playerData.betsCount = bet;
                await _playersReference.Child(playerData.name).SetRawJsonValueAsync(JsonUtility.ToJson(playerData));

                if (_playerBets.ContainsKey(playerId))
                    _playerBets[playerId] = bet;
                else
                    _playerBets.Add(playerId, bet);
                return;
            }
        }


        public int GetPlayerBet(int playerId)
        {
            return _playerBets.ContainsKey(playerId) ? _playerBets[playerId] : 0;
        }

        public void OnBetChanged(Action onBetsChanged)
        {
            _onBetChanged += onBetsChanged;
        }

        private void HandleChange(object sender, ValueChangedEventArgs e)
        {
            if (e.Snapshot?.Value == null) return;

            foreach (var child in e.Snapshot.Children)
            {
                var value = JsonUtility.FromJson<PlayerData>(child.GetRawJsonValue());
                if (_playerBets.ContainsKey(value.id))
                    _playerBets[value.id] = value.betsCount;
                else
                    _playerBets.Add(value.id, value.betsCount);
            }

            _onBetChanged?.Invoke();
        }
    }
}