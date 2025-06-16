using System;
using System.Collections.Generic;

namespace Runtime.Services.UserData
{
    [Serializable]
    public class UserData
    {
        public List<GameSessionData> GameSessionData = new List<GameSessionData>();
        public SettingsData SettingsData = new SettingsData();
        public GameData GameData = new GameData();
        public UserInventory UserInventory = new UserInventory();
        public UserProfileData UserProfileData = new UserProfileData();
    }
}