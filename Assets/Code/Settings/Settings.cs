namespace GameSettings
{
    public class Settings
    {
        public const string MiniGames = "MiniGamesSettings";

        public float SaveTimePeriod = 10.0f;
        public MiniGamesSettings MiniGamesSettings { get; private set; }

        private Settings()
        {
            LoadMiniGamesSettings();
        }

        private void LoadMiniGamesSettings()
        {
            MiniGamesSettings = Common.Utils.FileManager.LoadConfigFile<MiniGamesSettings>(MiniGames);
        }
    }
}
