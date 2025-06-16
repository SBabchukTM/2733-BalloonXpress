using UnityEngine;
using Octopus.SceneLoaderCore.Helpers;
using UnityEngine.UI;

namespace Octopus.Preloader
{
    public class HorizontalLoading : Loading
    {
        [SerializeField] private Slider progress;

        
        protected override void StartLoading()
        {
            progress.value = 0.0f;
        }

        private void Update()
        {
            if (progress.value < 0.9f) progress.value += 0.8f * Time.deltaTime;
        }
    }
}