using Scriptable_Objects;
using Services;
using TMPro;
using UnityEngine;
using Zenject;

namespace Controllers
{
    public class BetsController : MonoBehaviour
    {
        [SerializeField] private GameObject betsUI;
        [SerializeField] private TextMeshProUGUI betsText;
        private Configuration _configuration;
        private IBetsService _betsService;
        private IPlayerService _playerService;
        private int _bet;
        
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
            betsText.text = "Ставки:\n";
            var players = _playerService.GetAllPlayers();

            foreach (var playerData in players)
            {
                betsText.text += playerData.name + ": " + playerData.betsCount + '\n';
            }
        }

        public void ShowUI()
        {
            betsUI.SetActive(true);
        }

        public void SetLocalPlayerBet()
        {
            _betsService.MakeABet(_configuration.playerId,_bet);
        }

        public void SetBet(string bet)
        {
            this._bet = int.Parse(bet);
        }
    }
}