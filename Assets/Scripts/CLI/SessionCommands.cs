using System.Linq;
using QFSW.QC;
using Services;
using UnityEngine;
using Zenject;

namespace CLI
{
    public class SessionCommands : MonoBehaviour
    {
        private ISessionService _sessionService;
        [Inject]
        public void Construct(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }

        [Command("session.getCards")]
        public void GetCards()
        {
            var cards = _sessionService.GetCardsInSession();

            foreach (var card in cards)
            {
                Debug.Log($"Player: '{card.playerId}' with a card: '{card.playedCardId}'");
            }
        }

        [Command("sessions.getWinner")]
        public void GetWinner()
        {
            /*var winner = _sessionService.AnnounceSessionWinnerId();
            var cards = _sessionService.GetCardsInSession();
            var card = cards.FirstOrDefault(x => x.playerId == winner);
            Debug.Log($"Player: '{winner}' won with '{card.playedCardId}'");*/
        }
    }
}