using System.Collections.Generic;
using Scriptable_Objects;

namespace Services
{
    public interface IHandsService
    {
        void SetHand(int playerId, IEnumerable<CardSheet> cards);
        void RemoveFromHand(int playerId, int cardId);
        IEnumerable<CardSheet> GetHand(int playerId);
    }
}