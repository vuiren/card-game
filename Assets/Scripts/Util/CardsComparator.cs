using Scriptable_Objects;

namespace Util
{
    public static class CardsComparator
    {
        /// <summary>
        /// -1 - card1 хуже, 0 - карты равны, 1 - card1 лучше
        /// </summary>
        public static int CompareCards(CardSheet card1, CardSheet card2, CardSheet trump)
        {
            if (card1.cardSuit == trump.cardSuit && card2.cardSuit != trump.cardSuit)
            {
                return 1;
            }

            if (card2.cardSuit == trump.cardSuit && card1.cardSuit != trump.cardSuit)
            {
                return -1;
            }
            
            if (card1.cardValue < card2.cardValue)
            {
                return -1;
            }

            if (card1.cardValue == card2.cardValue)
            {
                return 0;
            }

            if (card1.cardValue > card2.cardValue)
            {
                return 1;
            }

            return 0;
        }
    }
}