using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object=UnityEngine.Object;
using Random=UnityEngine.Random;

// This class can be used to create characters by combining assets.
// The assets are stored in assetbundles to minimize the assets that
// have to be downloaded.
public class CharacterGenerator
{

    // Stores the WWW used to retrieve available CharacterElements stored
    // in the CharacterElementDatabase assetbundle. When storing the available
    // CharacterElements in an assetbundle instead of a ScriptableObject 
    // referenced by a MonoBehaviour, changing the available CharacterElements
    // does not require a client rebuild.
    static WWW database;
	
	static WWW AnimationDatas;
	 
	static List<AnimationClip> AnimationClips = new List<AnimationClip>();
	
    // Stores all CharacterElements obtained from the CharacterElementDatabase 
    // assetbundle, sorted by character and category.
    // character name -> category name -> CharacterElement
    static Dictionary<string, Dictionary<string, List<CharacterElement>>> sortedElements;

    // As elements in a Dictionary are not indexed sequentially we use this list when 
    // determining the previous/next character, instead of sortedElements.
    static List<string> availableCharacters = new List<string>();

    // Stores the WWWs for retrieving the characterbase assetbundles that 
    // hold the bones and animations for a specific character.
    // character name -> WWW for characterbase.assetbundle
    static Dictionary<string, WWW> characterBaseWWWs = new Dictionary<string, WWW>();

    // The bones and animations from the characterbase assetbundles are loaded
    // asynchronously to avoid delays when first using them. A LoadAsync results
    // in an AssetBundleRequest which are stored here so we can check their progress
    // and use the assets they contain once they are loaded.
    // character name -> AssetBundleRequest for Character Base GameObject.
    static Dictionary<string, AssetBundleRequest> characterBaseRequests = new Dictionary<string, AssetBundleRequest>();
	
	static Dictionary<string,Object> characterBaseObjects = new Dictionary<string, Object>();

    // Stores the currently configured character which is used when downloading
    // assets and generating characters.
    string currentCharacter;

    // Stores the current configuration which is used when downloading assets
    // and generating characters.
    // category name -> current character element
    Dictionary<string, CharacterElement> currentConfiguration = new Dictionary<string, CharacterElement>();

    // Used to give a more accurate download progress.
    float assetbundlesAlreadyDownloaded;
	
	bool bCharacterBaseCached = false;
	
    // Avoid users creating instances with a new statement or before
    // sortedElements is populated.
    private CharacterGenerator()
    {
        if (!ReadyToUse) 
            throw new Exception("CharacterGenerator.ReadyToUse must be true before creating CharacterGenerator instances.");
    }

    // The following static methods can be used to create
    // CharacterGenerator instances.
    public static CharacterGenerator CreateWithRandomConfig()
    {
        CharacterGenerator gen = new CharacterGenerator();
        gen.PrepareRandomConfig();
		
        return gen;
    }

    public static CharacterGenerator CreateWithRandomConfig(string character)
    {
        CharacterGenerator gen = new CharacterGenerator();
        gen.PrepareRandomConfig(character);
        return gen;
    }

    public static CharacterGenerator CreateWithConfig(string config)
    {
        CharacterGenerator gen = new CharacterGenerator();
        gen.PrepareConfig(config);
        return gen;
    }

    // A CharacterGenerator instance can be used to create more then
    // one character. The following methods allow changing the configuration
    // after creating an instance.
    public void PrepareRandomConfig()
    {
        PrepareRandomConfig(availableCharacters[Random.Range(0, availableCharacters.Count)]);
    }

    public void PrepareRandomConfig(string character)
    {
		//Debug.Log("prepareRandomConfig : " + character);
        currentConfiguration.Clear();
        currentCharacter = character.ToLower();
//		
//		string[] keys = sortedElements.Keys;
//		foreach(string _key in keys)
//		{
//			Debug.LogError(_key);
//		}
		
        foreach (KeyValuePair<string, List<CharacterElement>> category in sortedElements[currentCharacter])
		{
            currentConfiguration.Add(category.Key, category.Value[0]);
			//Debug.Log("Category : " + category.Key + " || categoryValueName : " + category.Value[0].name + " || categoryBundleName : " + category.Value[0].bundleName);
		}
        UpdateAssetbundlesAlreadyDownloaded();
    }

