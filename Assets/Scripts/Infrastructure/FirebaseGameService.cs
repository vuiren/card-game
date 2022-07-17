using Domain.DTO;
using Firebase.Database;
using Scriptable_Objects;
using Services;
using UnityEngine;

namespace Infrastructure
{
    public class FirebaseGameService : IGameService
    {
        private readonly Configuration _configuration;
        private readonly DatabaseReference _gameReference;
        private bool _isHostReady;
        private GameData _lastGameData;

        public FirebaseGameService(Configuration configuration)
        {
            _configuration = configuration;
            _gameReference = FirebaseDatabase.DefaultInstance.RootReference.Child(_configuration.gameId).Child("game");
            _gameReference.ValueChanged += HandleChange;
        }

        public bool IsHostReady()
        {
            return _isHostReady;
        }

        public void SetHostReady(bool ready)
        {
            if (!_configuration.isHost) return;

            _isHostReady = ready;
            _gameReference.GetValueAsync().ContinueWith(x =>
            {
                var gameData = JsonUtility.FromJson<GameData>(x.Result.GetRawJsonValue());

                gameData.hostReady = _isHostReady;

                _gameReference.SetRawJsonValueAsync(JsonUtility.ToJson(gameData));
            });
        }

        public void SetCardsInHandsCount(int count)
        {
            if (!_configuration.isHost) return;

            _gameReference.GetValueAsync().ContinueWith(x =>
            {
                var gameData = JsonUtility.FromJson<GameData>(x.Result.GetRawJsonValue());
                gameData.cardsInHandsCount = count;
                _gameReference.SetRawJsonValueAsync(JsonUtility.ToJson(gameData));
            });
        }

        public int GetCardsInHandsCount()
        {
            return _lastGameData.cardsInHandsCount;
        }

        public void DeleteGameData()
        {
            FirebaseDatabase.DefaultInstance.RootReference.Child(_configuration.gameId).SetValueAsync(null);
        }

        private void HandleChange(object sender, ValueChangedEventArgs e)
        {
            if (e.Snapshot?.Value == null) return;

            var value = JsonUtility.FromJson<GameData>(e.Snapshot.GetRawJsonValue());
            _lastGameData = value;
            _isHostReady = value.hostReady;
        }
    }
}