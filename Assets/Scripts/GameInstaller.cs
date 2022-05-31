using CLI;
using Controllers;
using Services;
using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        ServicesDependencyInjection.Inject(Container);
        ControllersDependencyInjection.Inject(Container);
    }
}