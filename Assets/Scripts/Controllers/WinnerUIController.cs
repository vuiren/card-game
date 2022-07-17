using Cysharp.Threading.Tasks;
using Services;
using TMPro;
using UnityEngine;
using Util;
using Zenject;

namespace Controllers
{
    public class WinnerUIController : MonoBehaviour
    {
        [SerializeField] private GameObject winnerUI;
        [SerializeField] private TextMeshProUGUI winnerNameText;
        private IPlayerService _playerService;
        [Inject]
        public void Construct(IPlayerService playerService, ISessionService sessionService)
        {
            _playerService = playerService;
            sessionService.OnSessionWinnerAnnounced(ShowWinner);
        }

        private void ShowWinner(int obj)
        {
            var player = _playerService.GetPlayer(obj);
            winnerNameText.text = $"Игрок {player.name}!";
            WinnerShowing();
        }

        private async UniTask WinnerShowing()
        {
            winnerUI.SetActive(true);
            await Tasks.Delay(2000);
            winnerUI.SetActive(false);
        }
    }
}