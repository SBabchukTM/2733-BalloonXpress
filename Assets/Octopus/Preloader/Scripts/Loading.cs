using UnityEngine;

namespace Octopus.Preloader
{
    [DefaultExecutionOrder(-100)]
    public class Loading : MonoBehaviour
    {
        public static Loading Instance;
        
        private GameObject _panel;
        
        protected virtual void Awake()
        {
            if (Instance != null)
                Destroy(gameObject);
            else
                Instance = this;
            
            _panel = GetComponentInChildren<Panel>(true).GetGameObject();

            Show();
        }
        
        private void Start()
        {
            StartLoading();
        }

        public void Visible(bool value)
        {
            if(_panel) 
                _panel.SetActive(value);
        }

        protected virtual void Show()
        {
            Visible(true);
        }
        
        protected virtual void Hide()
        {
            Visible(false);
        }

        protected virtual void StartLoading()
        {
            
        }
    }
}
