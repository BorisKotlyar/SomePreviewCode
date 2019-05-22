using Zenject;

namespace Localization
{
	public class LocalizationInstaller<T> : Installer<LocalizationInstaller<T>> where T : ILocalizationController
	{
		public override void InstallBindings()
		{
			Container.Bind<ILocalizationController>()
				.To<T>()
				.AsSingle()
				.NonLazy();
		}
	}
}