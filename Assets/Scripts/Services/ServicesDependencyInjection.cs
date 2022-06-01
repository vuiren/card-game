using UnityEngine;
using Zenject;

namespace Services
{
    public static class ServicesDependencyInjection
    {
        public static void Inject(DiContainer container)
        {
            var playerService = Object.FindObjectOfType<PlayerService>();
            if (!playerService)
            {
                Debug.LogError("No player service Found");
            }
            container.Bind<IPlayerService>().FromInstance(playerService).AsSingle();

            var actorsIdService = Object.FindObjectOfType<ActorsIdService>();
            if (!actorsIdService)
            {
                Debug.LogError("No actorsIdService Found");
            }
            container.Bind<IActorsIdService>().FromInstance(actorsIdService).AsSingle();

            var betsService = Object.FindObjectOfType<BetsService>();
            if (!betsService)
            {
                Debug.LogError("No betsService Found");
            }
            container.Bind<IBetsService>().FromInstance(betsService).AsSingle();

            var turnsService = Object.FindObjectOfType<TurnsService>();
            if (!turnsService)
            {
                Debug.LogError("No turnsService Found");
            }
            container.Bind<ITurnsService>().FromInstance(turnsService).AsSingle();
            
            var handsService = Object.FindObjectOfType<HandsService>();
            if (!handsService)
            {
                Debug.LogError("No handsService Found");
            }
            container.Bind<IHandsService>().FromInstance(handsService).AsSingle();
        }
    }
}