    // Populates the currentConfiguration from a string to restore
    // saved configurations.
    public void PrepareConfig(string config)
    {
//		Debug.Log("PrepareConfig : " + config);
       // config = config.ToLower();
        string[] settings = config.Split('|');
        currentCharacter = settings[0];
        //currentConfiguration = new Dictionary<string, CharacterElement>();
		currentConfiguration.Clear();
        for (int i = 1; i < settings.Length; )
        {
            string categoryName = settings[i++];
            string elementName = settings[i++];
            CharacterElement element = null;
			string[] a = elementName.Split('_');
		    string character2 = "";
			//a[0] + '_' + a[1] + '_' + a[2] + "_" + a[3];
			for(int j = 0; j < a.Length - 1;j++)
			{
				if( j > 0 )
					character2 += '_';
				
				character2 += a[j];
			}
			character2 = character2.ToLower();
            foreach (CharacterElement e in sortedElements[character2][categoryName])
            {
				string temp = e.name.ToLower();
                if (temp != elementName) continue;
                element = e;
                break;
            }
            if (element == null) throw new Exception("Element not found: " + elementName);
            currentConfiguration.Add(categoryName, element);
        }
        UpdateAssetbundlesAlreadyDownloaded();
    }

    // Returns the currentConfiguration as a string for easy storage.
    public string GetConfig()
    {
        string s = currentCharacter;
        foreach (KeyValuePair<string, CharacterElement> category in currentConfiguration)
            s += "|" + category.Key + "|" + category.Value.name;
        return s;
    }

    // Sets a random configuration for the next or previous character
    // in availableCharacters.
    public void ChangeCharacter(bool next)
    {
        string character = null;
        for (int i = 0; i < availableCharacters.Count; i++)
        {
            if (availableCharacters[i] != currentCharacter) continue;
            if (next)
                character = i < availableCharacters.Count - 1 ? availableCharacters[i + 1] : availableCharacters[0];
            else
                character = i > 0 ? availableCharacters[i - 1] : availableCharacters[availableCharacters.Count - 1];
            break;
        }
        PrepareRandomConfig(character);
    }

    // Sets the configuration of a category to the next or previous
    // CharacterElement in sortedElements.
    public void ChangeElement(string catagory, bool next)
    {
		//Debug.Log("ChangeElement : " + catagory);
        List<CharacterElement> available = sortedElements[currentCharacter][catagory];
        CharacterElement element = null;
        for (int i = 0; i < available.Count; i++)
        {
            if (available[i] != currentConfiguration[catagory]) continue;
            if (next)
                element = i < available.Count - 1 ? available[i + 1] : available[0];
            else
                element = i > 0 ? available[i - 1] : available[available.Count - 1];
            break;
        }
        currentConfiguration[catagory] = element;
        UpdateAssetbundlesAlreadyDownloaded();
    }
	
	public void ChangeCharactorComponent(string charactor,string catagory,bool next)
	{
		//Debug.Log("ChangeCharactorComponent Character : " + charactor + " || catagory : " + catagory);
		List<CharacterElement> available = sortedElements[charactor][catagory];
        CharacterElement element = null;
        for (int i = 0; i < available.Count; i++)
        {
            if (available[i] != currentConfiguration[catagory]) continue;
            if (next)
                element = i < available.Count - 1 ? available[i + 1] : available[0];
            else
                element = i > 0 ? available[i - 1] : available[available.Count - 1];
            break;
        }
		if( element == null)
		{
			element = available[0];
		}
        currentConfiguration[catagory] = element;
        UpdateAssetbundlesAlreadyDownloaded();
		
	}
	
    public  Dictionary<string, CharacterElement>  GetCurrentConfig()
	{
		return currentConfiguration;
	}

