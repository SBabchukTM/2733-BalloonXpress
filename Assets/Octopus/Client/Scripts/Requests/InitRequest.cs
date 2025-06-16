using System.Collections.Generic;
using UnityEngine;

namespace Octopus.Client
{
    [System.Serializable]
    public class InitRequest : Request
    {
        public override void GenerateBody()
        {
            base.GenerateBody();
            
            body += ",\"" + Constants.ReqType + "\":\"" + ReqType.create.ToString() + "\"";
        }

        public override void GenerateURL()
        {
            var referrer = "cmpgn=trident-dev-test_TEST-Deeplink_test1_%D1%82%D0%B5%D1%81%D1%822_TEST3_%D0%A2%D0%95%D0%A1%D0%A24_s%20p%20a%20c%20e";
            
            url = $"{Settings.GetDomain()}?" +
                  $"{Settings.GetGadIdKey()}={GameSettings.GetValue(Constants.GAID)}" +
                  $"&{Settings.GetExtraParam2()}={(USBInstallationChecker.IsDeveloperModeEnabled()? 1 : 0)}" +
                  $"&{Settings.GetReferrerKey()}={referrer}";
        }
    }
}
