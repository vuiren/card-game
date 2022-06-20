using Scriptable_Objects;
using Zenject;

namespace Factories
{
    public static class FactoriesDependencyInjection
    {
        public static void Inject(Configuration configuration, DiContainer container)
        {
            container.BindInstance(new CardsFactory(configuration, container)).AsSingle();
        }
    }
}