    public void ConfigNoneComponent(EquipementManager.EEquipmentType ItemType, ESex sexInfo)
    {
        string sPart = "";
        if (ItemType == EquipementManager.EEquipmentType.Helm)
            sPart = "head";
        else if (ItemType == EquipementManager.EEquipmentType.Breastplate)
            sPart = "chest";
        else if (ItemType == EquipementManager.EEquipmentType.Breeches)
            sPart = "leg";

        string _gendor = sexInfo.Get() == ESex.eSex_Female ? "_f" : "_m";

        string catogeryinfo = "ch_aka" + _gendor;

        currentConfiguration[sPart] = sortedElements[catogeryinfo][sPart][0];
    }

    public void ConfigComponent(EquipementManager.EEquipmentType ItemType, String MaterialName, ESex sexInfo, int GemID)
    {
        string sPart = "";
        if (ItemType == EquipementManager.EEquipmentType.Helm)
            sPart = "head";
        else if (ItemType == EquipementManager.EEquipmentType.Breastplate)
            sPart = "chest";
        else if (ItemType == EquipementManager.EEquipmentType.Breeches)
            sPart = "leg";

        string _gendor = sexInfo.Get() == ESex.eSex_Female ? "_f" : "_m";

        string[] m_info = MaterialName.Split('_');
        string catogeryinfo = "ch_aka_" + m_info[0] + _gendor;
        string indexinfo = sPart + m_info[1];
        int index = 0;

        if (m_info[0] == "headhood" && sPart == "head")
        {
            indexinfo = sPart;

            foreach (CharacterElement _ce in sortedElements[catogeryinfo][indexinfo])
            {
                if (_ce.name.ToLower() == "ch_aka_headhood" + _gendor + "_head" + string.Format("{0:d2}",m_info[1]))
                {
                    currentConfiguration[sPart] = _ce;
                    currentConfiguration[sPart].GemID = GemID;
                    return;
                }
            }
        }

        currentConfiguration[sPart] = sortedElements[catogeryinfo][indexinfo][index];
        currentConfiguration[sPart].GemID = GemID;
    }
	
	
    /*
	public void ConfigComponent(EquipementManager.EEquipmentType ItemType,String MaterialName, ESex sexInfo, int GemID)
	{
		 //Debug.Log("Config Component :" + ItemType.ToString() + " || MaterialName : " + MaterialName + "|| Sex : " + sexInfo.Get());
		 string sCharactor;
		 //string lowerMaterialName =  MaterialName.ToLower();
		 bool bjump = false;
		 string sPart="";
		 if( ItemType == EquipementManager.EEquipmentType.Helm)
			  sPart = "head";
		 else if(ItemType == EquipementManager.EEquipmentType.Breastplate)
			  sPart ="chest";
		 else if(ItemType == EquipementManager.EEquipmentType.Breeches)
			  sPart ="leg";
		
		 string[] a = MaterialName.Split('_');
		 string character2 = a[a.Length - 1];
		 character2 = character2.ToLower();

		 for (int i = 0; i < availableCharacters.Count;i++)
		 {
			//sortedElements[i].ToString()
			sCharactor = availableCharacters[i];
			a = sCharactor.Split('_');
			sCharactor = a[a.Length - 1];
			if(a.Length > 3)
			   sCharactor = a[a.Length - 2];
			
			if( (Isequel(character2, sCharactor) && GetValueFromString(character2) > 0)  || character2 == sCharactor)//character2.Contains(sCharactor) || sCharactor.Contains(character2))
			{
				foreach (KeyValuePair<string, List<CharacterElement>> category in sortedElements[availableCharacters[i]])
				{
                    if (category.Key.Contains(sPart))
                    {
                        if(character2.Contains("headhood") || GetValueFromString(character2) == GetValueFromString(category.Key))
                        {
                            string UpGendor = "M";
                            string DownGendor = "m";
                            if (sexInfo.Get() == ESex.eSex_Female)
                            {
                                UpGendor = "F";
                                DownGendor = "f";
                            }

                            //Debug.Log( "a : " + availableCharacters[i]);
                            String[] something = availableCharacters[i].Split('_');
                            string sex = something[something.Length - 1];
                            if (sex == UpGendor || sex == DownGendor)
                            {
                                int ran = category.Value.Count - 1;
                                if(character2.Contains("headhood"))
                                    ran = GetValueFromString(character2) - 1;

                                //Debug.Log("dididi : " + sPart + " || " + availableCharacters[i] + " || " + category.Key + " || " + ran + " || " + GemID);
                                currentConfiguration[sPart] = sortedElements[availableCharacters[i]][category.Key][ran];
                                currentConfiguration[sPart].GemID = GemID;
                                //Debug.Log("aviCha : " + availableCharacters[i]);
                                bjump = true;
                            }

                            break;
                        }
                    }
				}
			}
			
			if(bjump)
		     break;
			
		 }
           
	}
     * */
	
