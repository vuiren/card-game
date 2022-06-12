using System.Collections.Generic;
using System.Linq;
using Domain;
using Scriptable_Objects;
using Unity.VisualScripting;
using UnityEngine;

namespace Services
{
    public interface IHandsService
    {
        void SetHand(int playerId, IEnumerable<CardSheet> cards);
        void RemoveFromHand(int playerId, int cardId);
        IEnumerable<CardSheet> GetHand(int playerId);
    }

    public class HandsService : IHandsService
    {
        private readonly Dictionary<int, List<Card>> _hands = new();

        public void SetHand(int playerId, IEnumerable<Card> cards)
        {
            if (_hands.ContainsKey(playerId))
            {
                _hands[playerId] = cards.ToList();
            }
            else
            {
                _hands.Add(playerId, cards.ToList());
            }

            Debug.Log("Hand set");
        }

        public void SetHand(int playerId, IEnumerable<CardSheet> cards)
        {
            throw new System.NotImplementedException();
        }

        public void RemoveFromHand(int playerId, int cardId)
        {
            throw new System.NotImplementedException();
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

        IEnumerable<CardSheet> IHandsService.GetHand(int playerId)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Card> GetHand(int playerId)
        {
            return _hands.ContainsKey(playerId) ? _hands[playerId].ToArray() : null;
        }
    }
}