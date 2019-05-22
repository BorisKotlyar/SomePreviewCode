namespace UserData
{
    public class UserSaveData : ISaveble, IMinigameUserData
    {
        public void Save()
        {
            SaveMinigameStates();
        }

        public void Load()
        {
            LoadMinigameStates();
        }

        public void LoadMinigameStates()
        {
            // Load minigames
        }

        public void SaveMinigameStates()
        {
            // Save minigames
        }
    }
}