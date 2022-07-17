using System;

namespace Domain.DTO
{
    [Serializable]
    public class GameData
    {
        public bool hostReady;
        public int currentTurnId;
        public int cardsInHandsCount;
        public int lastId;
        public string playersOrder;
    }
}