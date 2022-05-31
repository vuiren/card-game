using UnityEngine;
using Zenject;

namespace Controllers
{
    public static class ControllersDependencyInjection
    {
        public static void Inject(DiContainer container)
        {
            var playersController = Object.FindObjectOfType<PlayersController>();
            container.BindInstance(playersController).AsSingle();
        }
    }
}