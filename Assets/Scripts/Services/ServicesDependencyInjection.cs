using UnityEngine;
using Zenject;

namespace Services
{
    public static class ServicesDependencyInjection
    {
        public static void Inject(DiContainer container)
        {
            container.Bind<IPlayerService>().FromInstance(new PlayerService()).AsSingle();
            container.Bind<IActorsIdService>().FromInstance(new ActorsIdService()).AsSingle();
            container.Bind<IBetsService>().FromInstance(new BetsService()).AsSingle();
            container.Bind<ITurnsService>().FromInstance(new TurnsService()).AsSingle();
            container.Bind<IHandsService>().FromInstance(new HandsService()).AsSingle();
            container.Bind<ICenterDeckService>().FromInstance(new CenterDeckService()).AsSingle();
        }
    }
}