using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using System;

public class BuildClient
{
	[ExecuteInEditMode]
	[MenuItem("Build/Build Assets")]
	public static int BuildAssets()
	{
		try {
			Debug.Log("Building assets bundles.");
			
			BuildClient builder = new BuildClient();
			builder.createBundles(); //TODO: make this work. also, check this doesn't fail if the assetbundles directory is missing

			Debug.LogError("Build complete.");
			return 0;
		} catch (System.Exception ex) {
			Debug.LogException(ex);
			Debug.LogError("Build failed.");
			return 1;
		}
	}

	[ExecuteInEditMode]
	[MenuItem("Build/Build Web Client")]
	public static int BuildWebClient()
	{
		try {
			Debug.Log("Building web client.");
			
			BuildClient builder = new BuildClient();
			builder.setBuildFlagsWeb(""); // TODO: Fill with proper value
			builder.makeBuild(BuildTarget.WebPlayer, WEB_BUILD_FILENAME);

			Debug.LogError("Build complete.");
			return 0;
		} catch (System.Exception ex) {
			Debug.LogException(ex);
			Debug.LogError("Build failed.");
			return 1;
		}
	}

	[ExecuteInEditMode]
	[MenuItem("Build/Build Native Client")]
	public static int BuildNativeClients()
	{
		try {
			Debug.Log("Building native clients.");
			
			BuildClient builder = new BuildClient();
			builder.makeClientBuild(CLIENT_TYPE.eClientType_Standalone_Win, BuildTarget.StandaloneWindows, WINDOWS_BUILD_FILENAME, false);
			builder.makeClientBuild(CLIENT_TYPE.eClientType_Standalone_OSX, BuildTarget.StandaloneOSXIntel, MAC_BUILD_FILENAME, false);
			builder.makeClientBuild(CLIENT_TYPE.eClientType_Standalone_Lin, BuildTarget.StandaloneLinuxUniversal, LINUX_BUILD_FILENAME, false);

			Debug.LogError("Build complete.");
			return 0;
		} catch (System.Exception ex) {
			Debug.LogException(ex);
			Debug.LogError("Build failed.");
			return 1;
		}
	}

	[ExecuteInEditMode]
	[MenuItem("Build/Build Test Native Clients")]
	public static int BuildTestNativeClients()
	{
		try {
			Debug.Log("Building test native clients.");
			
			BuildClient builder = new BuildClient();
			builder.makeClientBuild(CLIENT_TYPE.eClientType_Standalone_Win, BuildTarget.StandaloneWindows, WINDOWS_BUILD_FILENAME, true);
			builder.makeClientBuild(CLIENT_TYPE.eClientType_Standalone_OSX, BuildTarget.StandaloneOSXIntel, MAC_BUILD_FILENAME, true);
			builder.makeClientBuild(CLIENT_TYPE.eClientType_Standalone_Lin, BuildTarget.StandaloneLinuxUniversal, LINUX_BUILD_FILENAME, true);

			Debug.LogError("Build complete.");
			return 0;
		} catch (System.Exception ex) {
			Debug.LogException(ex);
			Debug.LogError("Build failed.");
			return 1;
		}
	}

	[ExecuteInEditMode]
	[MenuItem("Build/Build Steam Native clients")]
	public static int BuildSteamNativeClients()
	{
		try {
			Debug.Log("Building steam native clients.");
			
			BuildClient builder = new BuildClient();
			// Steam is only 32-bit for now as the Steam SDK does not support 64-bit apps
			builder.makeSteamBuild(CLIENT_TYPE.eClientType_Standalone_Win, BuildTarget.StandaloneWindows, WINDOWS_BUILD_FILENAME);
			builder.makeSteamBuild(CLIENT_TYPE.eClientType_Standalone_OSX, BuildTarget.StandaloneOSXIntel, MAC_BUILD_FILENAME);
			// Steam is only 32-bit for now as the Steam SDK does not support 64-bit apps
			builder.makeSteamBuild(CLIENT_TYPE.eClientType_Standalone_Lin, BuildTarget.StandaloneLinux, LINUX_BUILD_FILENAME);

			Debug.LogError("Build complete.");
			return 0;
		} catch (System.Exception ex) {
			Debug.LogException(ex);
			Debug.LogError("Build failed.");
			return 1;
		}
	}

	[ExecuteInEditMode]
	[MenuItem("Build/Build All")]
	public static int BuildAll()
	{
		BuildAssets();
		BuildWebClient();
		BuildNativeClients();
		BuildTestNativeClients();
		BuildSteamNativeClients();

		return 0;
	}



	//TODO: decide on an actual directory and naming structure
	public readonly static string WINDOWS_DIR_NAME = "win";
	public readonly static string MAC_DIR_NAME = "mac";
	public readonly static string LINUX_DIR_NAME = "lin";
	public readonly static string WEB_DIR_NAME = "Web";
	
	public readonly static string CLIENT_DIR_PREFIX = "Client";
	public readonly static string TESTCLIENT_DIR_PREFIX = "TestClient";
	public readonly static string STEAM_DIR_PREFIX = "Steam";
	
	public readonly static string WEB_BUILD_FILENAME = "WebPlayer";
	public readonly static string WINDOWS_BUILD_FILENAME = "Akaneiro.exe";
	public readonly static string MAC_BUILD_FILENAME = "Akaneiro.app";
	public readonly static string LINUX_BUILD_FILENAME = "Akaneiro";

