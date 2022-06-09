﻿using System.Collections.Generic;
using System.Linq;
using Domain;
using Game_Code.Domain;
using Unity.VisualScripting;
using UnityEngine;

namespace Services
{
    public interface IHandsService
    {
        void SetHand(Player player, IEnumerable<Card> cards);
        void RemoveFromHand(Player player, Card card);
        IEnumerable<Card> GetHand(Player player);
    }

    public class HandsService : IHandsService
    {
        private readonly Dictionary<int, List<Card>> _hands = new();

        public void SetHand(Player player, IEnumerable<Card> cards)
        {
            if (_hands.ContainsKey(player.actor.id))
            {
                _hands[player.actor.id] = cards.ToList();
            }
            else
            {
                _hands.Add(player.actor.id, cards.ToList());
            }

            Debug.Log("Hand set");
        }

        public void RemoveFromHand(Player player, Card card)
        {
            if (!_hands.ContainsKey(player.actor.id)) return;

            var cardInHand = _hands[player.actor.id].FirstOrDefault(x => x.cardSheet.cardId == card.cardSheet.cardId);
            if (cardInHand)
            {
                _hands[player.actor.id].Remove(cardInHand);
            }
        }

        public IEnumerable<Card> GetHand(Player player)
        {
            return _hands.ContainsKey(player.actor.id) ? _hands[player.actor.id].ToArray() : null;
        }
    }
}