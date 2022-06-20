using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Domain.DTO;

namespace Util
{
    public static class Tasks
    {
        public static async UniTask WaitForBets(Func<IEnumerable<PlayerData>> players, Func<int, int> getPlayerBet)
        {
            var ready = false;
            while (!ready)
            {
                await Delay();

                foreach (var playerData in players())
                {
                    var bet = getPlayerBet(playerData.id);
                    if (bet == -1)
                    {
                        ready = false;
                        break;
                    }

                    ready = true;
                }
            }
        }

        public static async UniTask Delay(int delayMS = 1000)
        {
            await UniTask.Delay(delayMS);
        }

        public static async UniTask WaitForHost(Func<bool> isHostReady)
        {
            while (!isHostReady()) await UniTask.Delay(1000);
        }
    }
}