using Scriptable_Objects;
using UnityEngine;
using Zenject;

namespace Factories
{
    public static class FactoriesDependencyInjection
    {
        public static void Inject(DiContainer container)
        {
            var configuration = container.Resolve<Configuration>();
            container.BindInstance(new CardsFactory(configuration)).AsSingle();
        }
    }
}