using System;
using Controllers;
using Scriptable_Objects;
using Services;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Domain
{
    public class Card : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        public CardSheet cardSheet;

        private GameController _gameController;
        private ITurnsService _turnsService;
        private IPlayerService _playerService;
        private int _ownerId;

        [Inject]
        public void Construct(int ownerId, GameController gameController, ITurnsService turnsService, IPlayerService playerService)
        {
            _ownerId = ownerId;
            _turnsService = turnsService;
            _gameController = gameController;
            _playerService = playerService;
        }

        public void UpdateCard()
        {
            spriteRenderer.sprite = cardSheet.cardSprite;
        }

        private void OnMouseDown()
        {
            Debug.Log($"Click on the card, card name: '{cardSheet.cardName}'");
            if(EventSystem.current.IsPointerOverGameObject()) return;
            var currentTurn = _turnsService.CurrentTurn();
            var localPlayerId = _playerService.LocalPlayerId;
            if (currentTurn != localPlayerId || currentTurn != _ownerId)
            {
                return;
            }
            _gameController.MakeAStep(localPlayerId, cardSheet);
        }
    }
}