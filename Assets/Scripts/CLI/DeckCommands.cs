using QFSW.QC;
using Services;
using UnityEngine;
using Zenject;

namespace CLI
{
    public class DeckCommands : MonoBehaviour
    {
        private IDeckService _deckService;
        private IPlayerService _playerService;

        [Inject]
        public void Construct(IPlayerService playerService, IDeckService deckService)
        {
            _deckService = deckService;
            _playerService = playerService;
        }

        [Command("deck.printAll")]
        public void PrintAllDecks()
        {
            var players = _playerService.GetAllPlayers();
            foreach (var playerData in players)
            {
                var deck = _deckService.GetPlayerDeck(playerData.id);
                Debug.Log($"Player '{playerData.id}' got deck '{deck.actor.id}'");
            }
        }
    }
}