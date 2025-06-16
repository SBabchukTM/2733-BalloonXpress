using System;
using System.Collections;
using Core;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

namespace Core
{
    [DefaultExecutionOrder(-50)]
    public class InternetConnectionMonitor : MonoBehaviour
    {
        [SerializeField] private bool StartOnAwake = true;
        
        [Header("Ping parameters")]
        [SerializeField] private string url = "https://google.com";

        [SerializeField] private float pingInterval = 5f;

        [SerializeField, Space(10)] private UnityEvent InternetAvailable;

        [SerializeField] private UnityEvent InternetNotAvailable;
        
        public static InternetConnectionMonitor Instance;

        private UnityWebRequest _request;
        private bool? isConnected;
        private Coroutine testCoroutine;
        
        private bool? IsConnected
        {
            get => isConnected;
            set
            {
                PrintDebugMessage($"IsConnected SET: {isConnected} -> {value}", MessageType.Verbose);
                
                if (isConnected == value) return;

                isConnected = value;
                
                CheckConnect(value);

                PrintDebugMessage($"Is Connected:: {isConnected}", MessageType.Verbose);
            }
        }
        
        private void CheckConnect(bool? isConnected)
        {
            switch (isConnected)
            {
                case true:
                    PrintDebugMessage("Internet Available", MessageType.Verbose);
                
                    InternetAvailable?.Invoke();
                    break;
                case false:
                    PrintDebugMessage("Internet Not Available", MessageType.Verbose);
                
                    PrintDebugMessage(" ----- Hide WebView in InternetConnectionMonitor", MessageType.Verbose);
                
                    InternetNotAvailable?.Invoke();
                    break;
            }
        }

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
                
                DontDestroyOnLoad(Instance.gameObject);

                if (StartOnAwake)
                {
                    StartCheckConnect();
                }
            }
        }

        private void StartCheckConnect()
        {
            if (testCoroutine == null)
            {
                testCoroutine = StartCoroutine(Testing());
            }
            else
            {
                PrintDebugMessage("Connection check already started!", MessageType.Warning);
            }
        }

        public void StopConnectionCheck()
        {
            if (testCoroutine != null)
            {
                StopCoroutine(testCoroutine);
                
                testCoroutine = null;
            }
            else
            {
                PrintDebugMessage("No active Connection check!", MessageType.Warning);
            }
        }

        public void CheckErrorReceived()
        {
            if (IsConnected == true)
            {
                IsConnected = false;
            }
        }
        
        private IEnumerator Testing()
        {
            PrintDebugMessage("Testing", MessageType.Verbose);
            
            while (true)
            {
                PrintDebugMessage($"Testing... {UnityEngine.Application.internetReachability}", MessageType.Verbose);
                
                if (UnityEngine.Application.internetReachability == NetworkReachability.NotReachable)
                {
                    IsConnected = false;
                }
                else
                {
                    if (!string.IsNullOrEmpty(url))
                    {
                        _request = new UnityWebRequest(url);

                        yield return _request.SendWebRequest();

                        if (_request.result == UnityWebRequest.Result.Success)
                        {
                            IsConnected = true;
                        }
                        else if (_request.result != UnityWebRequest.Result.InProgress)
                        {
                            IsConnected = false;

                            PrintDebugMessage($"Connection Error::{_request.error}", MessageType.Warning);
                        }

                    }
                    else
                    {
                        IsConnected = false;
                        
                        PrintDebugMessage($"Ping URL in Connectivity Manager ( Inspector ) is missing", MessageType.Error);
                    }
                }

                yield return new WaitForSeconds(pingInterval);
            }
        }

        private void PrintDebugMessage(string message, MessageType messageType)
        {
            if (!Settings.IsDebug())
            {
                return;
            }
            
            switch (messageType)
            {
                case MessageType.Warning:
                    Debug.LogWarning($"InternetConnectionMonitor:: {message}");
                    break;
                case MessageType.Error:
                    Debug.LogError($"InternetConnectionMonitor:: {message}");
                    break;
                default:
                    Debug.Log($"InternetConnectionMonitor:: {message}");
                    break;
            }
        }

        private enum MessageType
        {
            Verbose,
            Warning,
            Error
        }
    }

    [Serializable] public class UnityConnectivityEvent : UnityEvent<bool, string> { }
}
