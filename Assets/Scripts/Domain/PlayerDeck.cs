using System;
using Game_Code;
using UnityEngine;

namespace Domain
{
    [RequireComponent(typeof(Actor))]
    public class PlayerDeck : MonoBehaviour
    {
        public Actor actor;
        public Transform hand, selectedMapPoint;
        [SerializeField] private float gapBetweenCards;

        private Card[] _cards;
        private void Awake()
        {
            actor = GetComponent<Actor>();
        }

        public void PlaceACard(Card card)
        {
            
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                StructureHand();
            }
        }

        public void SetCards(Card[] cards)
        {
            _cards = cards;
        }

        public void StructureHand()
        {
            var firstX =- _cards.Length / gapBetweenCards;

            for (var i = 0; i < _cards.Length; i++)
            {
                var child = _cards[i].transform;
                child.position = hand.position;
                child.Translate(Vector3.right * (firstX + i * gapBetweenCards), Space.Self);
            }
        }
    }
}