using System;

namespace Domain.DTO
{
    [Serializable]
    public class PlayerData
    {
        public string name;
        public int id;
        public int betsCount;
        public int winCount;
    }
}