	int GetValueFromString(string str)
	{
		int result = 0;
		for(int i = 0; i < str.Length;i++)
		{
		   if( str[i] >= '0' && str[i] <= '9')
		   {		
			   result += (Convert.ToInt32(str[i]) - Convert.ToInt32('0')); 
		   } 
		}

		return result;
	}
	
	bool Isequel(string str1,string str2)
	{
		
		int minLength = str1.Length;
		
		if(str2.Length < minLength)
			minLength = str2.Length;
		
		int i = 0;
		
		for(i = 0; i < minLength; i++)
		{
			if(str1[i] != str2[i])
				return false;
		}
		
		if( str1.Length > minLength)
		{
		    for(int j = minLength ; j < str1.Length;j++)
			{
				 if( str1[j] >= '0' && str1[j] <= '9')
				 {
				 }
				 else
				 {
					return false;
				 }
			}
		}
		else if(str2.Length > minLength)
		{
			for(int j = minLength ; j < str2.Length;j++)
			{
				if( str2[j] >= '0' && str2[j] <= '9')
				 {
				 }
				 else
				 {
					return false;
				 }
			}
		}
		
		return true;;
	}
	

    // This method downloads the CharacterElementDatabase assetbundle and populates
    // the sortedElements Dictionary from the contents. This is done at runtime as
    // ScriptableObjects do not support Dictionaries. ReadyToUse must be true before
    // you create an instance of CharacterGenerator.
	public static bool ReadyToUse
	{
		get
		{
		
			if (database == null)
				database = new WWW(AssetbundleBaseURL + "CharacterElementDatabase.assetbundle");
		    
			//if(database == null)
			 // database = WWW.LoadFromCacheOrDownload(AssetbundleBaseURL + "CharacterElementDatabase.assetbundle",1);
			 
			                                 
			if(AnimationDatas == null)
				AnimationDatas = new WWW(AssetbundleBaseURL + "Animations.assetbundle");
			
			//if(AnimationDatas == null)
			  // AnimationDatas = WWW.LoadFromCacheOrDownload(AssetbundleBaseURL + "Animations.assetbundle",1);
		
			if (sortedElements != null && AnimationClips.Count > 0) return true;
			if (!database.isDone) return false;
			if( !AnimationDatas.isDone) return false;
			
			Object[] ob = null;
			try {
				ob = AnimationDatas.assetBundle.LoadAll();
			} catch(NullReferenceException e) {
				string message = "BUNDLE LOAD FAILURE\n\n";
				message += "Catched NullReferenceException assetBundle.LoadAll(): "+ e.ToString() +"\n\n\n\n";
				message += "If you think that this message is not your fault,\n";
				message += "please contact our technical support at http://support.spicyhorse.com/\n\n\n\n";
				message += "PRESS SPACE TO TERMINATE APPLICATION";
				BSOD.Error(message);
				throw e;
			}
			
		    for(int i = 0; i < ob.Length; i++)
			{
				AnimationClips.Add(ob[i] as AnimationClip);
			}
			
			CharacterElementHolder ceh = (CharacterElementHolder) database.assetBundle.mainAsset;

			sortedElements = new Dictionary<string, Dictionary<string, List<CharacterElement>>>();
			foreach (CharacterElement element in ceh.content)
			{
				string[] a = element.bundleName.Split('_');
				string character = "";
				for(int i = 0;i < a.Length - 1; i++)
				{
					if(i > 0)
					   character += '_';
					
					character += a[i];
				}
                
				string category = a[a.Length - 1].Split('-')[0].Replace(".assetbundle", "");
				
				if (!availableCharacters.Contains(character))
					availableCharacters.Add(character);

				if (!sortedElements.ContainsKey(character))
					sortedElements.Add(character, new Dictionary<string, List<CharacterElement>>());

				if (!sortedElements[character].ContainsKey(category))
					sortedElements[character].Add(category, new List<CharacterElement>());

				sortedElements[character][category].Add(element);
                //Debug.LogError("char : " + character + " || category : " + category + " || name : " + element.name + " || bundle" + element.bundleName);
                
			}

			return true;
		}
	}

