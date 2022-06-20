using Controllers;
using QFSW.QC;
using Services;
using UnityEngine;
using Zenject;

namespace CLI
{
    public class CenterDeckCommands : MonoBehaviour
    {
        private CenterDeckController _centerDeckController;
        private ICenterDeckService _centerDeckService;

        [Inject]
        public void Construct(CenterDeckController centerDeckController, ICenterDeckService centerDeckService)
        {
            _centerDeckController = centerDeckController;
            _centerDeckService = centerDeckService;
        }

        [Command("cards.trumpCard")]
        public void PrintTrumpCard()
        {
            Debug.Log($"Trump card: '{_centerDeckService.GetTrumpCard().cardName}'");
        }

        [Command("cards.countCardsInCenter")]
        public void CountCardsInCenter()
        {
            var cards = _centerDeckService.GetAllCardsInCenter();

            foreach (var cardSheet in cards) Debug.Log($"Card: '{cardSheet.cardName}'");

            Debug.Log($"{cards.Length} карт в центре");
        }
    }
}