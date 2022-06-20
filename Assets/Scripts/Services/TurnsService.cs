using System;
using System.Collections.Generic;
using Domain.DTO;

namespace Services
{
    public interface ITurnsService
    {
        void SetTurnsOrder(Queue<int> players);
        void NextTurn();
        int CurrentTurn();
        void OnTurnChange(Action<int> action);
        IEnumerable<int> GetLastTurnsOrder();
    }
}