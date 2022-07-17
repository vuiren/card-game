using UnityEngine;
using Zenject;

namespace Controllers
{
    public static class ControllersDependencyInjection
    {
        public static void Inject(DiContainer container)
        {
            InjectController<HandsController>(container);
            InjectController<CenterDeckController>(container);
            InjectController<GameController>(container);
            InjectController<BetsController>(container);
            InjectController<WaitingAnimationController>(container);
            InjectController<LocalPlayerCardsController>(container);
            InjectController<WeightsController>(container);
            InjectController<WeightsUIController>(container);
            InjectController<PlayerListController>(container);
        }

        private static void InjectController<T>(DiContainer container) where T : MonoBehaviour
        {
            var controllerInstance = Object.FindObjectOfType<T>();
            if (!controllerInstance) Debug.LogError($"Controller typeof '{typeof(T)}' not found");
            container.BindInstance(controllerInstance).AsSingle();
        }
    }
}