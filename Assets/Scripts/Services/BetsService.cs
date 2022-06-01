﻿using System.Collections.Generic;
using Game_Code.Domain;
using UnityEngine;

namespace Services
{
    public interface IBetsService
    {
        void MakeBet(Player player, int bet);
        void ClearBet(Player player);
    }
    
    public class BetsService : MonoBehaviour, IBetsService
    {
        private readonly Dictionary<int, int> _bets = new();

        public void MakeBet(Player player, int bet)
        {
            if (_bets.ContainsKey(player.actor.id))
            {
                Debug.LogWarning($"Bet by player with id: '{player.actor.id}' already done");
                return;
            }
            
            _bets.Add(player.actor.id, bet);
        }

        public void ClearBet(Player player)
        {
            if (_bets.ContainsKey(player.actor.id))
            {
                _bets.Remove(player.actor.id);
                Debug.Log("Bet cleared");
                return;
            }
            else
            {
                Debug.LogWarning("Bet not found");
            }
        }
    }
}