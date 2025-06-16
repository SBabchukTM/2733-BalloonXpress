using Runtime.Services;
using Core;
using Core.Compressor;

namespace Runtime.Services.UserData
{
    public class UserDataService
    {
        private readonly IPersistentDataProvider _persistentDataProvider;
        private readonly BaseCompressor _compressor;

        private UserData _userData;

        public UserDataService(IPersistentDataProvider persistentDataProvider, BaseCompressor compressor)
        {
            _persistentDataProvider = persistentDataProvider;
            _compressor = compressor;
        }

        public void Initialize()
        {
#if DEV
            _userData = _persistentDataProvider.Load<UserData>(ConstDataPath.UserDataPath, ConstDataPath.UserDataFileName) ?? new UserData();
#else
            _userData = _persistentDataProvider.Load<UserData>(ConstDataPath.UserDataPath, ConstDataPath.UserDataFileName,null, _compressor) ?? new UserData();
#endif
        }

        public UserData GetUserData()
        {
            return _userData;
        }
        
        public void SaveUserData()
        {
            if(_userData == null)
                return;

#if DEV
            _persistentDataProvider.Save(_userData, ConstDataPath.UserDataPath, ConstDataPath.UserDataFileName);
#else
            _persistentDataProvider.Save(_userData, ConstDataPath.UserDataPath, ConstDataPath.UserDataFileName, null, _compressor);
#endif
        }
    }
}