using System;
using System.Collections.Generic;
using UnityEngine;
using Object=UnityEngine.Object;

// When analyzing the available assets UpdateCharacterElementDatabase creates
// a CharacterElement for each possible element. For instance, one mesh with
// three possible textures results in three CharacterElements. All 
// CharacterElements are saved as part the CharacterGenerator ScriptableObject,
// and can be used on runtime to download and load the assets required for the
// element they represent.
[Serializable]
public class CharacterElement
{
    public string name;
    public string bundleName;
	public int    version = 1;
	private bool  bCached = false;
   
    // The WWWs for retrieving the appropriate assetbundle are stored 
    // statically, so CharacterElements that share an assetbundle can
    // use the same WWW.
    // path to assetbundle -> WWW for retieving required assets
    static Dictionary<string, WWW> wwws = new Dictionary<string, WWW>();

    // The required assets are loaded asynchronously to avoid delays
    // when first using them. A LoadAsync results in an AssetBundleRequest
    // which are stored here so we can check their progress and use the
    // assets they contain once they are loaded.
    AssetBundleRequest gameObjectRequest;
    AssetBundleRequest materialRequest;
    AssetBundleRequest boneNameRequest;
	
	Object m_GameObject = null;
	Object m_Material = null;
	Object m_BoneNames = null;
	
	public int GemID;

    public CharacterElement(string name, string bundleName)
    {
        this.name = name;
        this.bundleName = bundleName;
    }

    // Returns the WWW for retieving the assetbundle required for this 
    // CharacterElement, and creates a WWW only if one doesnt exist already. 
    public WWW WWW
    {
        get
        {
			
            /*if (!wwws.ContainsKey(bundleName))
                wwws.Add(bundleName, new WWW(CharacterGenerator.AssetbundleBaseURL + bundleName));*/
			
		   // if(Caching.IsVersionCached(CharacterGenerator.AssetbundleBaseURL + bundleName,version))
				//bCached = true;
			//else
		    bCached = false;
          
			if(!wwws.ContainsKey(bundleName))
				  wwws.Add(bundleName, new WWW(CharacterGenerator.AssetbundleBaseURL + bundleName));  //WWW.LoadFromCacheOrDownload(CharacterGenerator.AssetbundleBaseURL + bundleName,version));
			
			    //wwws.Add(bundleName, WWW.LoadFromCacheOrDownload(CharacterGenerator.AssetbundleBaseURL + bundleName,version));
			  	
            return wwws[bundleName];
        }
    }

    // Checks whether the SkinnedMeshRenderer and Material for this
    // CharacterElement are loaded, and starts the asynchronous loading
    // of those assets if it has not started already.
    public bool IsLoaded
    {
        get
        {
            if (!WWW.isDone) return false;
			
			if(!bCached)
			{
               if (gameObjectRequest == null)
                   gameObjectRequest = WWW.assetBundle.LoadAsync("rendererobject", typeof(GameObject));

               if (materialRequest == null)
               {
                   materialRequest = WWW.assetBundle.LoadAsync(name, typeof(Material));
               }
            
			   if (boneNameRequest == null)
                   boneNameRequest = WWW.assetBundle.LoadAsync("bonenames",typeof(StringHolder));

               if (!gameObjectRequest.isDone) return false;
               if (!materialRequest.isDone)   return false;
               if (!boneNameRequest.isDone)   return false;
			}
			else
			{
				if( m_GameObject == null)
					m_GameObject = WWW.assetBundle.Load("rendererobject", typeof(GameObject));
				
				if( m_Material == null)
					m_Material = WWW.assetBundle.Load(name, typeof(Material));
				
				if( m_BoneNames == null)
					m_BoneNames = WWW.assetBundle.Load("bonenames",typeof(StringHolder));
				
			}

            return true;
        }
    }

    public SkinnedMeshRenderer GetSkinnedMeshRenderer()
    {
        GameObject go;
		if(!bCached)
		{
		   go = (GameObject)Object.Instantiate(gameObjectRequest.asset);
           go.renderer.material = (Material)materialRequest.asset;
		}
		else
		{
		   go = (GameObject)Object.Instantiate(m_GameObject);
		   go.renderer.material = (Material)m_Material;
		   
		}
        return (SkinnedMeshRenderer)go.renderer;
    }

    public string[] GetBoneNames()
    {
		StringHolder holder ;
		
		if(!bCached)
		   holder = (StringHolder)boneNameRequest.asset;
		else
		   holder = (StringHolder)m_BoneNames;
		
		
        return holder.content;
    }
}