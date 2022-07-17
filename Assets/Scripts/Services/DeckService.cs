using System.Collections.Generic;
using System.Linq;
using Domain;
using UnityEngine;

namespace Services
{
    public interface IDeckService
    {
        void AssignDeckToPlayer(int playerId);
        void AssignMainDeckToPlayer(int playerId);
        PlayerDeck GetPlayerDeck(int playerId);
        void ClearPlayerDeck(int playerId);
    }
}