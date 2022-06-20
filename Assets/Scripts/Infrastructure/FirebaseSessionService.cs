using System;
using System.Collections.Generic;
using System.Linq;
using Domain.DTO;
using Firebase.Database;
using Scriptable_Objects;
using Services;
using UnityEngine;
using Util;

namespace Infrastructure
{
    public class FirebaseSessionService : ISessionService
    {
        private readonly ICenterDeckService _centerDeckService;
        private readonly Configuration _configuration;
        private readonly DatabaseReference _databaseReference;
        private Action<List<SessionPlayer>> _onCardsChanged;
        private Action<int> _onSessionWinnerAnnounced;
        private List<SessionPlayer> _sessionPlayers = new();

        public FirebaseSessionService(Configuration configuration, ICenterDeckService centerDeckService)
        {
            _configuration = configuration;
            _centerDeckService = centerDeckService;
            _databaseReference = FirebaseDatabase.DefaultInstance.RootReference.Child(configuration.gameId)
                .Child("session");
            _databaseReference.ValueChanged += HandleChange;
        }

        public void AddCardToSession(int playerId, CardSheet cardSheet)
        {
            var sessionPlayer = new SessionPlayer
            {
                playerId = playerId,
                playedCardId = cardSheet.cardId
            };
            _databaseReference.Child(playerId.ToString()).SetRawJsonValueAsync(JsonUtility.ToJson(sessionPlayer));
        }

        public void AnnounceSessionWinnerId()
        {
            var trumpCard = _centerDeckService.GetTrumpCard();
            var firstCardInSession = _sessionPlayers.FirstOrDefault();
            var bestCard = _sessionPlayers[0];

            foreach (var card in from card in _sessionPlayers
                     let result =
                         CardsComparator.CompareCards(_configuration, firstCardInSession.playedCardId, bestCard.playedCardId, card.playedCardId,
                             trumpCard.cardId)
                     where result == -1
                     select card) bestCard = card;

            _onSessionWinnerAnnounced?.Invoke(bestCard.playerId);
        }

        public List<SessionPlayer> GetCardsInSession()
        {
            return _sessionPlayers;
        }

        public void ClearCards()
        {
            _databaseReference.SetValueAsync(null);
            _sessionPlayers = new List<SessionPlayer>();
        }

        public void OnCardAddedToSession(Action<List<SessionPlayer>> cards)
        {
            _onCardsChanged += cards;
        }

        public void OnSessionWinnerAnnounced(Action<int> action)
        {
            _onSessionWinnerAnnounced += action;
        }

        private void HandleChange(object sender, ValueChangedEventArgs e)
        {
            if (e.Snapshot == null) return;

            var children = e.Snapshot.Children;
            foreach (var child in children)
            {
                var sessionPlayer = JsonUtility.FromJson<SessionPlayer>(child.GetRawJsonValue());
                _sessionPlayers.Add(sessionPlayer);
            }

            _onCardsChanged?.Invoke(_sessionPlayers);
        }
    }
}