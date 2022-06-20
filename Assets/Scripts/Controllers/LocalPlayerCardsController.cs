using System.Collections.Generic;
using System.Linq;
using Domain;
using UnityEngine;

namespace Controllers
{
    public class LocalPlayerCardsController : MonoBehaviour
    {
        [SerializeField]
        private List<Card> _localPlayerCards;

        public void AddCard(Card card)
        {
            _localPlayerCards.Add(card);
        }
        
        public IEnumerable<Card> GetLocalPlayerCards()
        {
            return _localPlayerCards;
        }

        public void SetLocalPlayerCards(IEnumerable<Card> cards)
        {
            _localPlayerCards = cards.ToList();
        }
    }
}