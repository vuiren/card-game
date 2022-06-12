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
}