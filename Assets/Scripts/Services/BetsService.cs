using System;

namespace Services
{
    public interface IBetsService
    {
        void MakeABet(int playerId, int bet);
        int GetPlayerBet(int playerId);
        void OnBetChanged(Action onBetsChanged);
    }
}