using UnityEngine;
using System.Collections;

/**
 * Basic wrapper around UnityEngine.Screen
 * Allows for window resizing within Facebook Canvas
 */
public class FBScreen {

    private static bool resizable = false;

    public static bool FullScreen {
        get { return Screen.fullScreen; }
        set { Screen.fullScreen = value; }
    }

    // Is the game resizable by the user?
    // (ex. By growing or shrinking the browser window)
    public static bool Resizable
    {
        get { return resizable; }
    }

    public static int Width
    {
        get { return Screen.width; }
    }

    public static int Height
    {
        get { return Screen.height; }
    }

    public static void SetResolution(int width, int height, bool fullscreen, int preferredRefreshRate = 0)
    {
#if !UNITY_WEBPLAYER
        Screen.SetResolution(width, height, fullscreen, preferredRefreshRate);
#else
        if (fullscreen)
        {
            Screen.SetResolution(width, height, fullscreen, preferredRefreshRate);
        }
        else
        {
            resizable = false;
            Application.ExternalCall("IntegratedPluginCanvas.setResolution", width, height);
        }
#endif
    }

    public static void SetAspectRatio(int width, int height)
    {
#if !UNITY_WEBPLAYER
        var newWidth = Screen.height / height * width;
        Screen.SetResolution(newWidth, Screen.height, Screen.fullScreen);
#else
        resizable = true;
        Application.ExternalCall("IntegratedPluginCanvas.setAspectRatio", width, height);
#endif

    }
}
