using UnityEngine;

namespace Scriptable_Objects
{
    [CreateAssetMenu(fileName = "New Card", menuName = "Add Card")]
    public class CardSheet : ScriptableObject
    {
        public int cardId;
        public string cardName = "Карта";
        public Sprite cardSprite;

        public override string ToString()
        {
            return $"Card Name: '{cardName}', Card Id: '{cardId}'";
        }
    }
}
