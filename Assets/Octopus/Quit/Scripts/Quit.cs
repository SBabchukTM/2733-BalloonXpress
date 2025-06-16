using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Octopus.Quit
{
    public class Quit : MonoBehaviour
    {
        [SerializeField] private UnityEvent OnClickButtonBack;

        private bool clickedBefore = false;

        private const float timerTime = 0.5f;

        private void Update()
        {
            if (!Input.GetKeyDown(KeyCode.Escape) || clickedBefore) return;
            
            clickedBefore = true;

            OnClickButtonBack?.Invoke();

            StartCoroutine(QuitingTimer());
        }

        private IEnumerator QuitingTimer()
        {
            yield return null;

            float counter = 0;

            while (counter < timerTime)
            {
                counter += Time.deltaTime;

                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    Exit();
                }

                yield return null;
            }

            clickedBefore = false;
        }

        private void Exit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            UnityEngine.Application.Quit();
#endif
        }
    }
}