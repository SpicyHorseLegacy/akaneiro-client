using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class ItemPrefabs : MonoBehaviour {
	#if UNITY_EDITOR
	// Check if there is an item in the array is not match the array ID.
    [MenuItem("Tools/Check All ItemID")]
    public static void CheckAllItemID()
    {
        
        ItemPrefabs sel = Selection.activeTransform.GetComponent<ItemPrefabs>();
        if (sel)
        {
            for (int i = 0; i < sel.itemPrefabs.Length; i++)
            {
                _UI_CS_ItemPrefabs _temp = sel.itemPrefabs[i];
                if (_temp != null)
                {
                    Debug.LogError(_temp.gameObject.name);
                    bool _foundId = false;
                    for (int j = 0; j < _temp.item.Length; j++)
                    {
                        Item _tempItem = _temp.item[j];
                        if (_tempItem != null)
                        {
                            if (!_foundId)
                            {
                                _foundId = true;
                                _temp.ItemID = _tempItem.TypeID;
								PrefabUtility.ReplacePrefab(_temp.gameObject, _temp.gameObject, ReplacePrefabOptions.ReplaceNameBased);
                            }

                            if (_tempItem.TypeID != _temp.ItemID)
                                Debug.LogError("[ItemID Check] found a item is not fit the array : " + _tempItem.name);
                        }
                    }
                }
            }
        }
    }
	#endif

	public static ItemPrefabs Instance;

	public _UI_CS_ItemPrefabs[] itemPrefabs;

    public Transform[] EnchantRagePrefab;
	public Transform[] EnchantBrutalPrefab;
	public Transform[] EnchantThirstPrefab;

    public Transform Enchant_Armor_HealthyPrefab;
    public Transform Enchant_Armor_FocusedPrefab;
    public Transform Enchant_Armor_FortifiedPrefab;
	
	public Texture2D defaultImg;
	
	public int[] WeaponIDs = new int[0];
	public int[] ArmorIDs = new int[0];
	public int[] AccessoryIDs = new int[0];
	public int[] ConsumableIDs = new int[0];
	public int[] MaterialIDs = new int[0];
	public int[] QuestIDs = new int[0];
	
	public void Awake()
	{
		Instance = this;
		
		List<int> _tempweapon = new List<int>();
		List<int> _temparmor = new List<int>();
		List<int> _tempaccessory = new List<int>();
		List<int> _tempconsumable = new List<int>();
		List<int> _tempmaterial = new List<int>();
		List<int> _tempquest = new List<int>();
		for(int i = 0; i < itemPrefabs.Length; i ++)
		{
			for(int j = 0; j < itemPrefabs[i].item.Length; j++)
			{
				if(itemPrefabs[i].item[j] != null)
				{
					if(itemPrefabs[i].item[j].GetComponent<WeaponBase>())
					{
						_tempweapon.Add(itemPrefabs[i].ItemID);
						break;
					}
					if(itemPrefabs[i].item[j].GetComponent<ArmorBase>())
					{
						_temparmor.Add(itemPrefabs[i].ItemID);
						break;
					}
					if(itemPrefabs[i].item[j].GetComponent<Accessory>())
					{
						_tempaccessory.Add(itemPrefabs[i].ItemID);
						break;
					}
					if(itemPrefabs[i].item[j].GetComponent<Item_Consumable>())
					{
						_tempconsumable.Add(itemPrefabs[i].ItemID);
						break;
					}
					if(itemPrefabs[i].item[j].GetComponent<Item_Mateiral>())
					{
						_tempmaterial.Add(itemPrefabs[i].ItemID);
						break;
					}
				}
			}
		}
		
		WeaponIDs = _tempweapon.ToArray();
		ArmorIDs = _temparmor.ToArray();
		AccessoryIDs = _tempaccessory.ToArray();
		ConsumableIDs = _tempconsumable.ToArray();
		MaterialIDs = _tempmaterial.ToArray();
	}
	
	public bool IsEquipement(int _id)
	{
		for(int i = 0 ; i< WeaponIDs.Length;i++)
		{
			if(WeaponIDs[i] == _id) return true;
		}
		for(int i = 0 ; i< ArmorIDs.Length;i++)
		{
			if(ArmorIDs[i] == _id) return true;
		}
		for(int i = 0 ; i< AccessoryIDs.Length;i++)
		{
			if(AccessoryIDs[i] == _id) return true;
		}
		return false;
	}
	
	public bool GetItemIcon(int id,int type,int idx, UIButton btn){
		
		string IconBundleName = "";
		
		if(-1 == idx){
			return false;
		}

        int prefabsIdx = 0;

        for (int i = 0; i < itemPrefabs.Length; i++)
        {
            _UI_CS_ItemPrefabs _temp = itemPrefabs[i];
            if (_temp != null && id == _temp.ItemID && _temp.item.Length > idx)
            {
                prefabsIdx = i;
            }
        }
		
		if(0 == idx){
			 btn.SetUVs(new Rect(0,0,1,1));
			 btn.SetTexture(defaultImg);
			 LogManager.Log_Warn("!!!!!!defaultImg!!!!!!");
			 return false;
		}
		
		if( prefabsIdx >= itemPrefabs.Length || prefabsIdx < 0)
		{
			return false; //Debug.LogError("Error prefabIdx is " + prefabsIdx.ToString());
		}
		else
		{
		   if(idx < 0 || idx >= itemPrefabs[prefabsIdx].item.Length)
		   {
			    return false;//Debug.LogError(" prefabIdx is " + prefabsIdx.ToString() + " error index is " + idx.ToString());
		   }
		   else
		   {
				if( itemPrefabs[prefabsIdx].item[idx] == null)
				     return false;  //Debug.LogError(" prefabIdx " + prefabsIdx.ToString() + " index " + idx.ToString() + " is empty");
		   }	 
		}
		
		if(itemPrefabs[prefabsIdx].item[idx].transform.GetComponent<ItemDownLoading>())
		{

			if(0 == _PlayerData.Instance.CharactorInfo.sex.Get()){
				IconBundleName = itemPrefabs[prefabsIdx].item[idx].transform.GetComponent<ItemDownLoading>().BoyIconBundle;
				if(ItemDownLoading.CachedObjectMap.ContainsKey(IconBundleName))
				{
					if(ItemDownLoading.CachedObjectMap[IconBundleName].ItemObject != null)
					{
						btn.SetUVs(new Rect(0,0,1,1));
					    btn.SetTexture((Texture2D)ItemDownLoading.CachedObjectMap[IconBundleName].ItemObject);
					    return true;
					}
				}
				
			}else{
				IconBundleName = itemPrefabs[prefabsIdx].item[idx].transform.GetComponent<ItemDownLoading>().GirlIconBundle;
				if(ItemDownLoading.CachedObjectMap.ContainsKey(IconBundleName))
				{
					if(ItemDownLoading.CachedObjectMap[IconBundleName].ItemObject != null)
					{
					   btn.SetUVs(new Rect(0,0,1,1));
					   btn.SetTexture((Texture2D)ItemDownLoading.CachedObjectMap[IconBundleName].ItemObject);
					   return true;
					}
				}
		
			}
			
		}
		else
		{
			if(0 == _PlayerData.Instance.CharactorInfo.sex.Get())
			{
				if(itemPrefabs[prefabsIdx].item[idx].Normal_State_IconBoy != null){
				  btn.SetUVs(new Rect(0,0,1,1));
				  btn.SetTexture(itemPrefabs[prefabsIdx].item[idx].Normal_State_IconBoy);
				}
				else{
				  LogManager.Log_Error("Item download err,Normal_State_IconBoy == null");
				  btn.SetUVs(new Rect(0,0,1,1));
				  btn.SetTexture(defaultImg);	
				}
				return true;
			}
			else
			{
				if(itemPrefabs[prefabsIdx].item[idx].Normal_State_IconGirl != null){
				  btn.SetUVs(new Rect(0,0,1,1));
			      btn.SetTexture(itemPrefabs[prefabsIdx].item[idx].Normal_State_IconGirl);
				}
				else{
				   LogManager.Log_Error("Item download err,Normal_State_IconGirl == null");
				  btn.SetUVs(new Rect(0,0,1,1));
				  btn.SetTexture(defaultImg);
				}
				return true;
			}
			
		}
		
		if(IconBundleName.Length == 0)
		{
			btn.SetTexture(defaultImg);
			return true;
		}
		
		//StartCoroutine
		 StartCoroutine(LoadingItemIcon(IconBundleName,prefabsIdx,idx,btn));

		 return true;
		
	}
	
	public bool GetItemIcon(int id,int type,int idx, UITexture texture){
		
		string IconBundleName = "";
		bool bEnable = texture.enabled;
		texture.enabled = false;
		
		if(-1 == idx){
			return false;
		}

        int prefabsIdx = 0;

        for (int i = 0; i < itemPrefabs.Length; i++)
        {
            _UI_CS_ItemPrefabs _temp = itemPrefabs[i];
            if (_temp != null && id == _temp.ItemID && _temp.item.Length > idx)
            {
                prefabsIdx = i;
            }
        }
		
		if(0 == idx){
			//texture.mainTexture = defaultImg;
			//texture.enabled = true;
			//LogManager.Log_Warn("!!!!!!defaultImg!!!!!!");
			//return false;
		}
		
		if( prefabsIdx >= itemPrefabs.Length || prefabsIdx < 0)
		{
			return false; //Debug.LogError("Error prefabIdx is " + prefabsIdx.ToString());
		}
		else
		{
		   if(idx < 0 || idx >= itemPrefabs[prefabsIdx].item.Length)
		   {
			    return false;//Debug.LogError(" prefabIdx is " + prefabsIdx.ToString() + " error index is " + idx.ToString());
		   }
		   else
		   {
				if( itemPrefabs[prefabsIdx].item[idx] == null)
				     return false;  //Debug.LogError(" prefabIdx " + prefabsIdx.ToString() + " index " + idx.ToString() + " is empty");
		   }	 
		}
		
		if(itemPrefabs[prefabsIdx].item[idx].transform.GetComponent<ItemDownLoading>())
		{
			if(0 == PlayerDataManager.Instance.GetPlayerInfoBase().sex.Get()){	
				IconBundleName = itemPrefabs[prefabsIdx].item[idx].transform.GetComponent<ItemDownLoading>().BoyIconBundle;
				if(ItemDownLoading.CachedObjectMap.ContainsKey(IconBundleName))
				{
					if(ItemDownLoading.CachedObjectMap[IconBundleName].ItemObject != null)
					{
						texture.mainTexture = (Texture2D)ItemDownLoading.CachedObjectMap[IconBundleName].ItemObject;
						texture.enabled = true;
					    return true;
					}
				}
			}else{
				IconBundleName = itemPrefabs[prefabsIdx].item[idx].transform.GetComponent<ItemDownLoading>().GirlIconBundle;
				if(ItemDownLoading.CachedObjectMap.ContainsKey(IconBundleName))
				{
					if(ItemDownLoading.CachedObjectMap[IconBundleName].ItemObject != null)
					{
						texture.mainTexture = (Texture2D)ItemDownLoading.CachedObjectMap[IconBundleName].ItemObject;
						texture.enabled = true;
						return true;
					}
				}
		
			}
			
		}
		else
		{
			
			if(0 == PlayerDataManager.Instance.GetPlayerInfoBase().sex.Get())
			{
				if(itemPrefabs[prefabsIdx].item[idx].Normal_State_IconBoy != null){
					texture.mainTexture =itemPrefabs[prefabsIdx].item[idx].Normal_State_IconBoy;
					texture.enabled = true;
				}
				else{
					LogManager.Log_Error("Item download err,Normal_State_IconBoy == null");	
					texture.mainTexture =defaultImg;
					texture.enabled = true;
				}
				return true;
			}
			else
			{
				if(itemPrefabs[prefabsIdx].item[idx].Normal_State_IconGirl != null){
					texture.mainTexture =itemPrefabs[prefabsIdx].item[idx].Normal_State_IconGirl;
					texture.enabled = true;
				}
				else{
					LogManager.Log_Error("Item download err,Normal_State_IconGirl == null");
					texture.mainTexture =defaultImg;
					texture.enabled = true;
				}
				return true;
			}
			
		}
		
		if(IconBundleName.Length == 0)
		{
			texture.mainTexture =defaultImg;
			return true;
		}
		
		//wtf//
		//StartCoroutine
		 StartCoroutine(LoadingItemIcon(IconBundleName,prefabsIdx,idx,texture));

		 return true;
		
	}

    private IEnumerator LoadingItemIcon(string iconBundleName, int prefabsIdx, int idx, UITexture img)
    {
        img.enabled = false;

        if (!ItemDownLoading.CachedObjectMap.ContainsKey(iconBundleName))
        {
            WWW database;
            string url = "";
            url = BundlePath.AssetbundleBaseURL;
            url += (iconBundleName + ".assetbundle");
            ItemDownLoading.ItemDownLoadData newItemData = new ItemDownLoading.ItemDownLoadData();
            newItemData.bHasDownloaded = false;
            newItemData.BundleString = iconBundleName;
            newItemData.ItemObject = null;
            newItemData.AddANewUITexture(img);

            database = new WWW(url);

            ItemDownLoading.CachedObjectMap.Add(iconBundleName, newItemData);

            yield return database;

            if (database.assetBundle.mainAsset != null)
            {
                ItemDownLoading.CachedObjectMap[iconBundleName].ItemObject = database.assetBundle.mainAsset;
                ItemDownLoading.CachedObjectMap[iconBundleName].bHasDownloaded = true;
            }

            yield return null;
        }

        if (ItemDownLoading.CachedObjectMap.ContainsKey(iconBundleName))
        {
            if (ItemDownLoading.CachedObjectMap[iconBundleName].bHasDownloaded == false)
            {
                ItemDownLoading.CachedObjectMap[iconBundleName].AddANewUITexture(img);
                img.mainTexture = defaultImg;
                yield return null;
            }

            if (ItemDownLoading.CachedObjectMap[iconBundleName].bHasDownloaded)
            {
                if (ItemDownLoading.CachedObjectMap[iconBundleName].ItemObject == null)
                {
                    img.mainTexture = defaultImg;
                }
                else
                    img.mainTexture = (Texture2D)ItemDownLoading.CachedObjectMap[iconBundleName].ItemObject;
            }
        }
        else
        {
            img.mainTexture = defaultImg;
        }

        img.enabled = true;
    }
	
	private IEnumerator LoadingItemIcon(string iconBundleName,int prefabsIdx,int idx,UIButton btn)
	{
		WWW database;
		string url = "";

        url = BundlePath.AssetbundleBaseURL;
	    url += (iconBundleName +".assetbundle");
		
		if(!ItemDownLoading.CachedObjectMap.ContainsKey(iconBundleName))
		{
		   //Debug.Log(iconBundleName + " item try to be downloaded");
			
		   ItemDownLoading.ItemDownLoadData newItemData =  new ItemDownLoading.ItemDownLoadData();			
		   newItemData.bHasDownloaded = false;			
	 	   newItemData.BundleString = iconBundleName;			
		   newItemData.ItemObject = null;
					
		   database = new WWW( url);
			
		   newItemData.CachedItemWWW = database; 
		   newItemData.mClickedButtons.Add(btn);
			
		   ItemDownLoading.CachedObjectMap.Add(iconBundleName,newItemData);
	
		   yield return database;
			
		   if(database.assetBundle.mainAsset != null)
		   {
			  ItemDownLoading.CachedObjectMap[iconBundleName].bHasDownloaded = true;
			  ItemDownLoading.CachedObjectMap[iconBundleName].ItemObject = database.assetBundle.mainAsset;
			  if(ItemDownLoading.CachedObjectMap[iconBundleName].ItemObject != null)
				btn.SetTexture((Texture2D)ItemDownLoading.CachedObjectMap[iconBundleName].ItemObject);	
		   }
		  
		}
	
		if(ItemDownLoading.CachedObjectMap.ContainsKey(iconBundleName))
		{
			if(ItemDownLoading.CachedObjectMap[iconBundleName].bHasDownloaded == false)
			{
				if(!ItemDownLoading.CachedObjectMap[iconBundleName].mClickedButtons.Contains(btn))
					ItemDownLoading.CachedObjectMap[iconBundleName].mClickedButtons.Add(btn);
				
				btn.SetTexture(defaultImg);
				yield return null;
			}
			
			if(ItemDownLoading.CachedObjectMap[iconBundleName].bHasDownloaded)
			{
			  // Debug.Log(iconBundleName + " item succeed to be downloaded");
			   if(ItemDownLoading.CachedObjectMap[iconBundleName].ItemObject == null)
				  btn.SetTexture(defaultImg);
			   else
				  btn.SetTexture((Texture2D)ItemDownLoading.CachedObjectMap[iconBundleName].ItemObject);
			}
		}
		else
		{
			btn.SetTexture(defaultImg);
		}

	}
	
	public Transform GetItemPrefab(int id,int type,int _prefabid){

        //Debug.LogError("Finding prefab : " + id + " || prefabid : " + _prefabid);

        if (-1 == _prefabid)
        {
			return null;
		}

        for (int i = 0; i < itemPrefabs.Length; i++)
        {
            _UI_CS_ItemPrefabs _temp = itemPrefabs[i];
            if (_temp != null && id == _temp.ItemID && _temp.item.Length > _prefabid)
            {
                return _temp.item[_prefabid].transform;
            }
        }

        Debug.LogError("[ITEM] Didn't find item prefab! [ID]" + id + " || [PrefabID]" + _prefabid);
        return null;
	}
	
	public int ItemIdToIdx(int id){
        
		return 0;
	}
	
}
