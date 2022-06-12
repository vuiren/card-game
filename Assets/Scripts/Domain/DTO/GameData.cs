using System;

namespace Domain.DTO
{
    [Serializable]
    public class GameData
    {
        public bool hostReady;
        public int currentTurnId;
        public int lastId;
        public int[] cardsInCenterIds;
        public string playersOrder;
    }
}