using System;
using System.Collections;
using Core;
using Octopus.Client;
using UnityEngine;
using UnityEngine.Networking;

public static class SenderRequest
{
    public static IEnumerator Send(Request webRequest, Action callback)
    {
        webRequest.GenerateBody();
        
        webRequest.GenerateURL();
           
        PrintMessage($"SenderRequest->Send: webRequest.Json(){webRequest.Json()}");
        PrintMessage($"SenderRequest->Send: webRequest.url={webRequest.url}");
        
        var unityWebRequest = new UnityWebRequest(webRequest.url, UnityWebRequest.kHttpVerbPOST);
        
        unityWebRequest.downloadHandler = new DownloadHandlerBuffer();
        unityWebRequest.SetRequestHeader("User-Agent", "runscope/0.1");
        
        var bytes = new System.Text.UTF8Encoding().GetBytes(XBase64.Encode(webRequest.Json()));

        unityWebRequest.uploadHandler = new UploadHandlerRaw(bytes);
        
        PrintFullRequest(unityWebRequest, webRequest.Json());

        yield return unityWebRequest.SendWebRequest();
        
        webRequest.Respone(unityWebRequest, callback);
        
        unityWebRequest.Dispose();
    }
    
    private static void PrintFullRequest(UnityWebRequest request, string jsonBody)
    {
        Debugger.Log($"@@@ [Request] URL: {request.url}", Color.cyan);
        Debugger.Log($"@@@ [Request] Method: {request.method}", Color.cyan);
        Debugger.Log($"@@@ [Request] Body: {jsonBody}", Color.green);
    }

    
    private static void PrintMessage(string message)
    {
        Debugger.Log($"@@@ SenderRequest ->: {message}", new Color(0.2f, 0.8f, 0.5f));
    }
}