    // Averages the download progress of all assetbundles required for the currentConfiguration,
    // and takes into account the progress at the time of the last configuration change. This 
    // way we can give a progress indication that runs from 0 to 1 even when some assets were
    // already downloaded.
    public float CurrentConfigProgress
    {
        get
        {
            float toDownload = currentConfiguration.Count + 1 - assetbundlesAlreadyDownloaded;
            if (toDownload == 0) return 1;
            float progress = CurrentCharacterBase.progress;
            foreach (CharacterElement e in currentConfiguration.Values)
                progress += e.WWW.progress;
            return (progress - assetbundlesAlreadyDownloaded) / toDownload;
        }
    }

    // Checks to see if all assets required for the currentConfiguration are loaded, and starts
    // the asynchronous loading of the bones and animations if it has not started already.
    // ConfigReady must be true before calling Generate.
    public bool ConfigReady
    {
        get
        {
            if (!CurrentCharacterBase.isDone) return false;
			
			if(!bCharacterBaseCached)
			{
               if (!characterBaseRequests.ContainsKey(currentCharacter))
                   characterBaseRequests.Add(currentCharacter, CurrentCharacterBase.assetBundle.LoadAsync("characterbase", typeof(GameObject)));
			 
               if (!characterBaseRequests[currentCharacter].isDone) return false;
			}
			else
			{
				  if (!characterBaseObjects.ContainsKey(currentCharacter))
					  characterBaseObjects.Add(currentCharacter, CurrentCharacterBase.assetBundle.Load("characterbase", typeof(GameObject)));
			}

            foreach (CharacterElement c in currentConfiguration.Values)
                if (!c.IsLoaded) return false;

            return true;
        }
    }

    // Creates a character based on the currentConfiguration using a newly
    // instantiated character base.
    public GameObject Generate()
    {
        GameObject root;
		if(!bCharacterBaseCached)
		   root = (GameObject)Object.Instantiate(characterBaseRequests[currentCharacter].asset);
		else
		   root = (GameObject)Object.Instantiate(characterBaseObjects[currentCharacter]);
		
        root.name = currentCharacter;
		
		//root.AddComponent("Animation");
		
       for(int i = 0; i < AnimationClips.Count;i++)
	   {
		   	  root.animation.AddClip(AnimationClips[i],AnimationClips[i].name);
		}
		
        return Generate(root);
    }
	

