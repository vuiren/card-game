using Domain;
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

            var centerDeckService = new CenterDeckService();
            container.Bind<ICenterDeckService>().FromInstance(centerDeckService).AsSingle();
            container.Bind<ISessionService>().FromInstance(new SessionService(centerDeckService)).AsSingle();

            var decks = Object.FindObjectsOfType<PlayerDeck>();
            container.Bind<IDeckService>().FromInstance(new DeckService(decks)).AsSingle();
        }
    }
}