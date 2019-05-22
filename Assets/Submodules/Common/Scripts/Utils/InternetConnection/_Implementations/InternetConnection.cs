using UnityEngine;
using System;
using System.Collections.Generic;
using Localization;
using Zenject;

public class InternetConnection : IInternetConnection
{
	[Inject] private readonly ILocalizationController _localizationController;
	[Inject] private readonly ISystemDialogService _systemDialogService;
	
	public bool IsInternetExists
	{
		get { return Application.internetReachability != NetworkReachability.NotReachable; }
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
		if (messageType == LostInternetMessage.none) return;

		MobileSystemDialog errorMessage = null;
		switch (messageType)
		{
			case LostInternetMessage.buy:
				errorMessage = new MobileSystemDialog(
					_localizationController.GetText("Common.Allert.Attention"),
					_localizationController.GetText("InternetLost.Buy"),
					new List<MobileSystemDialogButton>
					{
						new MobileSystemDialogButton("OK", () =>
						{
							if (onClose != null) onClose.Invoke();
						})
					}, () =>
					{
						if (onClose != null) onClose.Invoke();
					});
				break;
			case LostInternetMessage.restore:
				errorMessage = new MobileSystemDialog(
					_localizationController.GetText("Common.Allert.Attention"),
					_localizationController.GetText("InternetLost.Restore"),
					new List<MobileSystemDialogButton>
					{
						new MobileSystemDialogButton("OK", () =>
						{
							if (onClose != null) onClose.Invoke();
						})
					}, () =>
					{
						if (onClose != null) onClose.Invoke();
					});
				break;
			case LostInternetMessage.transition:
				errorMessage = new MobileSystemDialog(
					_localizationController.GetText("Common.Allert.Attention"),
					_localizationController.GetText("InternetLost.Transition"),
					new List<MobileSystemDialogButton>
					{
						new MobileSystemDialogButton("OK", () =>
						{
							if (onClose != null) onClose.Invoke();
						})
					}, () =>
					{
						if (onClose != null) onClose.Invoke();
					});
				break;
			case LostInternetMessage.watchAds:
				errorMessage = new MobileSystemDialog(
					_localizationController.GetText("Common.Allert.Attention"),
					_localizationController.GetText("InternetLost.Transition.WatchAds"),
					new List<MobileSystemDialogButton>
					{
						new MobileSystemDialogButton("OK", () =>
						{
							if (onClose != null) onClose.Invoke();
						})
					}, () =>
					{
						if (onClose != null) onClose.Invoke();
					});
				break;
		}

		_systemDialogService.Show(errorMessage);
	}
}