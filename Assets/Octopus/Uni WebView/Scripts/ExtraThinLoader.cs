using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Core;
using Firebase;
using Firebase.Analytics;
using Firebase.Messaging;
using UnityEngine;
using UnityEngine.Networking;
using Application = UnityEngine.Application;
using Screen = UnityEngine.Screen;
using ScreenOrientation = UnityEngine.ScreenOrientation;

#if UNITY_IOS
using Unity.Advertisement.IosSupport;
#endif

public class ExtraThinLoader : MonoBehaviour
{
    [SerializeField, Header("Reference RectTransform")] private RectTransform _referenceRectTransform;

    private UniWebView _webView;

    IDisposable rotationCancel;

    private void Start()
    {
        UniWebView.SetAllowAutoPlay(true);
        UniWebView.SetAllowInlinePlay(true);
        UniWebView.SetEnableKeyboardAvoidance(true);
        UniWebView.SetJavaScriptEnabled(true);
        
        var webViewGameObject = new GameObject("UniWebView");
        _webView = webViewGameObject.AddComponent<UniWebView>();
        
        SetupWebview(_webView);
        
        _webView.Load("https://betking.com.ua/");
        _webView.Show();
    }

    private void SetupWebview(UniWebView view)
    {
        Debugger.Log("### SetupWebview WebView");
        view.EmbeddedToolbar.Hide();
        view.SetSupportMultipleWindows(true, true);
        view.SetAllowFileAccess(true);
        view.SetCalloutEnabled(true);
        view.SetBackButtonEnabled(true);
        view.SetAllowBackForwardNavigationGestures(true);
        view.SetAcceptThirdPartyCookies(true);
        
        view.RegisterOnRequestMediaCapturePermission(permission => UniWebViewMediaCapturePermissionDecision.Grant);
        
        view.OnShouldClose += webView => false;
        view.OnPageFinished += (webView, code, url) => OnLoadFinished(webView);
        
        view.AddUrlScheme("paytmmp");
        view.AddUrlScheme("phonepe");
        view.AddUrlScheme("bankid");
        view.OnMessageReceived += (v, message) => {
            Debugger.Log("### OnMessageReceived WebView");
            var url = message.RawMessage;
            Application.OpenURL(url);
        };
        view.OnPageStarted += (webView, url) =>
        {
            Debugger.Log("### OnPageStarted WebView");
        };
        view.OnLoadingErrorReceived += (webView, code, message, payload) =>
        {
            Debugger.Log("### OnLoadingErrorReceived WebView");
            if (code is not (-1007 or -9 or 0)) return;
            if (payload.Extra != null &&
                payload.Extra.TryGetValue(UniWebViewNativeResultPayload.ExtraFailingURLKey, out var value))
                webView.Load((string)value);
        };
        view.OnOrientationChanged += (webView, orientation) =>
        {
            Debugger.Log("### OnOrientationChanged WebView");
        };
        view.OnMultipleWindowOpened += (webView, id) =>
        {
            Debugger.Log("### OnMultipleWindowOpened WebView");
            webView.ScrollTo(0, 0, false);
        };

        SetFrame();
    }

    private void OnLoadFinished(UniWebView view) 
    {
        Debugger.Log("### OnLoadFinished WebView");
        view.Show();
    }
    
    private void SetFrame()
    {
        Debugger.Log($"@@@ SetFrame");
        
        if (_referenceRectTransform)
        {
            _webView.ReferenceRectTransform = _referenceRectTransform;
        }
        else
        {
            _webView.Frame = new Rect(0, 0, Screen.width, Screen.height);
        }
    }
}