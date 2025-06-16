#if UNITY_ANDROID && !UNITY_EDITOR
using UnityEngine;
using UnityEngine.Android;

public class KeyboardVisibilityDetector : AndroidJavaProxy
{
    private readonly AndroidJavaObject _rootView;
    private int _lastVisibleHeight = 0;
    private const float KeyboardThresholdRatio = 0.15f;

    public delegate void KeyboardChanged(bool visible);
    public event KeyboardChanged OnKeyboardChanged;

    public KeyboardVisibilityDetector(AndroidJavaObject activity) 
        : base("android.view.ViewTreeObserver$OnGlobalLayoutListener")
    {
        _rootView = activity.Call<AndroidJavaObject>("getWindow").Call<AndroidJavaObject>("getDecorView");
        _rootView.Call<AndroidJavaObject>("getViewTreeObserver").Call("addOnGlobalLayoutListener", this);
    }

    public void onGlobalLayout()
    {
        using (AndroidJavaObject rect = new AndroidJavaObject("android.graphics.Rect"))
        {
            _rootView.Call("getWindowVisibleDisplayFrame", rect);
            int visibleHeight = rect.Call<int>("height");
            int rootViewHeight = _rootView.Call<int>("getHeight");

            if (_lastVisibleHeight == 0)
            {
                _lastVisibleHeight = visibleHeight;
                return;
            }

            int diff = rootViewHeight - visibleHeight;
            bool isKeyboardVisible = diff > rootViewHeight * KeyboardThresholdRatio;

            bool wasVisible = _lastVisibleHeight < rootViewHeight * (1 - KeyboardThresholdRatio);

            if (isKeyboardVisible != wasVisible)
            {
                OnKeyboardChanged?.Invoke(isKeyboardVisible);
            }

            _lastVisibleHeight = visibleHeight;
        }
    }

    public void Cleanup()
    {
        _rootView.Call<AndroidJavaObject>("getViewTreeObserver").Call("removeOnGlobalLayoutListener", this);
    }
}
#endif