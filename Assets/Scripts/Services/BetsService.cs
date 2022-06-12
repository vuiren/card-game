using System;
using System.Collections.Generic;
using Domain;
using UnityEngine;

namespace Services
{
    public interface IBetsService
    {
        void MakeABet(int playerId, int bet);
        int GetPlayerBet(int playerId);
        void OnBetChanged(Action onBetsChanged);
    }
}