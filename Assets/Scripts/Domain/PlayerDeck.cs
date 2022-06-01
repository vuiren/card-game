using System;
using UnityEngine;

namespace Domain
{
    public class PlayerDeck : MonoBehaviour
    { 
        public Transform hand;
        [SerializeField] private float gapBetweenCards;

        public void StructureHand()
        {
            var firstX =- hand.childCount / gapBetweenCards;

            for (var i = 0; i < hand.childCount; i++)
            {
                var child = hand.GetChild(i);
                child.position = hand.position;
                child.Translate(Vector3.right * (firstX + i * gapBetweenCards), Space.Self);
            }
        }
    }
}