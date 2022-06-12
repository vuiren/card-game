using System;
using System.Collections.Generic;
using System.Linq;
using Domain;
using Domain.DTO;
using Firebase.Database;
using Mono.CSharp;
using Scriptable_Objects;
using Services;
using UnityEngine;
using Util;

namespace Infrastructure
{
    public class FirebaseSessionService: ISessionService
    {
        private readonly Configuration _configuration;
        private readonly ICenterDeckService _centerDeckService;
        private readonly DatabaseReference _databaseReference;
        private List<SessionPlayer> _sessionPlayers = new();
        private Action<List<SessionPlayer>> _onCardsChanged;
        
        public FirebaseSessionService(Configuration configuration, ICenterDeckService centerDeckService)
        {
            _configuration = configuration;
            _centerDeckService = centerDeckService;
            _databaseReference = FirebaseDatabase.DefaultInstance.RootReference.Child(configuration.gameId).Child("session");
            _databaseReference.ValueChanged += HandleChange;
        }

        private void HandleChange(object sender, ValueChangedEventArgs e)
        {
            if(e.Snapshot == null) return;

            var children = e.Snapshot.Children;
            foreach (var child in children)
            {
                var sessionPlayer = JsonUtility.FromJson<SessionPlayer>(child.GetRawJsonValue());
                _sessionPlayers.Add(sessionPlayer);
            }

            _onCardsChanged?.Invoke(_sessionPlayers);
        }

        public void AddCardToSession(int playerId, CardSheet cardSheet)
        {
            var sessionPlayer = new SessionPlayer()
            {
                playerId = playerId,
                playedCardId = cardSheet.cardId,
            };
            _databaseReference.Child(playerId.ToString()).SetRawJsonValueAsync(JsonUtility.ToJson(sessionPlayer));
        }

        public int GetSessionWinnerId()
        {
            var trumpCard = _centerDeckService.GetTrumpCard();

            var bestCard = _sessionPlayers[0];

            foreach (var card in from card in _sessionPlayers let result = CardsComparator.CompareCards(_configuration, bestCard.playedCardId, card.playedCardId, trumpCard.cardId) where result == -1 select card)
            {
                bestCard = card;
            }

            return bestCard.playerId;
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
    }
}