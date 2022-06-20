using System.Collections.Generic;
using System.Linq;
using Domain;
using UnityEngine;

namespace Services
{
    public interface IDeckService
    {
        void AssignDeckToPlayer(int playerId);
        void AssignMainDeckToPlayer(int playerId);
        PlayerDeck GetPlayerDeck(int playerId);
        void ClearPlayerDeck(int playerId);
    }

    public class DeckService : IDeckService
    {
        private readonly PlayerDeck[] _allDecks;

        private readonly Queue<PlayerDeck> _decks;
        private readonly Dictionary<int, PlayerDeck> _playerDecks = new();

        public DeckService(IEnumerable<PlayerDeck> decks)
        {
            var playerDecks = decks as PlayerDeck[] ?? decks.ToArray();
            _allDecks = playerDecks.ToArray();
            _decks = new Queue<PlayerDeck>(playerDecks);
        }

        public void AssignDeckToPlayer(int playerId)
        {
            if (_playerDecks.ContainsKey(playerId))
            {
                Debug.LogWarning($"Deck for player: '{playerId}' is already assigned");
                return;
            }

            _playerDecks.Add(playerId, _decks.Dequeue());
        }

        public void AssignMainDeckToPlayer(int playerId)
        {
        }

        public PlayerDeck GetPlayerDeck(int playerId)
        {
            if (!_playerDecks.ContainsKey(playerId))
            {
                Debug.LogWarning($"Player '{playerId}' got no deck assigned");
                return null;
            }

            return _playerDecks[playerId];
        }

        public void ClearPlayerDeck(int deckId)
        {
            var deck = _allDecks.FirstOrDefault(x => x.actor.id == deckId)?.hand;

            if (!deck)
            {
                Debug.LogWarning("No deck found for clear");
                return;
            }

            for (var i = 0; i < deck.transform.childCount; i++)
            {
                var child = deck.transform.GetChild(i);
                Object.Destroy(child.gameObject);
            }
        }
    }
}