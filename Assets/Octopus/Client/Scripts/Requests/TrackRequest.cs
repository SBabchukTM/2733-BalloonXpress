using System;
using UnityEngine;
using UnityEngine.Networking;

namespace Octopus.Client
{
    [System.Serializable]
    public class TrackRequest : Request
    {
        public override void GenerateBody()
        {
            base.GenerateBody();
            
            body += ",\"" + Constants.ReqType + "\":\"" + ReqType.track.ToString() + "\"";
            body += ",\"" + "trackID" + "\":\"" + PlayerPrefs.GetString(Constants.PushCampaignUID, "null") + "\"";
        }
        
        public override void GenerateURL()
        {
            url = $"{Settings.GetPostbackApiUrl()}?" +
                  $"{Settings.GetPostbackTrackingIdKey()}={GameSettings.GetValue("trackingId")}" +
                  $"&{Settings.GetPostbackFcmTokenKey()}={GameSettings.GetValue(Constants.FcmTokenKey)}" +
                  $"";
        }
        
        public override void Respone(UnityWebRequest response, Action finished)
        {
            
        }
    }
}
