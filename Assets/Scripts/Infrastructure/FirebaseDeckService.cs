using System.Collections.Generic;
using System.Linq;
using Domain;
using Services;
using UnityEngine;

namespace Infrastructure
{
    public class FirebaseDeckService : IDeckService
    {
        private readonly Dictionary<int, PlayerDeck> _deckAssignments = new();
        private readonly PlayerDeck _mainDeck;
        private readonly Queue<PlayerDeck> _playerDecks;

        public FirebaseDeckService(IEnumerable<PlayerDeck> playerDecks)
        {
            var collection = playerDecks as PlayerDeck[] ?? playerDecks.ToArray();
            _playerDecks = new Queue<PlayerDeck>(collection.Where(x => !x.mainDeck));
            _mainDeck = collection.FirstOrDefault(x => x.mainDeck);
        }

        public void AssignDeckToPlayer(int playerId)
        {
            var deck = _playerDecks.Dequeue();

            _deckAssignments.Add(playerId, deck);
        }

        public void AssignMainDeckToPlayer(int playerId)
        {
            _deckAssignments.Add(playerId, _mainDeck);
        }

        public PlayerDeck GetPlayerDeck(int playerId)
        {
            return _deckAssignments[playerId];
        }

        public void ClearPlayerDeck(int playerId)
        {
            var deck = _deckAssignments[playerId];

            if (!deck)
            {
                Debug.LogWarning("No deck found for clear");
                return;
            }

            for (var i = 0; i < deck.hand.childCount; i++)
            {
                var child = deck.hand.transform.GetChild(i);
                Object.Destroy(child.gameObject);
            }
        }
    }
}