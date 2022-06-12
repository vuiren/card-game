using System;
using UnityEngine;

namespace Scriptable_Objects
{
    public enum Suit
    {
        Spades,
        Hearts,
        Diamonds,
        Clubs,
    }
    
    [Serializable]
    [CreateAssetMenu(fileName = "New Card", menuName = "Add Card")]
    public class CardSheet : ScriptableObject
    {
        public int cardId;
        public string cardName = "Карта";
        public Suit cardSuit;
        public Sprite cardSprite;
        public int cardValue;

        public override string ToString()
        {
            return $"Card Name: '{cardName}', Card Id: '{cardId}'";
        }
    }
}
