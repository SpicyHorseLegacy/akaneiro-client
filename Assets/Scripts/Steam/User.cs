using UnityEngine;
using System.Collections;
using ManagedSteam;
using ManagedSteam.SteamTypes;

public class User : MonoBehaviour
{
    public static string Path { get; private set; }

    private IUser user;

	// Use this for initialization
    void Start()
    {
        if (Steamworks.SteamInterface == null)
        {
            UnityEngine.Debug.LogError("SteamInterface startup failed!");
            return;
        }

        user = Steamworks.SteamInterface.User;

        Path = user.GetUserDataFolder().Path;
    }
	
	// Update is called once per frame
	void Update ()
    {
	}
}
