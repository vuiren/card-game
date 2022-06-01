using UnityEngine;

namespace Scriptable_Objects
{
    [CreateAssetMenu(fileName = "New Configuration", menuName = "Add Configuration", order = 0)]
    public class Configuration : ScriptableObject
    {
        public CardSheet[] cardsInGame;
        public GameObject cardPrefab;
    }
}