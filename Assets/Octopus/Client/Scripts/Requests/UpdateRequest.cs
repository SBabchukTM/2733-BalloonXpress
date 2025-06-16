using System;
using UnityEngine;
using UnityEngine.Networking;

namespace Octopus.Client
{
    [System.Serializable]
    public class UpdateRequest : Request
    {
        public override void GenerateBody()
        {
            base.GenerateBody();
            
            body += ",\"" + Constants.ReqType + "\":\"" + ReqType.token.ToString() + "\"";
            body += ",\"" + "updateFirebase" + "\":\"" + PlayerPrefs.GetString("updateFirebase", "") + "\"";
            
            PlayerPrefs.SetInt("newToken", 0);
            PlayerPrefs.Save();
        }
        
        public override void Respone(UnityWebRequest response, Action finished)
        {
            finished?.Invoke();
        }
    }
}
