using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TextureDownLoadingMan : MonoBehaviour {
	
	//Instance
	public static TextureDownLoadingMan Instance  = null;
	
	public class TextureDownLoadData{
		public string 		BundleNameString;
		public bool   		bHasDownloaded;
		public Texture2D 	textureObj;
		public WWW 			CachedTextureWWW;
	}
	
	void Awake(){
		Instance = this;
	}
	
	public void DownLoadingTexture(string bundleName,UITexture targetObj) {
		StartCoroutine(StartDownload(bundleName,targetObj));
	}
	public IEnumerator StartDownload(string bundleName,UITexture targetObj) {
		string strDownLoadFolder = CharacterGenerator.AssetbundleBaseURL ;
		if( bundleName.Length > 0) {
			Texture2D textureTemp = null;
			WWW tempWWW = new WWW(strDownLoadFolder + bundleName + ".assetbundle");
			yield return tempWWW;
			if(tempWWW.assetBundle != null) {
				textureTemp = (Texture2D)tempWWW.assetBundle.mainAsset;
				targetObj.mainTexture = textureTemp;
				tempWWW.assetBundle.Unload(false);
			}
		}
	}	

	public void DownLoadingTexture(string bundleName,Transform targetObj) {
		StartCoroutine(StartDownload(bundleName,targetObj));
	}
	IEnumerator StartDownload(string bundleName,Transform targetObj) {
		string strDownLoadFolder = CharacterGenerator.AssetbundleBaseURL ;
		if( bundleName.Length > 0) {
			Texture2D textureTemp = null;
			WWW tempWWW = new WWW(strDownLoadFolder + bundleName + ".assetbundle");
			yield return tempWWW;
			if(tempWWW.assetBundle != null) {
				textureTemp = (Texture2D)tempWWW.assetBundle.mainAsset;
				if (textureTemp != null && targetObj.GetComponent<MeshRenderer>() && targetObj.GetComponent<MeshRenderer>().materials[0] != null){
					if (targetObj.GetComponent<MeshRenderer>().materials[0].mainTexture != textureTemp)
					targetObj.GetComponent<MeshRenderer>().materials[0].mainTexture = textureTemp;
				}
				tempWWW.assetBundle.Unload(false);
			}
		}
	}
	
	
	
}
