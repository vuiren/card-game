using System;
using System.Collections.Generic;
using System.Linq;
using Domain.DTO;
using Scriptable_Objects;
using Util;

namespace Services
{
    public interface ISessionService
    {
        void AddCardToSession(int playerId, CardSheet cardSheet);
        void AnnounceSessionWinnerId();
        List<SessionPlayer> GetCardsInSession();
        void ClearCards();
        void OnCardAddedToSession(Action<List<SessionPlayer>> cards);
        void OnSessionWinnerAnnounced(Action<int> action);
    }
}