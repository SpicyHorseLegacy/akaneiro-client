using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public struct ValStruct
{
  public int value;
  public bool bmodify;
}
	
class UpdateCharacterElementDatabase
{
	
	static Dictionary<string, ValStruct> ResourceVersions = new Dictionary<string, ValStruct>();
	
	
	static WWW database = null;
	
	public static void ScanVersion()
	{
	   // if (database == null)
	      database = new WWW(CharacterGenerator.AssetbundleBaseURL + "CharacterElementDatabase.assetbundle");
	}
		
    // This method collects information about all available
    // CharacterElements stores it in the CharacterElementDatabase
    // assetbundle. Which CharacterElements are available is 
    // determined by checking the generated materials.
    [MenuItem("Character Generator/Update Character Element Database")]
    public static void Execute()
    {
	   // ScanVersion();
		/*
		ResourceVersions.Clear();
		
		if( database.assetBundle != null)
		{
	      CharacterElementHolder ceh = (CharacterElementHolder) database.assetBundle.mainAsset;
	
	      foreach (CharacterElement element in ceh.content)
	      {
			 if (!ResourceVersions.ContainsKey(element.bundleName))
			 {
			   ValStruct theValue = new ValStruct();
			   theValue.value = element.version;
			   theValue.bmodify = false;
	      	   ResourceVersions.Add(element.bundleName,theValue);
			 }
	      }
		}
		*/
		
	
        List<CharacterElement> characterElements = new List<CharacterElement>();
			
		
        // As a CharacterElement needs the name of the assetbundle
        // that contains its assets, we go through all assetbundles
        // to match them to the materials we find.
        string[] assetbundles = Directory.GetFiles(CreateAssetbundles.AssetbundlePath);
        string[] materials = Directory.GetFiles("Assets/Model/Character", "*.mat",SearchOption.AllDirectories);
	    
		
        foreach (string material in materials)
        {
            foreach (string bundle in assetbundles)
            {
				
                FileInfo bundleFI = new FileInfo(bundle);
				if(bundleFI.Name.Contains("SamuraiJack"))
					Debug.Log(bundleFI.Name);
                FileInfo materialFI = new FileInfo(material);
                string bundleName = bundleFI.Name.Replace(".assetbundle", "");
				string localMaterial = materialFI.Name.ToLower();
                if (!localMaterial.Contains(bundleName)) continue;
                if (!material.Contains("Materials")) continue;
				
				CharacterElement newCharacterElement = new CharacterElement(materialFI.Name.Replace(".mat", ""), bundleFI.Name);
				
				if(ResourceVersions.ContainsKey(newCharacterElement.bundleName) )
				{
					if(!ResourceVersions[newCharacterElement.bundleName].bmodify)
					{
						 ValStruct theValue = ResourceVersions[newCharacterElement.bundleName];
						 theValue.bmodify = true;
						 theValue.value += 1;
						 ResourceVersions[newCharacterElement.bundleName] = theValue;
					}
						
					newCharacterElement.version = ResourceVersions[newCharacterElement.bundleName].value;
				}
                characterElements.Add(newCharacterElement);
                break;
            }
        }

        // After collecting all CharacterElements we store them in an
        // assetbundle using a ScriptableObject.

        // Create a ScriptableObject that contains the list of CharacterElements.
		CharacterElementHolder t = ScriptableObject.CreateInstance<CharacterElementHolder> ();
		t.content = characterElements;

        // Save the ScriptableObject and load the resulting asset so it can 
        // be added to an assetbundle.
        string p = "Assets/CharacterElementDatabase.asset";
        AssetDatabase.CreateAsset(t, p);
		Object o = AssetDatabase.LoadAssetAtPath(p, typeof(CharacterElementHolder));

        // Build the CharacterElementDatabase assetbundle.
        BuildPipeline.BuildAssetBundle(o, null, CreateAssetbundles.AssetbundlePath + "CharacterElementDatabase.assetbundle", BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets, CreateAssetbundles.GetCurrentTarget());

        // Delete the ScriptableObject.
        AssetDatabase.DeleteAsset(p);

        Debug.Log("******* Updated Character Element Database, added " + characterElements.Count + " elements *******");
    }
}