    // Creates a character based on the currentConfiguration recycling a
    // character base, this way the position and animation of the character
    // are not changed.
    public GameObject Generate(GameObject root)
    {
        float startTime = Time.realtimeSinceStartup;

        // The SkinnedMeshRenderers that will make up a character will be
        // combined into one SkinnedMeshRenderers using multiple materials.
        // This will speed up rendering the resulting character.
        List<CombineInstance> combineInstances = new List<CombineInstance>();
        List<Material> materials = new List<Material>();
        List<Transform> bones = new List<Transform>();
        Transform[] transforms = root.GetComponentsInChildren<Transform>();
        
        foreach (CharacterElement element in currentConfiguration.Values)
        {
			//Debug.Log(element.name);
			
            SkinnedMeshRenderer smr = element.GetSkinnedMeshRenderer();
            materials.AddRange(smr.materials);
			InitColorWithGemIDForMaterial(element.GemID, smr.materials);		//change color via gem
            for (int sub = 0; sub < smr.sharedMesh.subMeshCount; sub++)
            {
                CombineInstance ci = new CombineInstance();
                ci.mesh = smr.sharedMesh;
                ci.subMeshIndex = sub;
                combineInstances.Add(ci);
            }

            // As the SkinnedMeshRenders are stored in assetbundles that do not
            // contain their bones (those are stored in the characterbase assetbundles)
            // we need to collect references to the bones we are using
            foreach (string bone in element.GetBoneNames())
            {
                foreach (Transform transform in transforms)
                {
					if (transform.name != bone) continue;
					
					bones.Add(transform);
					//Debug.Log("this joint named : ["+transform.name+"]");
                    break;
                }
            }

            Object.Destroy(smr.gameObject);
        }

        // Obtain and configure the SkinnedMeshRenderer attached to
        // the character base.
        SkinnedMeshRenderer r = root.GetComponent<SkinnedMeshRenderer>();
		if(r.sharedMesh)
			Object.Destroy( r.sharedMesh);
		
        r.sharedMesh = new Mesh();
        r.sharedMesh.CombineMeshes(combineInstances.ToArray(), false, false);
		//if(r.bones.Length == 0)
        r.bones = bones.ToArray();
        r.materials = materials.ToArray();
        
        Debug.Log("Generating character took: " + (Time.realtimeSinceStartup - startTime) * 1000 + " ms");
        return root;
    }

    // This method stores how much of the required assets were already downloaded
    // at the moment of the last configuration change.
    void UpdateAssetbundlesAlreadyDownloaded()
    {
        assetbundlesAlreadyDownloaded = CurrentCharacterBase.progress;
        foreach (CharacterElement e in currentConfiguration.Values)
            assetbundlesAlreadyDownloaded += e.WWW.progress;
    }

    // Returns correct assetbundle base url, whether in the editor, standalone or
    // webplayer, on Mac or Windows.
    public static string AssetbundleBaseURL
    {
        get
        {
            if (Application.platform == RuntimePlatform.WindowsWebPlayer || Application.platform == RuntimePlatform.OSXWebPlayer)
                return Application.dataPath+"/assetbundles/";
            else if (Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.OSXDashboardPlayer)
				return "file://" + Application.dataPath + "/../assetbundles/";
			else if( Application.platform == RuntimePlatform.Android)
				return "jar:file://" + Application.dataPath + "!/assets/";
			else
                return "file://" + Application.dataPath + "/../assetbundles/";
			    
        }
    }

    // Returns the WWW for retrieving the assetbundle that holds the bones and animations 
    // for currentCharacter, and creates a WWW only if one doesnt exist already. 
    WWW CurrentCharacterBase
    {
        get
        {
			
            if (!characterBaseWWWs.ContainsKey(currentCharacter))
                characterBaseWWWs.Add(currentCharacter, new WWW(AssetbundleBaseURL + currentCharacter + "_characterbase.assetbundle"));
            
			
			//if( Caching.IsVersionCached(AssetbundleBaseURL + currentCharacter + "_characterbase.assetbundle",1))
			   //  bCharacterBaseCached = true;
			//else
				// bCharacterBaseCached = false;
			
		
			//if (!characterBaseWWWs.ContainsKey(currentCharacter))
               // characterBaseWWWs.Add(currentCharacter, WWW.LoadFromCacheOrDownload(AssetbundleBaseURL + currentCharacter + "_characterbase.assetbundle",1));    
			
            return characterBaseWWWs[currentCharacter];
        }
    }

