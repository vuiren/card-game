using QFSW.QC;
using Services;
using UnityEngine;
using Zenject;

namespace CLI
{
    public class TurnsCommands : MonoBehaviour
    {
        private ITurnsService _turnsService;

        [Inject]
        public void Construct(ITurnsService turnsService)
        {
            _turnsService = turnsService;
        }

        [Command("turns.current")]
        public void CurrentTurn()
        {
            var current = _turnsService.CurrentTurn();

            Debug.Log($"Current turn: '{current}'");
        }

        [Command("turns.next")]
        public void NextTurn()
        {
            _turnsService.NextTurn();

            CurrentTurn();
        }
    }
}