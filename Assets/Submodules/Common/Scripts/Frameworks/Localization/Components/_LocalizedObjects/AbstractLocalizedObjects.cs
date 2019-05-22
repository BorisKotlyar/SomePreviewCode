using UnityEngine;
using Zenject;

namespace Localization
{
	public abstract class AbstractLocalizedObjects : MonoBehaviour
	{
		[Inject] protected readonly ILocalizationController _localizationController;

		private void OnEnable()
		{
			_localizationController.OnLocalizationChange += Refresh;
			Refresh(_localizationController.CurrentLanguageCode);
		}

		private void OnDisable()
		{
			_localizationController.OnLocalizationChange -= Refresh;
		}

		protected abstract void Refresh(string languageCode);
	}
}