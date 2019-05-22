using Zenject;

public class InternetConnectionInstaller<T> : Installer<InternetConnectionInstaller<T>> where T : IInternetConnection
{
    public override void InstallBindings()
    {
        Container.Bind<IInternetConnection>()
            #if UNITY_EDITOR
            .To<EditorInternetConnection>()
            #else
            .To<T>()
            #endif
            .AsSingle()
            .NonLazy();
    }
}
