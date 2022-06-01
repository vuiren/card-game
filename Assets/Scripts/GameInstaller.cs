using CLI;
using Controllers;
using Scriptable_Objects;
using Services;
using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private Configuration configuration;
    public override void InstallBindings()
    {
        Container.BindInstance(configuration).AsSingle();
        ServicesDependencyInjection.Inject(Container);
        ControllersDependencyInjection.Inject(Container);
    }
}