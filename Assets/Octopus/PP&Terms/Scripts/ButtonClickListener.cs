using UnityEngine;
using UnityEngine.UI;

namespace Core
{
    [RequireComponent(typeof(Button))]
    public class ButtonClickListener : MonoBehaviour
    {
        [SerializeField] private string url;
        
        private Button button;
        
        private void Awake()
        {
            button = GetComponent<Button>();

            button.onClick.AddListener(() =>
            {
                WebviewShowPage.Instance.LoadUrl(url);
            });
        }
    }
}
