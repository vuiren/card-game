using System.Collections.Generic;
using Scriptable_Objects;
using Services;
using TMPro;
using UnityEngine;
using Zenject;

namespace Controllers
{
    public class BetsController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI playerBetsText, betText;
        private int _bet;
        private IBetsService _betsService;
        private Configuration _configuration;
        private IPlayerService _playerService;

        [Inject]
        public void Construct(Configuration configuration, IBetsService betsService, IPlayerService playerService)
        {
            _configuration = configuration;
            _betsService = betsService;
            _playerService = playerService;
            _betsService.OnBetChanged(DrawBets);
        }

        private void DrawBets()
        {
            playerBetsText.text = "Ставки:\n";
            var players = _playerService.GetAllPlayers();

            foreach (var playerData in players)
                playerBetsText.text += playerData.name + ": " + playerData.betsCount + '\n';
        }

        public void MakeABet(int playerId, int bet)
        {
            _betsService.MakeABet(playerId, bet);
        }

        public int GetPlayerBet(int playerId)
        {
            return _betsService.GetPlayerBet(playerId);
        }

        public void SetTurnsOrder(Queue<int> queue)
        {
            _betsService.SetTurnsOrder(queue);
        }

        public void SetLocalPlayerBet()
        {
            _betsService.MakeABet(_configuration.playerId, _bet);
        }

        public void AddBet()
        {
            _bet++;
            betText.text = _bet.ToString();
        }

        public void SubBet()
        {
            _bet--;
            _bet = _bet < 0 ? 0 : _bet;
            betText.text = _bet.ToString();
        }

        public bool BetsSet()
        {
            return _betsService.BetsSet();
        }

        public void SetBet(string bet)
        {
            _bet = int.Parse(bet);
        }
    }
}