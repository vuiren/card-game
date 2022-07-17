using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace Services
{
    public interface IBetsService
    {
        void MakeABet(int playerId, int bet);
        int GetPlayerBet(int playerId);
        void OnBetChanged(Action onBetsChanged);
        void OnBetPlayerChanged(Action onBetPlayerChanged);
        int GetCurrentBetPlayer();
        void SetTurnsOrder(Queue<int> turnsOrder);
        UniTask FinishMakingBet();
        bool BetsSet();
    }
}