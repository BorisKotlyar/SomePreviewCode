#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using Zenject;

public class EditorSystemDialogService : ISystemDialogService
{
    [Inject] private IDebugService _debugService;

    public void Show(MobileSystemDialog mobileSystemDialog)
    {
        if (mobileSystemDialog.Buttons.Count <= 0 || mobileSystemDialog.Buttons.Count > 3 ||
            !(Application.platform == RuntimePlatform.WindowsEditor ||
              Application.platform == RuntimePlatform.OSXEditor ||
              Application.platform == RuntimePlatform.LinuxEditor)) return;

        switch (mobileSystemDialog.Buttons.Count)
        {
            case 1:
                if (EditorUtility.DisplayDialog(mobileSystemDialog.TitleText, mobileSystemDialog.BodyText,
                    mobileSystemDialog.Buttons[0].ButtonLabel, string.Empty))
                {
                    mobileSystemDialog.Buttons[0].Action();
                }

                break;
            case 2:
                if (EditorUtility.DisplayDialog(mobileSystemDialog.TitleText, mobileSystemDialog.BodyText,
                    mobileSystemDialog.Buttons[0].ButtonLabel, mobileSystemDialog.Buttons[1].ButtonLabel))
                {
                    mobileSystemDialog.Buttons[0].Action();
                }
                else
                {
                    mobileSystemDialog.Buttons[1].Action();
                }

                break;
            case 3:
                var option = EditorUtility.DisplayDialogComplex(
                    mobileSystemDialog.TitleText,
                    mobileSystemDialog.BodyText,
                    mobileSystemDialog.Buttons[0].ButtonLabel,
                    mobileSystemDialog.Buttons[1].ButtonLabel,
                    mobileSystemDialog.Buttons[2].ButtonLabel);
                switch (option)
                {
                    case 0:
                    case 1:
                    case 2:
                        mobileSystemDialog.Buttons[option].Action();
                        break;
                    default:
                        _debugService.Log("Dialog unrecognized option.");
                        break;
                }

                break;
        }
    }
}
#endif
