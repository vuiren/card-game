using System.Collections.Generic;
using System.Linq;
using Game_Code.Domain;
using Mono.CSharp;
using Scriptable_Objects;
using UnityEngine;
using Util;

namespace Services
{
    public interface ISessionService
    {
        void AddCardToSession(int playerId, CardSheet cardSheet);
        int GetSessionWinner();
        List<Tuple<int, CardSheet>> GetCardsInSession();
        void ClearCards();
    }
    
    public class SessionService : ISessionService
    {
        private readonly List<Tuple<int, CardSheet>> _cardsInSession = new();
        private readonly ICenterDeckService _centerDeckService;
        
        public SessionService(ICenterDeckService centerDeckService)
        {
            _centerDeckService = centerDeckService;
        }
        
        public void AddCardToSession(int playerId, CardSheet cardSheet)
        {
            _cardsInSession.Add(new Tuple<int, CardSheet>(playerId, cardSheet));
        }

        public int GetSessionWinner()
        {
            var trumpCard = _centerDeckService.TrumpCard;

            var bestCard = _cardsInSession[0];

            foreach (var card in from card in _cardsInSession let result = CardsComparator.CompareCards(bestCard.Item2, card.Item2, trumpCard.cardSheet) where result == -1 select card)
            {
                bestCard = card;
            }

            return bestCard.Item1;
        }

        public List<Tuple<int, CardSheet>> GetCardsInSession()
        {
            return _cardsInSession;
        }

        public void ClearCards()
        {
            _cardsInSession.Clear();
        }
    }
}