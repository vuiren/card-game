using UnityEngine;

namespace Scriptable_Objects
{
    [CreateAssetMenu(fileName = "New Configuration", menuName = "Add Configuration", order = 0)]
    public class Configuration : ScriptableObject
    {
        public int playerId;
        public string playerName, gameId;
        public int playersCount;
        public int lastHandCount;
        public bool isHost;
        public CardSheet[] cardsInGame;
        public GameObject cardPrefab;
    }
}