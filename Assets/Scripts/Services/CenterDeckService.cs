using System.Collections.Generic;
using System.Linq;
using Domain;
using Scriptable_Objects;
using UnityEngine;

namespace Services
{
    public interface ICenterDeckService
    {
        void SetTrumpCard(int trumpCardId);
        CardSheet GetTrumpCard();
        CardSheet[] GetCards(int cardsCount);
        void SetCards(IEnumerable<CardSheet> cardSheets);
        CardSheet[] GetAllCardsInCenter();
    }
    
    public class CenterDeckService : ICenterDeckService
    {
        private List<CardSheet> _cardsInCenter;

        public Card TrumpCard { get; set; }
        public void SetTrumpCard(int trumpCardId)
        {
            throw new System.NotImplementedException();
        }

        public CardSheet GetTrumpCard()
        {
            throw new System.NotImplementedException();
        }

        public CardSheet[] GetCards(int cardsCount)
        {
            var cards = new List<CardSheet>(_cardsInCenter.Take(cardsCount));
            _cardsInCenter.RemoveRange(0, cardsCount);

            return cards.ToArray();
        }

        public void SetCards(IEnumerable<CardSheet> cardSheets)
        {
            _cardsInCenter = cardSheets.ToList();
        }

        public CardSheet[] GetAllCardsInCenter()
        {
            return _cardsInCenter.ToArray();
        }
    }
}