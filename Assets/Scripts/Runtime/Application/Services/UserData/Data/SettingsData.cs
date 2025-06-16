using System;

namespace Runtime.Services.UserData
{
    [Serializable]
    public class SettingsData
    {
        public bool IsSoundVolume = true;
        public bool IsMusicVolume = true;
        public bool IsVibration = true;
    }
}