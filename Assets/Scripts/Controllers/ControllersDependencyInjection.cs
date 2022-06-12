using UnityEngine;
using Zenject;

namespace Controllers
{
    public static class ControllersDependencyInjection
    {
        public static void Inject(DiContainer container)
        {
            var handsController = Object.FindObjectOfType<HandsController>();
            if (!handsController)
            {
                Debug.LogError("No hands controller found");
            }
            container.BindInstance(handsController).AsSingle();
            
            var centerDeckController = Object.FindObjectOfType<CenterDeckController>();
            if (!centerDeckController)
            {
                Debug.LogError("No centerDeckController found");
            }
            container.BindInstance(centerDeckController).AsSingle();
            
            var gameController = Object.FindObjectOfType<GameController>();
            if (!gameController)
            {
                Debug.LogError("No gameController found");
            }
            container.BindInstance(gameController).AsSingle();
            
            var betsController = Object.FindObjectOfType<BetsController>();
            if (!betsController)
            {
                Debug.LogError("No betsController found");
            }
            container.BindInstance(betsController).AsSingle();
        }
    }
}