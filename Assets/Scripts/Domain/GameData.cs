using System;

namespace Domain
{
    [Serializable]
    public class GameData
    {
        public int currentTurnId;
        public int[] cardsInCenterIds;
    }
}