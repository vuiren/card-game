using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Domain;
using Domain.DTO;
using Firebase.Database;
using Scriptable_Objects;
using Services;
using UnityEngine;
using Util;
using Zenject;

namespace Infrastructure
{
    public class FirebaseTurnsService: ITurnsService
    {
        private readonly DatabaseReference _gameRef;
        private readonly Configuration _configuration;
        private int _currentTurn = -1;
        private Action<int> _onTurnChanged;

        [Inject]
        public FirebaseTurnsService(Configuration configuration)
        {
            _configuration = configuration;
            _gameRef = FirebaseDatabase.DefaultInstance.RootReference.Child(_configuration.gameId).Child("game");
            _gameRef.ValueChanged += HandleChildChange;
        }

        private void HandleChildChange(object sender, ValueChangedEventArgs e)
        {
            if(e.Snapshot?.Value == null) return;

            var value = JsonUtility.FromJson<GameData>(e.Snapshot.GetRawJsonValue());
            _currentTurn = value.currentTurnId;
            _onTurnChanged?.Invoke(_currentTurn);
        }

        public async void SetTurnsOrder(Queue<int> players)
        {
            var gameData = new GameData();
            var orderString = ArrayMethods.TurnArrayToString(players);

            gameData.playersOrder = orderString;
            gameData.currentTurnId = players.Peek();
            _currentTurn = gameData.currentTurnId;
            
            var jsonData = JsonUtility.ToJson(gameData);

            await _gameRef.SetRawJsonValueAsync(jsonData);
        }

        public async void NextTurn()
        {
            var value = await _gameRef.GetValueAsync();
            var gameData = JsonUtility.FromJson<GameData>(value.GetRawJsonValue());
            
            var ids = ArrayMethods.TurnIdsStringToIntArray(gameData.playersOrder);

            var idsQueue = new Queue<int>(ids);
            idsQueue.Dequeue();
            
            gameData.playersOrder = ArrayMethods.TurnArrayToString(idsQueue);
            gameData.currentTurnId = idsQueue.Count > 0 ? idsQueue.Peek() : -1;

            _currentTurn = gameData.currentTurnId;
            await _gameRef.SetRawJsonValueAsync(JsonUtility.ToJson(gameData));
            Debug.Log(gameData);
        }

        public int CurrentTurn()
        {
            return _currentTurn;
        }

        public void OnTurnChange(Action<int> action)
        {
            _onTurnChanged += action;
        }
    }
}