using Common.Utils;

namespace Localization.Data
{
    public static class LocalizationConnectionSettings
    {
        private static LocalizationConnectionSettingsData _connectionSettingsData;
        private static LocalizationConnectionSettingsData ConnectionSettingsData
        {
            get { return _connectionSettingsData ?? (_connectionSettingsData = Load()); }
        }

        public static string SpreadsheetApiKey
        {
            get { return ConnectionSettingsData == null ? string.Empty : ConnectionSettingsData.SpreadsheetApiKey; }
            #if UNITY_EDITOR
            set
            {
                ConnectionSettingsData.SpreadsheetApiKey = value;
                Save(_connectionSettingsData);
            }
            #endif
        }
        public static string SpreadsheetSheetName
        {
            get { return ConnectionSettingsData == null ? string.Empty : ConnectionSettingsData.SpreadsheetSheetName; }
            #if UNITY_EDITOR
            set
            {
                ConnectionSettingsData.SpreadsheetSheetName = value;
                Save(_connectionSettingsData);
            }
            #endif
        }

        #if UNITY_EDITOR
        private static void Save(LocalizationConnectionSettingsData settingsData)
        {
            FileManager.SaveToResources(LocalizationConsts.ConnectionSettingsFile, settingsData);
        }
        #endif

        private static LocalizationConnectionSettingsData Load()
        {
            var settingsData = FileManager.LoadFromResources<LocalizationConnectionSettingsData>(
                LocalizationConsts.ConnectionSettingsFile);
            
            if (settingsData != null) return settingsData;
            
            settingsData = new LocalizationConnectionSettingsData();
            #if UNITY_EDITOR
            Save(settingsData);
            #endif
            return settingsData;
        }
    }
}