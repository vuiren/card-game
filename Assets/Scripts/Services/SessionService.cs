using System;
using System.Collections.Generic;
using System.Linq;
using Domain;
using Domain.DTO;
using Scriptable_Objects;
using UnityEngine;
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
    
    public class SessionService : ISessionService
    {
        private readonly List<Mono.CSharp.Tuple<int, CardSheet>> _cardsInSession = new();
        private readonly ICenterDeckService _centerDeckService;
        private readonly Configuration _configuration;
        
        public SessionService(Configuration configuration, ICenterDeckService centerDeckService)
        {
            _configuration = configuration;
            _centerDeckService = centerDeckService;
        }
        
        public void AddCardToSession(int playerId, CardSheet cardSheet)
        {
            _cardsInSession.Add(new Mono.CSharp.Tuple<int, CardSheet>(playerId, cardSheet));
        }

        public void AnnounceSessionWinnerId()
        {
            throw new NotImplementedException();
        }

        public int GetSessionWinnerId()
        {
            var trumpCard = _centerDeckService.GetTrumpCard();

            var bestCard = _cardsInSession[0];

            foreach (var card in from card in _cardsInSession let result = CardsComparator.CompareCards(_configuration, bestCard.Item2.cardId, card.Item2.cardId, trumpCard.cardId) where result == -1 select card)
            {
                bestCard = card;
            }

            return bestCard.Item1;
        }

        public List<SessionPlayer> GetCardsInSession()
        {
            return _cardsInSession.Select(x=>new SessionPlayer(){playerId = x.Item1, playedCardId = x.Item2.cardId}).ToList();
        }

        public void ClearCards()
        {
            _cardsInSession.Clear();
        }

        public void OnCardAddedToSession(Action<List<SessionPlayer>> cards)
        {
            throw new NotImplementedException();
        }

        public void OnSessionWinnerAnnounced(Action<int> action)
        {
            throw new NotImplementedException();
        }
    }
}