	private static string uiManagerName = "UI manager";
	private GameObject uiManager = null;

	public BuildClient() {
		openPersistentSceneIfNotAlreadyOpen();
		uiManager = GameObject.Find(uiManagerName);
	}
	
	private bool openPersistent() {
		string persistentSceneName = "_PersistentScene.unity";
		string persistentScenePath = "Assets/Scenes/GUI/" + persistentSceneName;
		persistentScenePath.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
		return EditorApplication.OpenScene(persistentScenePath);
	}
	
	private bool openPersistentSceneIfNotAlreadyOpen() {
		string persistentSceneName = "_PersistentScene.unity";
		string persistentScenePath = "Assets/Scenes/GUI/" + persistentSceneName;
		persistentScenePath.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
		if (EditorApplication.currentScene == persistentScenePath) {
			return true;
		} else {
			bool openedMap = EditorApplication.OpenScene(persistentScenePath);
			if (openedMap != true) {
				Debug.LogError("Failed to open scene: " + persistentScenePath);
			}
			return openedMap;
		}
	}

	private void makeBuild(BuildTarget buildTarget, string ouputFilename) {
		if (EditorUserBuildSettings.activeBuildTarget != buildTarget) {
		 	EditorUserBuildSettings.SwitchActiveBuildTarget(buildTarget);
		}

		string[] scenes = {
			"Assets/Scenes/GUI/_PersistentScene.unity".Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar),
			"Assets/Scenes/GUI/EmptyScenes.unity".Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar),
			"Assets/Scenes/Areas/CharacterCreate.unity".Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
		};
		BuildOptions options = BuildOptions.None;
		string error = BuildPipeline.BuildPlayer(scenes, ouputFilename, buildTarget, options);
		throw new Exception ("BuildPlayer failed: " + error);
	}
	
	private void makeClientBuild(CLIENT_TYPE clientType, BuildTarget buildTarget, string buildFilename, bool isTestClient) {
		this.setBuildFlagsClient(clientType);
		this.makeBuild(buildTarget, buildFilename);
	}

	private void makeSteamBuild(CLIENT_TYPE clientType, BuildTarget buildTarget, string buildFilename) {
		this.setBuildFlagsSteam(clientType);
		this.makeBuild(buildTarget, buildFilename);
	}

	private void createBundles() {
		// create ALL the bundles!
		CreateAssetbundles.Execute();
		BuildBundleTexturesCartoon.Execute();
		
		BuildBundleTexturesMaterial.Execute();
		BuildBundleTexturesMiniMap.Execute();
		BuildBundleTexturesNPC.Execute();
		BuildBundleTexturesOtherBg.Execute();
		BuildBundleTexturesWorldMap.Execute();
		
		BuildBundleMonsters.Execute();
		BuildBundleItems.Execute();
	}
	
	private void setBuildFlags(bool isWebLogin, string myVersion, bool isClient, CLIENT_TYPE clientType, bool isTestClient, bool isSteamClient) {
		this.setWebLogin(isWebLogin);
		this.setMyVersion(myVersion);
		this.setClientVersion(isClient);
		this.setClientType(clientType);
		this.setClientTestVersion(isTestClient);
		this.setSteamworks(isSteamClient);
	}
	
	private void setBuildFlagsWeb(string myVersion) {
		this.setBuildFlags(true, myVersion, false, CLIENT_TYPE.eClientType_WebPlayer, false, false);
	}
	
	private void setBuildFlagsClient(CLIENT_TYPE clientType) {
		this.setBuildFlags(false, null, true, clientType, false, false);
	}
	
	private void SetBuildFlagsTestClient(CLIENT_TYPE clientType) {
		this.setBuildFlags(false, null, true, clientType, true, false);
	}
	
	private void setBuildFlagsSteam(CLIENT_TYPE clientType) {
		this.setBuildFlags(false, null, false, clientType, false, true);
	}
	
	private void setClientType(CLIENT_TYPE setting) {
		// sets the client type for analytics
		Platform platform = this.uiManager.GetComponent<Platform>();
		platform.eClientType = setting;
	}
	
	private void setClientVersion(bool setting) {
		// is this a standalone client build?
		ClientLogicCtrl clientLogicCtrl = this.uiManager.GetComponent<ClientLogicCtrl>();
		clientLogicCtrl.isClientVer = setting;
	}
	
	private void setClientTestVersion(bool setting) {
		// is the client build connecting to the test server?
		ClientLogicCtrl clientLogicCtrl = this.uiManager.GetComponent<ClientLogicCtrl>();
		clientLogicCtrl.isConnectTestServer = setting;
	}
	
	private void setMyVersion(string setting) {
		// set the version string for web versions
		if ((setting == null) || (setting == "")) {
			setting = System.DateTime.Today.ToString("yyyyMMdd");
		}
		WebLoginCtrl webLoginCtrl = this.uiManager.GetComponent<WebLoginCtrl>();
		webLoginCtrl.MyVersion = setting;
	}
	
	private void setWebLogin(bool setting) {
		// use web credentials for web build
		WebLoginCtrl webLoginCtrl = this.uiManager.GetComponent<WebLoginCtrl>();
		webLoginCtrl.IsWebLogin = setting;
	}
	
	private void setSteamworks(bool setting) {
		// is this a Steam build?
		Steamworks steamworks = this.uiManager.GetComponent<Steamworks>();
		steamworks.isSteamWork = setting;	
	}	
}
