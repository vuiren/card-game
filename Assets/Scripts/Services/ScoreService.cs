using System;

namespace Services
{
    public interface IScoreService
    {
        void AddPointToPlayer(int playerId);
        int GetPlayerPoints(int playerId);
        void OnScoreChanged(Action action);
    }
}