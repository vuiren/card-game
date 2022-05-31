using UnityEngine;
using Zenject;

namespace Services
{
    public static class ServicesDependencyInjection
    {
        public static void Inject(DiContainer container)
        {
            var playerService = Object.FindObjectOfType<PlayerService>();
            container.Bind<IPlayerService>().FromInstance(playerService).AsSingle();

            var actorsIdService = Object.FindObjectOfType<ActorsIdService>();
            container.Bind<IActorsIdService>().FromInstance(actorsIdService).AsSingle();

            var betsService = Object.FindObjectOfType<BetsService>();
            container.Bind<IBetsService>().FromInstance(betsService).AsSingle();

            var turnsService = Object.FindObjectOfType<TurnsService>();
            container.Bind<ITurnsService>().FromInstance(turnsService).AsSingle();
        }
    }
}