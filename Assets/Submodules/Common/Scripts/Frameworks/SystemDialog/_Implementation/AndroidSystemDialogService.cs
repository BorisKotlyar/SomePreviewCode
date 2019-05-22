#if UNITY_ANDROID
using UnityEngine;

public class AndroidSystemDialogService : ISystemDialogService
{
	public void Show(MobileSystemDialog mobileSystemDialog)
	{
		if (Application.platform != RuntimePlatform.Android) return;

		var message = new MNPopup(mobileSystemDialog.TitleText, mobileSystemDialog.BodyText);
		foreach (var button in mobileSystemDialog.Buttons)
		{
			message.AddAction(button.ButtonLabel, () => { button.Action(); });
		}

		message.AddDismissListener(() =>
		{
			if (mobileSystemDialog.OnDissmiss != null)
			{
				mobileSystemDialog.OnDissmiss.Invoke();
			}
		});
		message.Show();
	}
}
#endif
