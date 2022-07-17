using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Domain.DTO;
using Firebase.Database;
using Mono.CSharp;
using Scriptable_Objects;
using Services;
using UnityEngine;
using Util;

namespace Infrastructure
{
    public class FirebaseBetsService : IBetsService
    {
        private readonly Configuration _configuration;
        private readonly Dictionary<int, int> _playerBets = new();
        private readonly DatabaseReference _playersReference, _betsQueueReference;
        private Action _onBetChanged, _onBetPlayerChanged;
        private int _currentBetPlayerId = -1;
        private BetsQueueData lastQueueData;

        public FirebaseBetsService(Configuration configuration)
        {
            _configuration = configuration;
            _playersReference = FirebaseDatabase.DefaultInstance.RootReference.Child(_configuration.gameId)
                .Child("players");
            _betsQueueReference = _playersReference.Parent.Child("BetsQueue");
            _betsQueueReference.ValueChanged += HandleBetsQueueChange;
            _playersReference.ValueChanged += HandleChange;
        }

        private void HandleBetsQueueChange(object sender, ValueChangedEventArgs e)
        {
            if (e.Snapshot?.Value == null) return;
            var value = JsonUtility.FromJson<BetsQueueData>(e.Snapshot.GetRawJsonValue());
            _currentBetPlayerId = value.currentPlayerId;
            lastQueueData = value;
            _onBetPlayerChanged?.Invoke();
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

        public void OnBetPlayerChanged(Action onBetPlayerChanged)
        {
            _onBetPlayerChanged += onBetPlayerChanged;
        }

        public int GetCurrentBetPlayer() => _currentBetPlayerId;
        public async void SetTurnsOrder(Queue<int> turnsOrder)
        {
            var betsQueueData = new BetsQueueData();
            var orderString = ArrayMethods.TurnArrayToString(turnsOrder);

            betsQueueData.playersQueue = orderString;
            betsQueueData.currentPlayerId = turnsOrder.Peek();
            betsQueueData.done = false;
            _currentBetPlayerId = betsQueueData.currentPlayerId;

            var jsonData = JsonUtility.ToJson(betsQueueData);

            await _betsQueueReference.SetRawJsonValueAsync(jsonData);
        }

        public async UniTask FinishMakingBet()
        {
            var value = await _betsQueueReference.GetValueAsync();
            var queueData = JsonUtility.FromJson<BetsQueueData>(value.GetRawJsonValue());
            var queue = new Queue<int>(ArrayMethods.TurnIdsStringToIntArray(queueData.playersQueue));
            queue.Dequeue();
            if (queue.Count == 0)
            {
                queueData.currentPlayerId = -1;
                queueData.playersQueue = "";
                queueData.done = true;
            }
            else
            {
                var newId = queue.Peek();
                queueData.currentPlayerId = newId;
                queueData.playersQueue = ArrayMethods.TurnArrayToString(queue.ToArray());
            }
            await _betsQueueReference.SetRawJsonValueAsync(JsonUtility.ToJson(queueData));
        }

        public bool BetsSet()
        {
            return lastQueueData.done;
        }
    }
}