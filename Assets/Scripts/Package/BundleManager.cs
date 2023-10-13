using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class AssetItem
{
	public string bundle;
	public List<string> assets;
	public int currentVersion = -1;
	public int size;
	public string hashCode = "";
}

public class BundleManager : ScriptableObject 
{
	/*
	public static BundleManager Instance
	{
		get
		{
			if(instance == null)
				Init();
			
			return instance;
		}
	}
	*/
	
	//private static BundleManager instance = null;
	
	public List<AssetItem> content = new List<AssetItem>();
	
	private Dictionary<string, AssetItem> bundleDic = null;
	private Dictionary<string, AssetItem> assetDic = null;
	
	private Dictionary<string, WWW> dlBundles = new Dictionary<string, WWW>();
	/*
	public static void Init()
	{
		instance = (BundleManager)Resources.Load("BundleAssetList");
		
		if(instance == null)
			instance = new BundleManager();
		
		instance.init();
	}
	*/
	
    public string GetAssetBundleUrl()
	{
	    if (Application.platform == RuntimePlatform.WindowsWebPlayer || Application.platform == RuntimePlatform.OSXWebPlayer)
            return Application.dataPath+"/assetbundles/";
        else if (Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.OSXDashboardPlayer)
			return "file://" + Application.dataPath + "/../assetbundles/";
		else
            return "file://" + Application.dataPath + "/../assetbundles/";
	}
	
	public void Init()
	{
		bundleDic = new Dictionary<string, AssetItem>();
		assetDic = new Dictionary<string, AssetItem>();
		
		foreach (AssetItem ai in content)
		{
			bundleDic[ai.bundle] = ai;
			foreach(string asset in ai.assets)
				assetDic[asset] = ai;
		}
	}
	
	public void Add(List<string> assets, string bundle, string hashCode, int size)
	{
		AssetItem ai = null;
		
		if(bundleDic.ContainsKey(bundle))
		{
			if( bundleDic[bundle].hashCode != hashCode)
			{
				ai = bundleDic[bundle];
				ai.assets = assets;
				ai.currentVersion++;
				ai.size = size;
				ai.hashCode = hashCode;
			}
			else
			{
				Debug.Log("No change to bundle" + bundle);
				return;
			}
		}
		else
		{
			ai = new AssetItem();
			ai.assets = assets;
			ai.bundle = bundle;
			ai.currentVersion = 0;
			ai.size = size;
			ai.hashCode = hashCode;
			
			bundleDic[bundle] = ai;
			content.Add(ai);
		}
		
		foreach(string asset in assets)
			assetDic[asset] = ai;
	}
	
	public WWW GetWWW(string bundleName)
	{
		if(dlBundles.ContainsKey(bundleName))
			return dlBundles[bundleName];
			
		if(!bundleDic.ContainsKey(bundleName))
		{
			Debug.LogError("Cannot found " + bundleName + " in the bundle list.");
			return null;
		}
		
		// Figure out map or normal bundle
		string suffix = bundleName.Substring(bundleName.IndexOf('.') + 1);
		string url = "";
		if(suffix == "assetBundles")
			url = GetAssetBundleUrl() + bundleName;
		else if(suffix == "unity3d")
			//url = GetAssetBundleUrl() + "Maps/" + bundleName;
		    url = GetAssetBundleUrl() + bundleName;
		else
			Debug.LogError("Error bundle type " + suffix);
		
		// Download or cache download
		AssetItem ai = bundleDic[bundleName];
		if(ai.currentVersion == -1)
			dlBundles[bundleName] = new WWW(url);
		else
			dlBundles[bundleName] = WWW.LoadFromCacheOrDownload(url, ai.currentVersion);
		
		Debug.Log(bundleName + " version : " + ai.currentVersion);
		
		return dlBundles[bundleName];
	}
	
	public int GetBundleSize(string bundleName)
	{
		if(bundleDic.ContainsKey(bundleName))
			return bundleDic[bundleName].size;
		else
			return -1;
	}
	
	public string GetBundleName(string assetName)
	{
		if(bundleDic == null)
			Debug.LogError("Bundle Manager haven't inited before using.");
	
		if(assetDic.ContainsKey(assetName))
		{
			return assetDic[assetName].bundle;
		}
		else
		{
			Debug.LogError("Cannot find " + assetName + " in bundles");
			return "";
		}
	}
	
	void OnApplicationQuit()
	{
		foreach(KeyValuePair<string, WWW> pair in dlBundles)
			pair.Value.assetBundle.Unload(false);
	}
	
	/*
	public void BuiltInWWW()
	{
		string BundleURL = GetAssetBundleUrl() + "BuildList.assetbundle";
		
		BuildPipeline.BuildAssetBundle(Instance, null, BundleURL, BuildAssetBundleOptions.CollectDependencies);
		
	}
	*/

}
