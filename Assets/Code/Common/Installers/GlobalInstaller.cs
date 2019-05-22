using System.Linq;
using GameSettings;
using Localization;
using UnityEngine;
using UserData;
using Zenject;

public class GlobalInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Application.targetFrameRate = 60;

        InstallSubModules();

        InternetConnectionInstaller<InternetConnection>.Install(Container);
        LocalizationInstaller<GoogleSheetsLocalizationController>.Install(Container);

        InstallSettings();
        InstallUserData();
    }

    private void InstallSubModules()
    {
        Container.Bind<ISystemDialogService>()
#if UNITY_EDITOR
            .To<EditorSystemDialogService>()
#elif UNITY_ANDROID
            .To<AndroidSystemDialogService>()
#elif UNITY_IOS
            .To<IosSystemDialogService>()
#else
            .To<DefaultSystemDialogService>()
#endif
            .AsSingle();

        Container.Bind(new[] { typeof(IDebugService) }.Concat(typeof(IDebugService).GetInterfaces()))
            .To<DebugService>()
            .AsSingle()
            .NonLazy();
    }

    private void InstallSettings()
    {
        Container.Bind<Settings>()
            .AsSingle()
            .NonLazy();
    }

    private void InstallUserData()
    {
        Container.BindInterfacesAndSelfTo<LocalUser>()
            .AsSingle()
            .NonLazy();
    }
}
