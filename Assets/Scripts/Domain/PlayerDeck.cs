using System;
using UnityEngine;

namespace Domain
{
    public class PlayerDeck : MonoBehaviour
    { 
        public Transform hand;

        [SerializeField] private float gapBetweenCards;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StructureHand();
            }
        }

        public void StructureHand()
        {
            var firstX =- hand.childCount / gapBetweenCards;

            for (int i = 0; i < hand.childCount; i++)
            {
                var child = hand.GetChild(i);
                child.position = hand.position;
                child.Translate(Vector3.right * (firstX + i * gapBetweenCards), Space.Self);
            }
        }
    }
}