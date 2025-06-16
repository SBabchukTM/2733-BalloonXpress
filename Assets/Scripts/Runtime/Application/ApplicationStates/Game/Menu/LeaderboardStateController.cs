using System.Threading;
using Core.StateMachine;
using Runtime.UI;
using Cysharp.Threading.Tasks;
using ILogger = Core.ILogger;
using Runtime.Services.UserData;
using System.Collections.Generic;
using System.Linq;
using Core;
using Core.Factory;
using Zenject;
using UnityEngine;
using Runtime.Services;

namespace Runtime.Game
{
    public class LeaderboardStateController : StateController, IInitializable
    {
        private readonly IUiService _uiService;
        private readonly IAssetProvider _assetProvider;
        private readonly GameObjectFactory _factory;
        private readonly UserDataService _userDataService;
        private readonly StartSettingsController _startSettingsController;

        private GameObject _leaderboardRecordDisplayPrefab; 

        private LeaderboardScreen _screen;

        public LeaderboardStateController(ILogger logger, IUiService uiService, IAssetProvider assetProvider, GameObjectFactory factory, UserDataService userDataService, StartSettingsController startSettingsController) : base(logger)
        {
            _uiService = uiService;
            _assetProvider = assetProvider;
            _userDataService = userDataService;
            _factory = factory;
            _startSettingsController = startSettingsController;
        }

        public async void Initialize()
        {
            _leaderboardRecordDisplayPrefab = await _assetProvider.Load<GameObject>(ConstPrefabs.LeaderboardRecordPrefab);
        }

        public override UniTask Enter(CancellationToken cancellationToken)
        {
            CreateScreen();
            SubscribeToEvents();

            return UniTask.CompletedTask;
        }

        public override async UniTask Exit()
        {
            await _uiService.HideScreen(ConstScreens.LeaderboardScreen);
        }

        private void CreateScreen()
        {
            _screen = _uiService.GetScreen<LeaderboardScreen>(ConstScreens.LeaderboardScreen);
            _screen.Initialize(CreateRecordsList());
            _screen.ShowAsync().Forget();
        }

        private void SubscribeToEvents()
        {
            _screen.OnBackPressed += async () => await GoTo<MenuStateController>();
        }

        private List<LeaderboardRecordDisplay> CreateRecordsList()
        {
            var recordsDataList = CreateRecordsDataList();
            List<LeaderboardRecordDisplay> result = new(recordsDataList.Count);

            for(int i = 0; i < recordsDataList.Count; i++)
            {
                LeaderboardRecordDisplay display = _factory.Create<LeaderboardRecordDisplay>(_leaderboardRecordDisplayPrefab);
                display.Initialize(recordsDataList[i]);
                result.Add(display);
            }

            return result;
        }

        private List<LeaderboardRecord> CreateRecordsDataList()
        {
            var recordsData = CreateFakeRecords();
            recordsData.Add(new LeaderboardRecord() { Name = _userDataService.GetUserData().UserProfileData.Username, Balance = _userDataService.GetUserData().UserInventory.Balance });

            recordsData = recordsData.OrderByDescending(x => x.Balance).ToList();

            for (int i = 0; i < recordsData.Count; i++)
                recordsData[i].Place = i + 1;

            return recordsData;
        }

        private List<LeaderboardRecord> CreateFakeRecords() => new()
        {
            new LeaderboardRecord{Name = "Vanessa", Balance = 62230 },
            new LeaderboardRecord{Name = "Mike", Balance = 52420 },
            new LeaderboardRecord{Name = "Steve", Balance = 51235 },
            new LeaderboardRecord{Name = "Fred", Balance = 43214 },
            new LeaderboardRecord{Name = "Susie", Balance = 33521 },
            new LeaderboardRecord{Name = "John", Balance = 31231 },
            new LeaderboardRecord{Name = "Margaret", Balance = 22352 },
            new LeaderboardRecord{Name = "Andrew", Balance = 11231 },
            new LeaderboardRecord{Name = "Lisa", Balance = 10001 },
            new LeaderboardRecord{Name = "Joshua", Balance = 9921 },
            new LeaderboardRecord{Name = "Peter", Balance = 6312 },
            new LeaderboardRecord{Name = "Stewie", Balance = 5555 },
            new LeaderboardRecord{Name = "Lois", Balance = 4002 },
            new LeaderboardRecord{Name = "Mary", Balance = 3321 },
            new LeaderboardRecord{Name = "Iren", Balance = 2001 },
            new LeaderboardRecord{Name = "Natalie", Balance = 1430 },
            new LeaderboardRecord{Name = "Brian", Balance = 777 },
            new LeaderboardRecord{Name = "Meg", Balance = 10 },
        };
    }

    public class LeaderboardRecord
    {
        public string Name;
        public int Place;
        public int Balance;
    }
}