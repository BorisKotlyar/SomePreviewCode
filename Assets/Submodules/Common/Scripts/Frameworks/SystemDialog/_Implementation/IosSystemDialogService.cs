#if UNITY_IOS
using UnityEngine;
using System.Linq;
using Prime31;

public class IosSystemDialogService : ISystemDialogService
{
	private MobileSystemDialog _mobileSystemDialog;
	
	public void Show(MobileSystemDialog mobileSystemDialog)
	{
		if (Application.platform != RuntimePlatform.IPhonePlayer) return;
		_mobileSystemDialog = mobileSystemDialog;
		
		EtceteraManager.alertButtonClickedEvent += OnDialogCloseIphone;
		EtceteraBinding.showAlertWithTitleMessageAndButtons(
			_mobileSystemDialog.TitleText, _mobileSystemDialog.BodyText,
			_mobileSystemDialog.Buttons.Select(a => a.ButtonLabel).ToArray());
	}
	
	private void OnDialogCloseIphone(string pressedButton)
	{
		if (Application.platform != RuntimePlatform.IPhonePlayer) return;
		
		EtceteraManager.alertButtonClickedEvent -= OnDialogCloseIphone;
		var button = _mobileSystemDialog.Buttons.FirstOrDefault(a => a.ButtonLabel == pressedButton);
		if (button != null)
		{
			button.Action();
		}
	}
}
#endif