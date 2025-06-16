using System.Collections.Generic;
using Extension;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Octopus.SceneLoaderCore.Helpers
{
	public class SceneLoader : MonoBehaviour
    {
	    public static SceneLoader Instance;

		[Header("Main scene"), Scene, Tooltip("First scene for game")]
		public string mainScene;

		[Header("Preloader scene"), Scene, Tooltip("Loading scene")]
		public string preloaderScene;
		
		[Header("Webview scene"), Scene, Tooltip("Webview scene for offers")]
		public string webviewScene;
		
		private CanvasGroup _fadeGroup;     

		private string _targetScene;
		
		private void Awake()
		{
			if (Instance) return;
			
			Instance = this;

			DontDestroyOnLoad(gameObject);
		}
		
        private void Start()
        {
            SwitchToScene(preloaderScene);
        }
        
        public void SwitchToScene(string targetScene)
		{
			Debug.Log($"🔄 Switching to scene: {targetScene}");
			
			SetTargetScene(targetScene);

			UIFader.Instance.FadeIn();
		}
        
        public string GetCurrentScene()
        {
	        return SceneManager.GetActiveScene().name;
        }

        internal void LoadScene()
        {
	        UIFader.Instance.FadeOut();

	        SceneManager.LoadScene(GetTargetScene());
        }

		private void SetTargetScene(string scene)
		{
			_targetScene = scene;
		}

		private string GetTargetScene()
		{
			return _targetScene;
		}
    }
}
