using UnityEngine;
using Zenject;

namespace Controllers
{
    public static class ControllersDependencyInjection
    {
        public static void Inject(DiContainer container)
        {
            var playersController = Object.FindObjectOfType<PlayersController>();
            if (!playersController)
            {
                Debug.LogError("No players controller found");
            }
            container.BindInstance(playersController).AsSingle();
            
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
        }
    }
}