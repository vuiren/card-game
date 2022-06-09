using CLI;
using Controllers;
using Factories;
using Scriptable_Objects;
using Services;
using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private Configuration configuration;
    public override void InstallBindings()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available) {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                Container.BindInstance(Firebase.FirebaseApp.DefaultInstance).AsSingle();

                // Set a flag here to indicate whether Firebase is ready to use by your app.
            } else {
                Debug.LogError($"Could not resolve all Firebase dependencies: {dependencyStatus}");
                // Firebase Unity SDK is not safe to use here.
            }
        });
        
        Container.BindInstance(configuration).AsSingle();
        ServicesDependencyInjection.Inject(Container);
        ControllersDependencyInjection.Inject(Container);
        FactoriesDependencyInjection.Inject(Container);
    }
}