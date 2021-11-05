using UnityEngine;

public static class HCDebug
{
    private const string prefixNormal = "<b>[HyperCat] </b>";
    private const string prefixWarning = "<b><color=yellow>[HyperCat] </color></b>";
    private const string prefixError = "<b><color=red>[HyperCat] </color></b>";
    private const string prefix = "<b>[HyperCat] </b>";

    public static void Log(object o)
    {
#if UNITY_EDITOR || PROTOTYPE
        Debug.Log(prefixNormal + o.ToString());
#endif
    }

    public static void LogWarning(object o)
    {
#if UNITY_EDITOR || PROTOTYPE
        Debug.LogWarning(prefixWarning + o.ToString());
#endif
    }

    public static void LogError(object o)
    {
#if UNITY_EDITOR || PROTOTYPE
        Debug.LogError(prefixError + o.ToString());
#endif
    }

    public static void Log(object o, HCColor color)
    {
#if UNITY_EDITOR || PROTOTYPE
        Debug.Log(string.Format("{0}<color={1}>{2}</color>", prefix, color.ToString(), o.ToString()));
#endif
    }
}

public enum HCColor
{
    white,
    black,
    red,
    green,
    blue,
    orange,
    violet,
    aqua,
    gray,
    magenta,
    purple,
    yellow,
}