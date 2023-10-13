using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using System.Security.Cryptography;
using System.Collections;


public class BuildBundleTexturesMiniMap
{
	
    [MenuItem("Build/Bundle/Textures/MiniMap")]
    public static void Execute()
    {
		List<Texture> TextureList = new List<Texture>();
		
		string[] strGetTexture = Directory.GetFiles( "Assets/TextureBundles/MiniMap","*",SearchOption.AllDirectories);
		
		foreach( string texFile in strGetTexture)
		{
			Texture  Newtex  = AssetDatabase.LoadAssetAtPath(texFile,typeof(Texture)) as Texture;
			
			if( Newtex != null)
				TextureList.Add(Newtex);
		}
		
		foreach(Texture eachTex in TextureList)
		{
		    string BundlePath = CreateCurrentSceneBuddle.AssetbundlePath + eachTex.name + ".assetbundle";
			
			bool bPass = false;

            bPass = BuildPipeline.BuildAssetBundle(eachTex, null, BundlePath, BuildAssetBundleOptions.CollectDependencies, CreateAssetbundles.GetCurrentTarget());

		}
		
		
	}
}
