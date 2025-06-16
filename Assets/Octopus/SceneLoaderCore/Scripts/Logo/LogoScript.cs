using UnityEngine;
using DG.Tweening;
using Octopus.SceneLoaderCore.Helpers;

namespace Octopus
{
    public class LogoScript : MonoBehaviour
    {
        
        private float time = 1;

        private void Start()
        {
            DOVirtual.DelayedCall(time, () => SceneLoader.Instance.SwitchToScene(SceneLoader.Instance.mainScene));
        }
    }
}
