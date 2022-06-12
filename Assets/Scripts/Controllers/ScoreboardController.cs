using System.Linq;
using Services;
using TMPro;
using UnityEngine;
using Zenject;

namespace Controllers
{
    public class ScoreboardController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI scoreboardText;
        private IPlayerService _playerService;
        private IScoreService _scoreService;
        
        [Inject]
        public void Construct(IScoreService scoreService, ISessionService sessionService, IPlayerService playerService)
        {
            _playerService = playerService;
            _scoreService = scoreService;
            scoreService.OnScoreChanged(UpdateScoreboard);
        }

        private void UpdateScoreboard()
        {
            scoreboardText.text = "Счёт побед:\n";
            var players = _playerService.GetAllPlayers().ToArray();
            foreach (var playerData in players)
            {
                var score = _scoreService.GetPlayerPoints(playerData.id);
                scoreboardText.text += playerData.name + ": " + score + '\n';
            }
        }
    }
}