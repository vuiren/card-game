using System;
using System.Collections.Generic;
using System.Linq;
using Scriptable_Objects;
using Services;
using TMPro;
using UnityEngine;
using Util;
using Zenject;

namespace Controllers
{
    public class WeightsUIController : MonoBehaviour
    {
        [SerializeField] private GameObject uiRoot;
        [SerializeField] private TextMeshPro leftPotScore, rightPotScore, playerScore, currentPlayerText;
        [SerializeField] private SpriteButton incButton, decButton, submitButton;
        private WeightsController _weightsController;
        private int _playerScore;
        private IBetsService _betsService;
        private IPlayerService _playerService;
        private int _currentPlayerId;
        private Configuration _configuration;
        [SerializeField]
        private int originalLeftWeight;

        [Inject]
        public void Construct(Configuration configuration, WeightsController weightsController,
            IPlayerService playerService,
            IBetsService betsService)
        {
            _weightsController = weightsController;
            _playerService = playerService;
            _betsService = betsService;
            _configuration = configuration;
            _weightsController.OnRightWeightUpdate += UpdateUI;
            _betsService.OnBetChanged(HandleChange);
            _betsService.OnBetPlayerChanged(HandleBetPlayerChange);
        }

        private void HandleBetPlayerChange()
        {
            _currentPlayerId = _betsService.GetCurrentBetPlayer();
            var players = _playerService.GetAllPlayers();
            var bets = players.Select(x => _betsService.GetPlayerBet(x.id)).Where(x => x >= 0);
            var score = bets.Sum();
            score = Mathf.Clamp(score, 0, score);
            Debug.Log($"Score is: {score}");
            leftPotScore.text = score.ToString();
            _weightsController.SetLeftWeight(score, false);
            originalLeftWeight = score;

            var isCurrentPlayerTurn = _configuration.playerId == _currentPlayerId;

            incButton.gameObject.SetActive(isCurrentPlayerTurn);
            decButton.gameObject.SetActive(isCurrentPlayerTurn);
            submitButton.gameObject.SetActive(isCurrentPlayerTurn);
            playerScore.gameObject.SetActive(isCurrentPlayerTurn);

            var player = _playerService.GetPlayer(_currentPlayerId);
            currentPlayerText.gameObject.SetActive(!isCurrentPlayerTurn);
            currentPlayerText.text = $"Bet by '{player.name}'";
            var playerColor = _configuration.PlayerColors.FirstOrDefault(x => x.playerId == _currentPlayerId);
            if(playerColor == null) return;
            currentPlayerText.color = playerColor.Color;
        }

        private void HandleChange()
        {
            var players = _playerService.GetAllPlayers();
            var bets = players.Select(x => _betsService.GetPlayerBet(x.id)).Where(x => x >= 0);
            var score = bets.Sum();
            score = Mathf.Clamp(score, 0, score);
            Debug.Log($"Score is: {score}");
            leftPotScore.text = score.ToString();
            _weightsController.SetLeftWeight(score, false);
        }

        private void UpdateUI()
        {
            rightPotScore.text = _weightsController.GetRightPotWeight().ToString();
        }

        public void ShowUI()
        {
            uiRoot.SetActive(true);
            var isCurrentPlayerTurn = _configuration.playerId == _currentPlayerId;
            originalLeftWeight = 0;
            _weightsController.SetLeftWeight(0);
            _playerScore = 0;
            playerScore.text = "0";
            incButton.gameObject.SetActive(isCurrentPlayerTurn);
            decButton.gameObject.SetActive(isCurrentPlayerTurn);
            submitButton.gameObject.SetActive(isCurrentPlayerTurn);
            playerScore.gameObject.SetActive(isCurrentPlayerTurn);
        }

        public void HideUI()
        {
            uiRoot.SetActive(false);
        }

        private void Start()
        {
            playerScore.text = "0";
            rightPotScore.text = "10";
            incButton.onPressed.AddListener(() =>
            {
                if (_currentPlayerId != _configuration.playerId) return;
                _playerScore++;
                UpdateScore();
            });

            decButton.onPressed.AddListener(() =>
            {
                if (_currentPlayerId != _configuration.playerId) return;
                _playerScore--;
                UpdateScore();
            });

            submitButton.onPressed.AddListener(ConfirmScore);
        }

        private void UpdateScore()
        {
            _playerScore = Mathf.Clamp(_playerScore, 0, _playerScore);
            _weightsController.SetLeftWeight(originalLeftWeight + _playerScore);
            _betsService.MakeABet(_currentPlayerId, _playerScore);
            leftPotScore.text = _weightsController.GetLeftPotWeight().ToString();
            playerScore.text = _playerScore.ToString();
            var overweight = _weightsController.GetLeftPotWeight() > _weightsController.GetRightPotWeight();
            submitButton.gameObject.SetActive(!overweight);
        }
        
        
        private async void ConfirmScore()
        {
            if (_currentPlayerId != _configuration.playerId) return;
            _betsService.MakeABet(_currentPlayerId, _playerScore);
            _playerScore = 0;
            await _betsService.FinishMakingBet();
        }
    }
}