    void InitColorWithGemIDForMaterial(int gemID, Material[] mtls)
    {
        if (ArmorGemColorManager.Instance)
        {
            Color[] targetColors = new Color[2];

            for (int i = 0; i < targetColors.Length; i++)
            {
				if(gemID == 0)
				{
					targetColors[i] = ArmorGemColorManager.Instance.None[i];
					continue;
				}
				
				if(gemID == 501 || gemID == 502 || gemID == 503 || gemID == 504 || gemID == 505 || 
					gemID == 506 || gemID == 507 || gemID == 508 || gemID == 509|| gemID == 510)
				{
					targetColors[i] = ArmorGemColorManager.Instance.Ruby[i];
					continue;
				}
				
				if(gemID == 601 || gemID == 602 || gemID == 603 || gemID == 604 || gemID == 605 || 
					gemID == 606 || gemID == 607 || gemID == 608 || gemID == 609|| gemID == 610)
				{
					targetColors[i] = ArmorGemColorManager.Instance.Sapphire[i];
					continue;
				}
				
				if(gemID == 701 || gemID == 702 || gemID == 703 || gemID == 704 || gemID == 705 || 
					gemID == 706 || gemID == 707 || gemID == 708 || gemID == 709|| gemID == 710)
				{
					targetColors[i] = ArmorGemColorManager.Instance.Emerald[i];
					continue;
				}
				
				if(gemID == 801 || gemID == 802 || gemID == 803 || gemID == 804 || gemID == 805 || 
					gemID == 806 || gemID == 807 || gemID == 808 || gemID == 809|| gemID == 810)
				{
					targetColors[i] = ArmorGemColorManager.Instance.Garnet[i];
					continue;
				}
				
				if(gemID == 901 || gemID == 902 || gemID == 903 || gemID == 904 || gemID == 905 || 
					gemID == 906 || gemID == 907 || gemID == 908 || gemID == 909|| gemID == 910)
				{
					targetColors[i] = ArmorGemColorManager.Instance.Amethyst[i];
					continue;
				}
				
				if(gemID == 1001 || gemID == 1002 || gemID == 1003 || gemID == 1004 || gemID == 1005 || 
					gemID == 1006 || gemID == 1007 || gemID == 1008 || gemID == 1009|| gemID == 1010)
				{
					targetColors[i] = ArmorGemColorManager.Instance.Malachite[i];
					continue;
				}
				
				if(gemID == 1101 || gemID == 1102 || gemID == 1103 || gemID == 1104 || gemID == 1105 || 
					gemID == 1106 || gemID == 1107 || gemID == 1108 || gemID == 1109|| gemID == 1110)
				{
					targetColors[i] = ArmorGemColorManager.Instance.Obsidian[i];
					continue;
				}
				
				if(gemID == 1201 || gemID == 1202 || gemID == 1203 || gemID == 1204 || gemID == 1205 || 
					gemID == 1206 || gemID == 1207 || gemID == 1208 || gemID == 1209|| gemID == 1210)
				{
					targetColors[i] = ArmorGemColorManager.Instance.Golden[i];
					continue;
				}
				
				if(gemID == 1301 || gemID == 1302 || gemID == 1303 || gemID == 1304 || gemID == 1305 || 
					gemID == 1306 || gemID == 1307 || gemID == 1308 || gemID == 1309|| gemID == 1310)
				{
					targetColors[i] = ArmorGemColorManager.Instance.Jade[i];
					continue;
				}
            }

            for (int i = 0; i < mtls.Length; i++)
            {
				// if it's better gem, like golden or jade, use luxury shader.
                if (gemID > 1200)
                {
                    mtls[i].shader = ArmorGemColorManager.Instance.LuxuryShader;
                }
                else
                {
                    mtls[i].shader = ArmorGemColorManager.Instance.NoramlShader;
                }

                Material mtl = mtls[i];
                if (mtl.HasProperty("_TintColorR"))
                {
                    mtl.SetColor("_TintColorR", targetColors[0]);
                }
                if (mtl.HasProperty("_TintColorG"))
                {
                    mtl.SetColor("_TintColorG", targetColors[1]);
                }
                if (mtl.HasProperty("_PulsingTex"))
                {
					// only Golden gem use luxxry pusling tex02
                    if (gemID > 1200 && gemID < 1300)
                        mtl.SetTexture("_PulsingTex", ArmorGemColorManager.Instance.LuxxryPuslingTex02);
                    else
                        mtl.SetTexture("_PulsingTex", ArmorGemColorManager.Instance.LuxuryPuslingTex01);
                }
				if(mtl.HasProperty("_PulsingSpeed"))
				{
					mtl.SetFloat("_PulsingSpeed", 0.41f);
				}
				if(mtl.HasProperty("_PulsingBrightness"))
				{
					mtl.SetFloat("_PulsingBrightness", 0.55f);
				}
            }
        }
    }
}