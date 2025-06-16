namespace Runtime.Services.UserData
{
    public static class UserDataExtensions
    {
        public static bool IsFirstSession(this UserData userData)
        {
            return userData.GameData.SessionNumber <= 1;
        }
    }
}