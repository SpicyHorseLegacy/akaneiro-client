using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using ManagedSteam;
using ManagedSteam.Exceptions;
using ManagedSteam.CallbackStructures;
using ManagedSteam.SteamTypes;      //we use this for the GamepadTextInputMode enum

public class Utils : MonoBehaviour
{
    bool bGamepadTextInputDismissed = false;

	// Use this for initialization
    void Start()
    {
        if (Steamworks.SteamInterface == null)
        {
            UnityEngine.Debug.LogError("SteamInterface startup failed!");
            return;
        }

        IUtils utils = Steamworks.SteamInterface.Utils;

        utils.LowBatteryPower += LowBatteryPowerFunc;
        utils.SteamShutdown += SteamShutdownFunc;
        utils.GamepadTextInputDismissed += GamepadTextInputDismissedFunc;
    }

    void SteamShutdownFunc(SteamShutdown value)
    {
        //Steam wants your game to quit, the problem with doing this in Unity is that the gamewindow have to be active when calling Application.Quit
        Application.Quit();
    }

    void LowBatteryPowerFunc(LowBatteryPower value)
    {
        //SteamAPI: This event will be fired when running on a laptop and less than 10 minutes of battery is left, fires then every minute.
        //Use value.MinutesBatteryLeft or utils.GetCurrentBatteryPower to find out the battery amount left, just remember that they work different!
    }

    void GamepadTextInputDismissedFunc(GamepadTextInputDismissed value)
    {
        //This event is fired when the used exits the textinput overlay invoked by utils.ShowGamepadTextInput.
        //If value.Submitted is true it means that the user submitted a string that can be retrieved with: utils.GetEnteredGamepadTextInput
        //Two version of utils.GetEnteredGamepadTextInput is available since some of the languages unity supports can't handle the 'out' keyword.
        //The way our wrapper is made makes it possible to ignore utils.GetEnteredGamepadTextLength just to get the string, but you can still use it if you
        //just wants to know the length of the submitted text.
        bGamepadTextInputDismissed = true;
    }

    public string GetGamepadTextInput()
    {
        IUtils utils = Steamworks.SteamInterface.Utils;
        bGamepadTextInputDismissed = false;

        if (utils.ShowGamepadTextInput(GamepadTextInputMode.NormalMode, GamepadTextInputLineMode.SingleLine, "Example text input", 64))
        {
            while (!bGamepadTextInputDismissed) { }
        }

        return utils.GetEnteredGamepadTextInput().Text;
    }

    //Thanks to Karl @ Stunlock Studios for giving me their function as he implemented utils.GetImage(Size/RGBA) into our library
    public Texture2D GetTextureFromSteamID(SteamID steamId)
    {
        IFriends friends = Steamworks.SteamInterface.Friends;
        IUtils utils = Steamworks.SteamInterface.Utils;

        ImageHandle avatarHandle = friends.GetLargeFriendAvatar(steamId);
        if (avatarHandle.IsValid)
        {
            uint width, height;
            if (utils.GetImageSize(avatarHandle, out width, out height))
            {
                Texture2D texture = new Texture2D((int)width, (int)height, TextureFormat.RGBA32, true);
                Color32[] buffer = new Color32[width * height];

                GCHandle bufferHandle = GCHandle.Alloc(buffer, GCHandleType.Pinned);

                try
                {
                    System.IntPtr bufferPtr = Marshal.UnsafeAddrOfPinnedArrayElement(buffer, 0);

                    if (utils.GetImageRGBA(avatarHandle, bufferPtr, (int)width * (int)height * 4))
                    {
                        // Flip vertical
                        for (int x = 0; x < width; x++)
                        {
                            for (int y = 0; y < height / 2; y++)
                            {
                                Color32 temp = buffer[x + (width * y)];
                                buffer[x + (width * y)] = buffer[x + (width * (height - 1 - y))];
                                buffer[x + (width * (height - 1 - y))] = temp;
                            }
                        }

                        texture.SetPixels32(buffer);
                        texture.Apply();
                    }
                }
                finally
                {
                    bufferHandle.Free();
                }
                return texture;
            }
        }

        return null;
    }

	// Update is called once per frame
	void Update ()
    {
        //Make sure to run Steamworks.SteamInterface.Update (where we take care of things like SteamAPI_RunCallbacks) and you won't have to run the code below, 
        //this is also mentioned in the intellisense as 'redundant'
        /*
        IUtils utils = Steamworks.SteamInterface.Utils;
        utils.RunFrame();
        */
	}


}
