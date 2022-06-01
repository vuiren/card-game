using System.Collections.Generic;
using Game_Code.Domain;
using UnityEngine;

namespace Services
{
    public interface ITurnsService
    {
        void SetTurnsOrder(Queue<Player> players);
        void NextTurn();
        Player CurrentTurn();
    }

    public class TurnsService : ITurnsService
    {
        private Queue<Player> _players;
        private Player _currentPlayer;

        public void SetTurnsOrder(Queue<Player> players)
        {
            _players = players;
        }

        public void NextTurn()
        {
            _currentPlayer = _players.Dequeue();
        }

        public Player CurrentTurn()
        {
            return _currentPlayer;
        }
    }
}