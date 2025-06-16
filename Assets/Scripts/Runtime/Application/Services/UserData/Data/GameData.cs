using System;

namespace Runtime.Services.UserData
{
    [Serializable]
    public class GameData
    {
        public int SessionNumber = 0;
        public bool IsAdb = false;
    }
}