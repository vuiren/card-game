using System.Collections.Generic;
using System.Linq;
using Domain;
using Game_Code.Domain;
using Unity.VisualScripting;
using UnityEngine;

namespace Services
{
    public interface IDeckService
    {
        void AssignDeckToPlayer(Player player);
        PlayerDeck GetPlayerDeck(Player player);
        void SetDeck(int deckId, Card[] cards);
        void ClearDeck(int deckId);
    }
    
    public class DeckService: IDeckService
    {
        private readonly Dictionary<int, PlayerDeck> _playerDecks = new();
        private readonly PlayerDeck[] _allDecks;
        
        private readonly Queue<PlayerDeck> _decks;
        
        public DeckService(IEnumerable<PlayerDeck> decks)
        {
            var playerDecks = decks as PlayerDeck[] ?? decks.ToArray();
            _allDecks = playerDecks.ToArray();
            _decks = new Queue<PlayerDeck>(playerDecks);    
        }
        
        public void AssignDeckToPlayer(Player player)
        {
            if (_playerDecks.ContainsKey(player.actor.id))
            {
                Debug.LogWarning($"Deck for player: '{player.actor.id}' is already assigned");
                return;
            }
            
            _playerDecks.Add(player.actor.id, _decks.Dequeue());
        }

        public PlayerDeck GetPlayerDeck(Player player)
        {
            if (!_playerDecks.ContainsKey(player.actor.id))
            {
                Debug.LogWarning($"Player '{player.actor.id}' got no deck assigned");
                return null;
            }

            return _playerDecks[player.actor.id];
        }

        public void SetDeck(int deckId, Card[] cards)
        {
            var deck = _allDecks.FirstOrDefault(x => x.actor.id == deckId);
        }

        public void ClearDeck(int deckId)
        {
            var deck = _allDecks.FirstOrDefault(x => x.actor.id == deckId).hand;

            if (!deck)
            {
                Debug.LogWarning("No deck found for clear");
                return;
            }
            
            for (int i = 0; i < deck.transform.childCount; i++)
            {
                var child = deck.transform.GetChild(i);
                Object.Destroy(child.gameObject);
            }
        }
    }
}