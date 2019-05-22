using System;
using System.Collections.Generic;
using Localization;
using UnityEngine;
using Zenject;

public class EditorInternetConnection : IInternetConnection
{

	[Inject] private readonly IDebugService _debugService;
	[Inject] private readonly ISystemDialogService _systemDialogService;
	[Inject] private readonly ILocalizationController _localizationController;

	public bool IsInternetExists
	{
		get
		{
			return PlayerPrefs.GetInt("EDITOR_INTERNET_EXISTS_CHECKER", 0) == 0 &&
			       Application.internetReachability != NetworkReachability.NotReachable;
		}
	}

	public void CheckConnection(Action onSuccess, Action onFail,
		LostInternetMessage messageType = LostInternetMessage.none)
	{
		if (IsInternetExists)
		{
			if (onSuccess != null)
			{
				onSuccess();
			}
		}
		else
		{
			if (onFail != null)
			{
				onFail();
			}
			ShowLostInternetMessage(messageType);
		}
	}

	public virtual void ShowLostInternetMessage(LostInternetMessage messageType, Action onClose = null)
	{
		if (messageType == LostInternetMessage.none)
		{
			return;
		}
		switch (messageType)
		{
			case LostInternetMessage.buy:
				_systemDialogService.Show (new MobileSystemDialog (
					_localizationController.GetText("Common.Allert.Attention"),
					_localizationController.GetText("InternetLost.Buy"),
					new List<MobileSystemDialogButton>
					{
						new MobileSystemDialogButton("OK", () =>
						{
							if (onClose != null) onClose.Invoke();
						})
					}
					));
				break;
			case LostInternetMessage.restore:
				_systemDialogService.Show (new MobileSystemDialog (
					_localizationController.GetText("Common.Allert.Attention"),
					_localizationController.GetText("InternetLost.Restore"),
					new List<MobileSystemDialogButton>
					{
						new MobileSystemDialogButton("OK", () =>
						{
							if (onClose != null) onClose.Invoke();
						})
					}
				));
				break;
			case LostInternetMessage.transition:
				_systemDialogService.Show (new MobileSystemDialog (
					_localizationController.GetText("Common.Allert.Attention"),
					_localizationController.GetText("InternetLost.Transition"),
					new List<MobileSystemDialogButton>
					{
						new MobileSystemDialogButton("OK", () =>
						{
							if (onClose != null) onClose.Invoke();
						})
					}
				));
				break;
			case LostInternetMessage.watchAds:
				_systemDialogService.Show (new MobileSystemDialog (
					_localizationController.GetText("Common.Allert.Attention"),
					_localizationController.GetText("InternetLost.Transition.WatchAds"),
					new List<MobileSystemDialogButton>
					{
						new MobileSystemDialogButton("OK", () =>
						{
							if (onClose != null) onClose.Invoke();
						})
					}
				));
				break;
		}
	}
}
