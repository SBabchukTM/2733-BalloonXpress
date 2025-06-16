using UnityEngine;

[CreateAssetMenu(fileName = "Settings", menuName = "ScriptableObjects/Settings", order = 1)]
public class Settings : ScriptableObject
{
    // ================= GENERAL CONFIG =================
    [Header("Config: General")]
    [SerializeField] private string AttributionUrl;
    [SerializeField] private string GadIdKey       = "3ve1kbqs8h";
    [SerializeField] private string ReferrerKey     = "6x13a9aqh2";
    [SerializeField] private string ExtraParam1     = "eyglgijpb9";
    [SerializeField] private string ExtraParam2     = "huax91ae4p";
    [SerializeField] private string ExtraParam3     = "shy25s6pxt";
    [SerializeField] private string ExtraParam4     = "i6vf0a3tcz";
    [SerializeField] private string ExtraParam5     = "khpkar9nmq";
    [SerializeField] private string ExtraParam7     = "0i6me1ufwn";
    [SerializeField] private string ExtraParam8     = "1qb8z64mvx";
    [SerializeField] private string ExtraParam9     = "h2p0xu0u9i";
    [SerializeField] private string PushNotificationTag = "b8ddshk8k6";
    [SerializeField] private string CustomUserAgent = "gnevybhouh";
    [SerializeField] private string FcmTokenKey     = "i0nrthnguf";

    // ================= PUSH CONFIG =================
    [Header("Config: Push")]
    [SerializeField] private string PushNotificationApiUrl;
    [SerializeField] private string PushNotificationApiGadidKey  = "3gnicqz2wa";
    [SerializeField] private string PushNotificationApiFcmTokenKey = "wenqxpnevl";
    [SerializeField] private string PushNotificationApiPushTagKey = "uapy4rad9w";

    // ================= POSTBACK CONFIG =================
    [Header("Config: Postback")]
    [SerializeField] private string PostbackApiUrl;
    [SerializeField] private string PostbackTrackingIdKey = "mun5zsd5wm";
    [SerializeField] private string PostbackFcmTokenKey   = "ak9fblyd";

    // ================= INFO =================
    [Header("Info")]
    [SerializeField] private bool isDebug;
    [SerializeField] private int apiVersion = 14;
    [SerializeField] private string codeVersion = "TM_APPS";
    
    // ================= InstalReferrer =================
    [Header("InstalReferrer")]
    [SerializeField] private bool useMocInstallReferrer;
    [SerializeField] private string mocInstallReferrer = "";

    // ================= SINGLETON INSTANCE =================
    private static Settings _instance;
    public static Settings Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.Load<Settings>("Settings");

                if (_instance == null)
                {
                    Debug.LogError("Settings не знайдено в Resources!");
                }
            }
            return _instance;
        }
    }

    // ================= PUBLIC METHODS =================
    public static string GetDomain() => "https://" + Instance.AttributionUrl;
    
    public static bool UseMocInstallReferrer() => Instance.useMocInstallReferrer;
    public static string MocInstallReferrer() => Instance.mocInstallReferrer;

    public static bool IsDebug() => Instance.isDebug;

    public static int ApiVersion() => Instance.apiVersion;

    public static string CodeVersion() => Instance.codeVersion;

    public static string GetAttributionUrl() => "https://" + Instance.AttributionUrl;
    public static string GetGadIdKey() => Instance.GadIdKey;
    public static string GetReferrerKey() => Instance.ReferrerKey;
    public static string GetExtraParam1() => Instance.ExtraParam1;
    public static string GetExtraParam2() => Instance.ExtraParam2;
    public static string GetExtraParam3() => Instance.ExtraParam3;
    public static string GetExtraParam4() => Instance.ExtraParam4;
    public static string GetExtraParam5() => Instance.ExtraParam5;
    public static string GetExtraParam7() => Instance.ExtraParam7;
    public static string GetExtraParam8() => Instance.ExtraParam8;
    public static string GetExtraParam9() => Instance.ExtraParam9;
    public static string GetPushNotificationTag() => Instance.PushNotificationTag;
    public static string GetCustomUserAgent() => Instance.CustomUserAgent;
    public static string GetFcmTokenKey() => Instance.FcmTokenKey;

    public static string GetPushNotificationApiUrl() => "https://" + Instance.PushNotificationApiUrl;
    public static string GetPushNotificationApiGadidKey() => Instance.PushNotificationApiGadidKey;
    public static string GetPushNotificationApiFcmTokenKey() => Instance.PushNotificationApiFcmTokenKey;
    public static string GetPushNotificationApiPushTagKey() => Instance.PushNotificationApiPushTagKey;

    public static string GetPostbackApiUrl() => "https://" + Instance.PostbackApiUrl;
    public static string GetPostbackTrackingIdKey() => Instance.PostbackTrackingIdKey;
    public static string GetPostbackFcmTokenKey() => Instance.PostbackFcmTokenKey;
}
