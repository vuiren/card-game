using System.Collections.Generic;
using Scriptable_Objects;

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
}