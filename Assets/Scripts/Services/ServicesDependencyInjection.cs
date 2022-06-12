using Domain;
using Infrastructure;
using Scriptable_Objects;
using UnityEngine;
using Zenject;

namespace Services
{
    public static class ServicesDependencyInjection
    {
        public static void Inject(Configuration configuration, DiContainer container)
        {
            container.Bind<IPlayerService>().FromInstance(new FirebasePlayerService(configuration)).AsSingle();
            container.Bind<IBetsService>().FromInstance(new FirebaseBetsService(configuration)).AsSingle();
            container.Bind<ITurnsService>().FromInstance(new FirebaseTurnsService(configuration)).AsSingle();
            container.Bind<IHandsService>().FromInstance(new FirebaseHandsService(configuration)).AsSingle();
            container.Bind<IScoreService>().FromInstance(new FirebaseScoreService(configuration)).AsSingle();

            var centerDeckService = new FirebaseCenterDeckService(configuration);
            container.Bind<ICenterDeckService>().FromInstance(centerDeckService).AsSingle();
            container.Bind<ISessionService>().FromInstance(new FirebaseSessionService(configuration, centerDeckService)).AsSingle();

            var decks = Object.FindObjectsOfType<PlayerDeck>();
            container.Bind<IDeckService>().FromInstance(new FirebaseDeckService(decks)).AsSingle();

            container.Bind<IGameService>().FromInstance(new FirebaseGameService(configuration)).AsSingle();
        }
    }
}