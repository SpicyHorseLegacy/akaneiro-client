using UnityEngine;
using System.Collections;

public class UrlOpener {
	static public void Open(string url) {
		if (Steamworks.activeInstance != null) {
			Steamworks.SteamInterface.Friends.ActivateGameOverlayToWebPage(url);
		} else {
			Application.OpenURL(url);
		}
	}
}
