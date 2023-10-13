using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class ItemDownLoading : MonoBehaviour {

	// Use this for initialization
	
	static public ItemDownLoading Instance = null;
	
	public string MaterialBundle ="";
	
	public string MeshBundle="";
	
	public string SheathBundle="";
	
	public string SoundBundle="";
	
	public string BoyIconBundle = "";
	
	public string GirlIconBundle = "";

    [HideInInspector]
	public bool bStartDownLoadModel = false;
	
	[HideInInspector]
	public bool bStartDownLoadIcon = false;
	
	public class ItemDownLoadData
	{
		public string BundleString;
        public bool bHasDownloaded
        {
            get
            {
                return m_hasDownloaded;
            }
            set
            {
                m_hasDownloaded = value;
                if (m_hasDownloaded)
                {
                    for (int i = 0; i < NeedToBeSetTextures.Count; i++)
                    {
						UITexture _tex = NeedToBeSetTextures[i];
						if(_tex != null)
						{
	                        NeedToBeSetTextures[i].mainTexture = (Texture2D)ItemObject;
							NGUITools.SetActiveSelf(NeedToBeSetTextures[i].gameObject, true);
						}
                    }
					NeedToBeSetTextures.Clear();
					
                }
            }
        }
        bool m_hasDownloaded;
		public UnityEngine.Object ItemObject;
		public List<UIButton> mClickedButtons = new List<UIButton>();
		public WWW CachedItemWWW;

        List<UITexture> NeedToBeSetTextures = new List<UITexture>();
        public void AddANewUITexture(UITexture _texture)
        {
			if(_texture != null)
            	NeedToBeSetTextures.Add(_texture);
			else
				LogManager.Log_Warn("[UI] target uitexutre is null!");
        }
	}
	
	[HideInInspector]
	public static Dictionary<string,ItemDownLoading.ItemDownLoadData> CachedObjectMap = new Dictionary<string,ItemDownLoading.ItemDownLoadData>();

    public bool isDownloading = false;
	
	void Start () {
	    bStartDownLoadModel = true;
		bStartDownLoadIcon = true;
	}
	
	// Update is called once per frame
	void Update () {

        if (bStartDownLoadModel)
        {
            StartCoroutine(StartDownload(1));
            bStartDownLoadModel = false;
        }

        if (bStartDownLoadIcon)
        {
            StartCoroutine(StartDownload(2));
            bStartDownLoadIcon = false;
        }

        if (transform.collider != null)
        {
            transform.collider.isTrigger = true;
        }
        else
        {
            MeshFilter mft = transform.GetComponentInChildren<MeshFilter>();

            if (mft != null && mft.mesh != null)
            {
                if (mft.gameObject != null && mft.gameObject.collider != null)
                {
                    mft.gameObject.collider.isTrigger = true;

                }
            }
        }

        if (MeshBundle.Length > 0)
        {
            MeshFilter tmeshFilter = transform.GetComponentInChildren<MeshFilter>();

            if (tmeshFilter != null && (tmeshFilter.mesh == null || (tmeshFilter.mesh != null && tmeshFilter.mesh.vertices.Length == 0)))
            {
                if (CachedObjectMap.ContainsKey(MeshBundle))
                {
                    if (CachedObjectMap[MeshBundle].CachedItemWWW != null && CachedObjectMap[MeshBundle].CachedItemWWW.assetBundle != null)
                    {
                        Mesh MainMesh = (Mesh)CachedObjectMap[MeshBundle].CachedItemWWW.assetBundle.mainAsset;

                        if (tmeshFilter != null && MainMesh != null)
                        {
                            tmeshFilter.mesh = (Mesh)Instantiate(MainMesh);
                        }
                    }
                }
            }
        }

        if (MaterialBundle.Length > 0)
        {
            Renderer mRender = transform.GetComponentInChildren<Renderer>();

            if (mRender != null && mRender.material != null && mRender.material.mainTexture == null && CachedObjectMap.ContainsKey(MaterialBundle))
            {
                if (CachedObjectMap[MaterialBundle].CachedItemWWW != null && CachedObjectMap[MaterialBundle].CachedItemWWW.assetBundle != null)
                {
                    Material tMaterial = (Material)CachedObjectMap[MaterialBundle].CachedItemWWW.assetBundle.mainAsset;

                    if (tMaterial != null)
                    {
                        mRender.material = (Material)Instantiate(tMaterial);
                        checkIfArmor();
                    }
                }
                if (CachedObjectMap[MaterialBundle].ItemObject != null)
                {
                    Material tMaterial = (Material)CachedObjectMap[MaterialBundle].ItemObject;

                    if (tMaterial != null)
                    {
                        mRender.material = (Material)Instantiate(tMaterial);
                        checkIfArmor();
                    }
                }
            }
        }
	}
	
	IEnumerator StartDownload(int iPart)
	{
        string strDownLoadFolder = BundlePath.AssetbundleBaseURL;
		
		string strDownLoadFile = "";
		
		WWW tempWWW = null;
		
		if(iPart == 1)
		{
		  if( MaterialBundle.Length > 0)
		  {
			 Material tMaterial = null;
				
			 if(!CachedObjectMap.ContainsKey(MaterialBundle))
		     {
			    strDownLoadFile = strDownLoadFolder + MaterialBundle + ".assetbundle";
			
			    tempWWW = new WWW(strDownLoadFile);
					
				ItemDownLoadData newItemData =  new ItemDownLoadData();
					
				newItemData.bHasDownloaded = false;
					
				newItemData.BundleString = MaterialBundle;
					
				newItemData.ItemObject = null;
					
				newItemData.CachedItemWWW = tempWWW;
					
				CachedObjectMap.Add(MaterialBundle,newItemData);
					
	            yield return tempWWW;
			
			    tMaterial = (Material)tempWWW.assetBundle.mainAsset;
					
				if(tMaterial!= null)
			    {
				  CachedObjectMap[MaterialBundle].bHasDownloaded = true;
						
				  CachedObjectMap[MaterialBundle].ItemObject = tMaterial;
						
				  //Debug.Log( MaterialBundle +" bundle download success");
			    }
			     
			 }
				
			 if(CachedObjectMap.ContainsKey(MaterialBundle))
		     {
				if(!CachedObjectMap[MaterialBundle].bHasDownloaded)
					 yield return null;
				if(CachedObjectMap[MaterialBundle].bHasDownloaded)
					tMaterial = (Material)(CachedObjectMap[MaterialBundle].ItemObject);
		     }
             
             if (transform.renderer && tMaterial != null && transform.renderer.material != null)
		     {
                 if (transform.renderer.material.mainTexture != tMaterial.mainTexture)
			        transform.renderer.material = (Material)Instantiate(tMaterial);
                 checkIfArmor();
			 }
			 else
			 {
				Renderer ToRender = transform.GetComponentInChildren<Renderer>();

                if (tMaterial != null && ToRender && ToRender.material != null )
                {
                    if (ToRender.material.mainTexture != tMaterial.mainTexture)
                        ToRender.material = (Material)Instantiate(tMaterial);
                    checkIfArmor();
                }	
			 }
		   } 
		   
		   if(MeshBundle.Length > 0)
		   {
			  Mesh MainMesh = null;
			
			  if(!CachedObjectMap.ContainsKey(MeshBundle))
			  {
		         strDownLoadFile = strDownLoadFolder + MeshBundle + ".assetbundle";
				
		         tempWWW = new WWW(strDownLoadFile);
					
				 ItemDownLoadData newItemData =  new ItemDownLoadData();
					
				 newItemData.bHasDownloaded = false;
					
				 newItemData.BundleString = MeshBundle;
					
				 newItemData.ItemObject = null;
					
				 newItemData.CachedItemWWW = tempWWW;
				
				 CachedObjectMap.Add(MeshBundle,newItemData);
				
	             yield return tempWWW;
			
		         MainMesh  = (Mesh)tempWWW.assetBundle.mainAsset; 
					
				 if(MainMesh != null)
				 {
					 CachedObjectMap[MeshBundle].bHasDownloaded = true;
						
				     CachedObjectMap[MeshBundle].ItemObject = MainMesh;
				 }
			  }
			 
			  if(CachedObjectMap.ContainsKey(MeshBundle))
			  {
				 if(!CachedObjectMap[MeshBundle].bHasDownloaded)
					yield return null;
					
				 if(CachedObjectMap[MeshBundle].bHasDownloaded)	
					MainMesh = (Mesh)CachedObjectMap[MeshBundle].ItemObject;	
					
			  }
				
			  MeshFilter tmeshFilter = transform.GetComponentInChildren<MeshFilter>();
			   
			  if(tmeshFilter != null && MainMesh != null)
			  {
				 tmeshFilter.mesh = (Mesh)Instantiate(MainMesh);
			  }
		     
		   }
			
		   if(SheathBundle.Length > 0)
		   {
			  Transform SheathTransform = null;
					
			  if(!CachedObjectMap.ContainsKey(SheathBundle))
			  {
		         strDownLoadFile = strDownLoadFolder + SheathBundle + ".assetbundle";
				
		         tempWWW = new WWW(strDownLoadFile);
					
				 ItemDownLoadData newItemData =  new ItemDownLoadData();
					
				 newItemData.bHasDownloaded = false;
					
				 newItemData.BundleString = SheathBundle;
					
				 newItemData.ItemObject = null;
					
				 newItemData.CachedItemWWW = tempWWW;
				 
				 CachedObjectMap.Add(SheathBundle,newItemData);
				
	             yield return tempWWW;
			
		         SheathTransform  = (Transform)tempWWW.assetBundle.mainAsset; 
					
				 if(SheathTransform != null)
				 {
					CachedObjectMap[SheathBundle].bHasDownloaded = true;
					CachedObjectMap[SheathBundle].ItemObject = SheathTransform;
				 }
			  }
				
			  if(CachedObjectMap.ContainsKey(SheathBundle))
			  {
				  if(!CachedObjectMap[SheathBundle].bHasDownloaded)
					  yield return null;
				  if(CachedObjectMap[SheathBundle].bHasDownloaded)
					SheathTransform = (Transform)CachedObjectMap[SheathBundle].ItemObject;		
			  }
			  
			  if(transform.GetComponent<WeaponBase>() && SheathTransform != null)
				 transform.GetComponent<WeaponBase>().sheath = SheathTransform;
		   }
		
		   if( SoundBundle.Length > 0)
		   {
			   Transform trAttackSound = null;
				
			   if(!CachedObjectMap.ContainsKey(SoundBundle))
			   {
				  strDownLoadFile = strDownLoadFolder + SoundBundle + ".assetbundle";
			
			      tempWWW = new WWW(strDownLoadFile);
				  
				  ItemDownLoadData newItemData =  new ItemDownLoadData();
					
				  newItemData.bHasDownloaded = false;
					
				  newItemData.BundleString = SoundBundle;
					
				  newItemData.ItemObject = null;
				  
				  newItemData.CachedItemWWW = tempWWW;
					
				  CachedObjectMap.Add(SoundBundle,newItemData);
				
	              yield return tempWWW;
			
			      trAttackSound = (Transform)tempWWW.assetBundle.mainAsset;
					
				  if(trAttackSound != null)
				  {  
					CachedObjectMap[SoundBundle].bHasDownloaded = true;
					CachedObjectMap[SoundBundle].ItemObject = trAttackSound;
				  }
			   }
			   	
			   if(CachedObjectMap.ContainsKey(SoundBundle))
			   {
					if(!CachedObjectMap[SoundBundle].bHasDownloaded)
						yield return null;
					
					if(CachedObjectMap[SoundBundle].bHasDownloaded)
					   trAttackSound = (Transform)CachedObjectMap[SoundBundle].ItemObject;
			   }
	
			   if(transform.GetComponent<WeaponBase>() && trAttackSound != null)
			      transform.GetComponent<WeaponBase>().AttackSound = trAttackSound;
		   }
		}
		else if( iPart == 2)
		{
		   if(BoyIconBundle.Length > 0)
		   {
			  Texture2D boyIcon = null;
			  
			  if(!CachedObjectMap.ContainsKey(BoyIconBundle))
			  {
//				Debug.Log(BoyIconBundle + " item try to be downloaded");
					
			    strDownLoadFile =  strDownLoadFolder + BoyIconBundle + ".assetbundle";
			  
			    tempWWW = new WWW(strDownLoadFile);
					
				ItemDownLoadData newItemData =  new ItemDownLoadData();
					
				newItemData.bHasDownloaded = false;
					
			    newItemData.BundleString = BoyIconBundle;
					
				newItemData.ItemObject = null;
					
				newItemData.CachedItemWWW = tempWWW;
				 
				CachedObjectMap.Add(BoyIconBundle,newItemData);
			
			    yield return tempWWW;
					
			    //if (tempWWW.error != null) Debug.Log(BoyIconBundle + "DownLoadFailed " + tempWWW.error);
				
			    boyIcon = (Texture2D)tempWWW.assetBundle.mainAsset;
					
				if( boyIcon != null)
				{
				   CachedObjectMap[BoyIconBundle].bHasDownloaded = true;
				   CachedObjectMap[BoyIconBundle].ItemObject = boyIcon;
				}
			  }
			   
			  if(CachedObjectMap.ContainsKey(BoyIconBundle))
			  {
				  if(!CachedObjectMap[BoyIconBundle].bHasDownloaded)
				         yield return null;
					
				  if(CachedObjectMap[BoyIconBundle].bHasDownloaded)
				  {
//					 Debug.Log(BoyIconBundle + " item succeed to be downloaded");
						
					 boyIcon = (Texture2D)CachedObjectMap[BoyIconBundle].ItemObject;
				  }
			  }
			  
				
			  if(transform.GetComponent<Item>() && boyIcon != null)
				 transform.GetComponent<Item>().Normal_State_IconBoy = 	boyIcon;
		   }
			
		   if( GirlIconBundle.Length > 0)
		   {
			   Texture2D girlIcon = null;
					
			   if(!CachedObjectMap.ContainsKey(GirlIconBundle))
			   {
//				  Debug.Log(GirlIconBundle + " item try to be downloaded");
					
			      strDownLoadFile = strDownLoadFolder + GirlIconBundle + ".assetbundle";
			   	
			      tempWWW = new WWW(strDownLoadFile);
			
				  ItemDownLoadData newItemData =  new ItemDownLoadData();
					
				  newItemData.bHasDownloaded = false;
					
			      newItemData.BundleString = GirlIconBundle;
					
				  newItemData.ItemObject = null;
					
				  newItemData.CachedItemWWW = tempWWW;
					
				  CachedObjectMap.Add(GirlIconBundle,newItemData);
				
			      yield return tempWWW;
				
			      girlIcon = (Texture2D)tempWWW.assetBundle.mainAsset;
					
				  // if (tempWWW.error != null) Debug.Log(GirlIconBundle + "DownLoadFailed " + tempWWW.error);
					
				  if(girlIcon != null)
				  {
					  CachedObjectMap[GirlIconBundle].bHasDownloaded = true;
				      CachedObjectMap[GirlIconBundle].ItemObject = girlIcon;
				  }
			   }
			  	
			   if( CachedObjectMap.ContainsKey(GirlIconBundle))
			   {
				   if(!CachedObjectMap[GirlIconBundle].bHasDownloaded)
					  yield return null;
					
				   if(CachedObjectMap[GirlIconBundle].bHasDownloaded)
				   {
//					  Debug.Log(GirlIconBundle + " item succeed to be downloaded");
					  girlIcon = (Texture2D)CachedObjectMap[GirlIconBundle].ItemObject;
				   }
			   }
				
			   if(transform.GetComponent<Item>() && girlIcon != null)
				  transform.GetComponent<Item>().Normal_State_IconGirl = girlIcon;
		   }
			
		   
		}
	}

    void checkIfArmor()
    {
		//Debug.LogError(transform.name + "check armor");
        if (transform.GetComponent<ArmorBase>())
        {
            transform.GetComponent<ArmorBase>().InitArmorColorWithItemInfo(transform.GetComponent<ArmorBase>().ItemInfo);
        }

        if (transform.GetComponent<ArmorBase>() && transform.GetComponent<ArmorBase>().ReplaceInstance && transform.GetComponent<ArmorBase>().ReplaceInstance.GetComponent<ArmorBase>())
        {
            transform.GetComponent<ArmorBase>().ReplaceInstance.GetComponent<ArmorBase>().InitArmorColorWithItemInfo(transform.GetComponent<ArmorBase>().ItemInfo);
        }
		
		// because we are using the material which has a edgewidth. nomally, the value of width is big, that could make the item be seen dark.
		// so we check if this item is used to UI, reset the edgewidth to 0.5f.
		if(gameObject.layer == LayerMask.NameToLayer("NGUI") && renderer != null)
		{
			foreach (Material _mat in renderer.materials)
	        {
				if(_mat == null) {
					continue;
				}
	            if (_mat.HasProperty("_EdgeWidth"))
	            {
	                _mat.SetFloat("_EdgeWidth", 0.5f);
	            }
	        }
		}
    }
}
