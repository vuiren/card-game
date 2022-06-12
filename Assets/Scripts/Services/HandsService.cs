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
}