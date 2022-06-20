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
        [SerializeField] private Color activeColor, inactiveColor;
        [SerializeField] private SpriteRenderer cardModel;
        public CardSheet cardSheet;
        public bool cardActive;

        private GameController _gameController;
        private int _ownerId;
        private IPlayerService _playerService;
        private ITurnsService _turnsService;

        private void OnMouseDown()
        {
            Debug.Log($"Click on the card, card name: '{cardSheet.cardName}'");
            if (!cardActive || EventSystem.current.IsPointerOverGameObject()) return;
            var currentTurn = _turnsService.CurrentTurn();
            var localPlayerId = _playerService.LocalPlayerId;
            if (currentTurn != localPlayerId || currentTurn != _ownerId) return;
            _gameController.MakeAStep(localPlayerId, cardSheet);
        }

        [Inject]
        public void Construct(int ownerId, bool active, GameController gameController,
            ITurnsService turnsService, IPlayerService playerService)
        {
            SetCardActive(active);
            _ownerId = ownerId;
            _turnsService = turnsService;
            _gameController = gameController;
            _playerService = playerService;
        }

        public void SetCardActive(bool active)
        {
            cardModel.color = active ? activeColor : inactiveColor;
            cardActive = active;
        }

        public void UpdateCard()
        {
            spriteRenderer.sprite = cardSheet.cardSprite;
        }
    }
}