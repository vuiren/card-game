using Controllers;
using QFSW.QC;
using UnityEngine;
using Zenject;

namespace CLI
{
    public class HandsCommands : MonoBehaviour
    {
        private HandsController _handsController;

        [Inject]
        public void Construct(HandsController handsController)
        {
            _handsController = handsController;
        }

        [Command("hands.createHand")]
        public void CreateHand(int playerId, int cardsCount)
        {
            _handsController.SetRandomHandForPlayer(playerId, cardsCount);
        }

        [Command("hands.getHand")]
        public void GetHand(int playerId)
        {
            var hand = _handsController.GetHand(playerId);

            foreach (var card in hand) Debug.Log(card);
        }
    }
}