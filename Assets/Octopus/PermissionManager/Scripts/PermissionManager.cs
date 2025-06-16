using Core;
using UnityEngine;
using UnityEngine.Android;

namespace Permissions
{
    public static class PermissionManager
    {
        public static void AskPermission(string permission)
        {
            if (Permission.HasUserAuthorizedPermission(permission))
            {
                PrintMessage($"Permission {permission} granted!!");
            }
            else
            {
                var callbacks = new PermissionCallbacks();
                
                callbacks.PermissionDenied += PermissionCallbacks_PermissionDenied;
                callbacks.PermissionGranted += PermissionCallbacks_PermissionGranted;
                callbacks.PermissionDeniedAndDontAskAgain += PermissionCallbacks_PermissionDeniedAndDontAskAgain;
                
                Permission.RequestUserPermission(permission, callbacks);
            }
        }

        private static void PermissionCallbacks_PermissionDeniedAndDontAskAgain(string permissionName)
        {
            PrintMessage($"{permissionName} PermissionDeniedAndDontAskAgain");
        }

        private static void PermissionCallbacks_PermissionGranted(string permissionName)
        {
            PrintMessage($"{permissionName} PermissionCallbacks_PermissionGranted");
        }

        private static void PermissionCallbacks_PermissionDenied(string permissionName)
        {
            PrintMessage($"{permissionName} PermissionCallbacks_PermissionDenied");
        }

        private static void PrintMessage(string message)
        {
            Debugger.Log($"@@@ Client ->: {message}", new Color(0.2f, 0.4f, 0.9f));
        }
    }
}

