using Permissions;
using UnityEngine;

public class NotificationPermission : MonoBehaviour
{
    private string _permission = "android.permission.POST_NOTIFICATIONS";
    
    private void Start()
    {
        PermissionManager.AskPermission(_permission);
    }
}
