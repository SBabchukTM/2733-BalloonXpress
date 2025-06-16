using UnityEngine;

public class WebviewShowPage : MonoBehaviour
{
    public static WebviewShowPage Instance;

    [SerializeField, Header("Embedded Toolbar")] private bool _embeddedToolbar;
    
    [SerializeField, Header("Reference RectTransform")] private RectTransform _referenceRectTransform;
    
    [SerializeField, Header("Webview Background")] private GameObject _bgWebView;
    
    [SerializeField, Header("No Internet")] private GameObject _noInternet;
     
    private UniWebView _webView;
    
    private void Awake()
    {
        Instance = this;
    }
    
    public void LoadUrl(string urlBinom = "http://google.com")
    {
        if (_webView != null) return;
        
        CreateWebView();

        _webView.Load(urlBinom);

        Subscribe();
    }
    
    private void Subscribe()
    {
        _webView.OnPageErrorReceived += OnPageErrorReceived;

        _webView.OnPageFinished += OnPageFinished;
        
        _webView.OnShouldClose += OnPageClosed;
    }

    private void UnSubscribe()
    {
        _webView.OnPageErrorReceived -= OnPageErrorReceived;

        _webView.OnPageFinished -= OnPageFinished;
        
        _webView.OnShouldClose -= OnPageClosed;
    }

    private void CreateWebView()
    {
        var webViewGameObject = new GameObject("UniWebView");
        
        _webView = webViewGameObject.AddComponent<UniWebView>();

        SetFrame();
    }
    
    private void SetFrame()
    {
        if (_referenceRectTransform)
        {
            _webView.ReferenceRectTransform = _referenceRectTransform;
        }
        else
        {
            _webView.Frame = new Rect(0, 0, Screen.width, Screen.height);
        }
    }
    
    private void OnPageErrorReceived(UniWebView view, int statusCode, string url)
    {
        Remove();
        
        _noInternet.SetActive(true);

        UnSubscribe();
    }
    
    private bool OnPageClosed(UniWebView webview)
    {
        if(_bgWebView) _bgWebView.SetActive(false);

        return _bgWebView;
    }
    
    private void OnPageFinished(UniWebView view, int statusCode, string url)
    {
        Show();
    }
  
    private void Remove()
    {
        if(_bgWebView) _bgWebView.SetActive(false);
        
        Destroy(_webView);
        
        _webView = null;
    }

    public void Hide()
    {
        Remove();
    }
    
    private void Show()
    {
        if(_webView == null) return;
        
        if(_bgWebView) _bgWebView.SetActive(true);
        
        _webView.Show();

        CheckToolbar();
    }

    private void CheckToolbar()
    {
        if(_embeddedToolbar)
        {
            _webView.EmbeddedToolbar.Show();
        }
    }
}
