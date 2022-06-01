using UnityEngine;
using Zenject;

namespace Factories
{
    public static class FactoriesDependencyInjection
    {
        public static void Inject(DiContainer container)
        {
            var cardsFactory = Object.FindObjectOfType<CardsFactory>();

            if (!cardsFactory)
            {
                Debug.LogError("No cardsFactory found");
            }

            container.BindInstance(cardsFactory).AsSingle();
        }
    }
}