using System;
using System.Collections.Generic;
using UnityEngine;

namespace Services
{
    public interface ITurnsService
    {
        void SetTurnsOrder(Queue<int> players);
        void NextTurn();
        int CurrentTurn();
        void OnTurnChange(Action<int> action);
    }

    public class TurnsService : ITurnsService
    {
        private Queue<int> _players;
        private int _currentPlayer;

        public void SetTurnsOrder(Queue<int> players)
        {
            _players = players;
            _currentPlayer = players.Peek();
        }

        public void NextTurn()
        {
            if (_players.Count == 0)
            {
                Debug.LogWarning("No players in queue left");
                _currentPlayer = -1;
                return;
            }
            
            _currentPlayer = _players.Dequeue();
        }

        public int CurrentTurn()
        {
            return _currentPlayer;
        }

        public void OnTurnChange(Action<int> action)
        {
            
        }
    }
}