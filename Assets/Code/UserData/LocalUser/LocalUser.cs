namespace UserData
{
    public class LocalUser : User
    {
        public override ISaveble Load()
        {
            // just for example
            return new UserSaveData();
        }
    }
}
