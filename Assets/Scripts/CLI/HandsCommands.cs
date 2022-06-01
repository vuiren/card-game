using Controllers;
using QFSW.QC;
using Services;
using UnityEngine;
using Zenject;

namespace CLI
{
    public class HandsCommands : MonoBehaviour
    {
        private HandsController _handsController;
        private IPlayerService _playerService;
        
        [Inject]
        public void Construct(HandsController handsController, IPlayerService playerService)
        {
            _handsController = handsController;
            _playerService = playerService;
        }

        [Command("hands.createHand")]
        public void CreateHand(int playerId, int cardsCount)
        {
            var player = _playerService.GetPlayer(playerId);
            _handsController.SetRandomHandForPlayer(player, cardsCount);
        }

        [Command("hands.getHand")]
        public void GetHand(int playerId)
        {
            var player = _playerService.GetPlayer(playerId);
            var hand=_handsController.GetHand(player);

            foreach (var card in hand)
            {
                Debug.Log(card.cardSheet);
            }
        }
    }
}