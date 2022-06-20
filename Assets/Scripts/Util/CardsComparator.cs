using System.Linq;
using Scriptable_Objects;

namespace Util
{
    public static class CardsComparator
    {
        /// <summary>
        ///     -1 - card1 хуже, 0 - карты равны, 1 - card1 лучше
        /// </summary>
        public static int CompareCards(Configuration configuration, int firstCardInSessionId, int card1Id, int card2Id,
            int trumpCardId)
        {
            var firstCardInSession = configuration.cardsInGame.FirstOrDefault(x => x.cardId == firstCardInSessionId);
            if (firstCardInSession == null) return -1;

            var card1 = configuration.cardsInGame.FirstOrDefault(x => x.cardId == card1Id);
            if (card1 == null) return -1;

            var card2 = configuration.cardsInGame.FirstOrDefault(x => x.cardId == card2Id);
            if (card2 == null) return -1;

            var trump = configuration.cardsInGame.FirstOrDefault(x => x.cardId == trumpCardId);
            if (trump == null) return -1;

            if (card1.cardSuit == trump.cardSuit && card2.cardSuit != trump.cardSuit) return 1;

            if (card2.cardSuit == trump.cardSuit && card1.cardSuit != trump.cardSuit) return -1;

            if (card1.cardSuit == trump.cardSuit && card2.cardSuit == trump.cardSuit)
            {
                if (card1.cardValue < card2.cardValue) return -1;

                if (card1.cardValue == card2.cardValue) return 0;

                if (card1.cardValue > card2.cardValue) return 1;
            }


            if (card1.cardSuit == firstCardInSession.cardSuit &&
                card2.cardSuit != firstCardInSession.cardSuit) return 1;

            if (card2.cardSuit == firstCardInSession.cardSuit &&
                card1.cardSuit != firstCardInSession.cardSuit) return -1;

            if (card1.cardSuit == firstCardInSession.cardSuit && card2.cardSuit == firstCardInSession.cardSuit)
            {
                if (card1.cardValue < card2.cardValue) return -1;

                if (card1.cardValue == card2.cardValue) return 0;

                if (card1.cardValue > card2.cardValue) return 1;
            }

            return 0;
        }
    }
}