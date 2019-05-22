using UnityEditor;

namespace Localization.Editor
{
	public class LocalizationEditorManager : EditorWindow
	{
		[MenuItem("DevGame/Localization/Refresh")]
		private static void RefreshLocalization()
		{
			LocalizationImporter.Import();
		}
		
		[MenuItem("DevGame/Localization/Settings")]
		private static void OpenLocalizationSettingsWindow()
		{
			var window = (LocalizationSettingsWindow) GetWindow(typeof(LocalizationSettingsWindow));
			window.titleContent.text = "Localization settings";
		}
		
		[MenuItem("DevGame/Localization/Values")]
		private static void OpenLocalizationValuesWindow()
		{
			var window = (LocalizationTableWindow) GetWindow(typeof(LocalizationTableWindow));
			window.titleContent.text = "Localization values";
		}
	}
}
