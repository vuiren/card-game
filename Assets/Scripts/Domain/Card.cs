using Scriptable_Objects;
using UnityEngine;

namespace Domain
{
    public class Card : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        public CardSheet cardSheet;

        public void UpdateCard()
        {
            spriteRenderer.sprite = cardSheet.cardSprite;
        }
    }
}