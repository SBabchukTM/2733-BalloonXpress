using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Octopus.Nointernet
{
    public class NoInternet : MonoBehaviour
    {
        public static NoInternet Instance;
        
        private GameObject _panel;
        
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }

            _panel = GetComponentInChildren<Panel>(true).GetGameObject();
        }

        public void Show()
        {
            _panel.SetActive(true);
        }
        
        public void Hide()
        {
            _panel.SetActive(false);
        }
    }
}

