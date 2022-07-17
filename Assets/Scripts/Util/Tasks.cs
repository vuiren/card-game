using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Domain.DTO;

namespace Util
{
    public static class Tasks
    {
        public static async UniTask WaitForBets(Func<IEnumerable<PlayerData>> players, Func<bool> betsSet)
        {
            var ready = false;
            while (!ready)
            {
                await Delay();

                if (betsSet())
                {
                    ready = true;
                    break;
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