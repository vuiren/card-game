﻿using System;
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
    public class FirebaseHandsService : IHandsService
    {
        private readonly Configuration _configuration;
        private readonly Dictionary<int, int[]> _hands = new();
        private readonly DatabaseReference _handsReference;

        public FirebaseHandsService(Configuration configuration)
        {
            _configuration = configuration;
            _handsReference = FirebaseDatabase.DefaultInstance.RootReference.Child(_configuration.gameId)
                .Child("hands");
            _handsReference.ValueChanged += HandleChange;
        }

        public void SetHand(int playerId, IEnumerable<CardSheet> cards)
        {
            var cardSheets = cards as CardSheet[] ?? cards.ToArray();
            var playerHand = new PlayerHand
            {
                playerId = playerId,
                cards = ArrayMethods.TurnArrayToString(cardSheets.Select(x => x.cardId))
            };

            _handsReference.Child(playerId.ToString()).SetRawJsonValueAsync(JsonUtility.ToJson(playerHand));

            if (!_hands.ContainsKey(playerId))
                _hands.Add(playerId, cardSheets.Select(x => x.cardId).ToArray());
        }

        public void RemoveFromHand(int playerId, int cardId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<CardSheet> GetHand(int playerId)
        {
            return _configuration.cardsInGame.Where(x => _hands[playerId].Contains(x.cardId));
        }

        private void HandleChange(object sender, ValueChangedEventArgs e)
        {
            if (e.Snapshot == null) return;

            foreach (var child in e.Snapshot.Children)
            {
                var value = JsonUtility.FromJson<PlayerHand>(child.GetRawJsonValue());

                if (_hands.ContainsKey(value.playerId))
                {
                    if (value.cards.Length == 0)
                        _hands[value.playerId] = Array.Empty<int>();
                    else
                        _hands[value.playerId] = ArrayMethods.TurnIdsStringToIntArray(value.cards);
                }
                else
                {
                    _hands.Add(value.playerId,
                        value.cards.Length == 0
                            ? Array.Empty<int>()
                            : ArrayMethods.TurnIdsStringToIntArray(value.cards));
                }
            }
        }
    }
}