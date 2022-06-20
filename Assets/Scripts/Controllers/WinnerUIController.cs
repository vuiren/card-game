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
            Debug.Log("==============NOOOOOTICE ME!!!!============");
            Debug.Log("Check 1!!!!");
            var player = _playerService.GetPlayer(obj);
            Debug.Log("Check 2!!!!");

            winnerNameText.text = $"Игрок {player.name}!";
            Debug.Log("Check 3!!!!");

            WinnerShowing();
        }

        private async UniTask WinnerShowing()
        {
            Debug.Log("Check 4!!!!");

            winnerUI.SetActive(true);
            await Tasks.Delay(2000);
            Debug.Log("Check 51!!!!");

            winnerUI.SetActive(false);
        }
    }
}