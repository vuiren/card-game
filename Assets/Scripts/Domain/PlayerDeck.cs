using Unity.VisualScripting;
using UnityEngine;

namespace Domain
{
    [RequireComponent(typeof(Actor))]
    public class PlayerDeck : MonoBehaviour
    {
        public bool mainDeck;
        public Actor actor;
        public Transform hand, selectedMapPoint;
        [SerializeField] private float gapBetweenCards;
        [SerializeField] private Transform hiddenCardRoot;
        [SerializeField] private bool cardsHidden;

        private Card[] _cards;

        private void Awake()
        {
            if (cardsHidden)
            {
                hiddenCardRoot.GameObject().SetActive(true);
                hand.gameObject.SetActive(false);
            }

            actor = GetComponent<Actor>();
        }

        public void SetCards(Card[] cards)
        {
            _cards = cards;
        }

        public void StructureHand()
        {
            var firstX = -_cards.Length / gapBetweenCards;

            for (var i = 0; i < _cards.Length; i++)
            {
                var child = _cards[i].transform;
                child.position = hand.position;
                child.Translate(Vector3.right * (firstX + i * gapBetweenCards), Space.Self);
            }
        }
    }
}