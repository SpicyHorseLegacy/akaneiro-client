using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class CS_Main : MonoBehaviour 
{
	public static CS_Main Instance = null;
	public CommModule g_commModule = new CommModule();
	
	[HideInInspector]
	public vectorAttrChange PlayerAttributes = null;

    
    void Awake()
	{
		DontDestroyOnLoad(gameObject);
		Instance = this;
	}

	// Use this for initialization
	void Start ()
	{
	}
	
	// Update is called once per frame
	void Update ()
	{
		g_commModule.Update();

	}
	
    void  OnLevelWasLoaded (int iLevel) 
	{
		if(1 != iLevel && 2 != iLevel){
		
			CS_Main.Instance.g_commModule.SendMessage(ProtocolBattle_SendRequest.MapLoadOK());
			//Debug.LogError("MapLoadOK");
		}
	}
	
	public void SetUserCharactor(SCharacterInfoBasic characterData, vectorAttrChange attrVec)
	{
		if(PlayerAttributes == null)
		{
			PlayerAttributes = new vectorAttrChange();
		}
		
		for(int i=0; i<attrVec.Count;i++)
		{
			SAttributeChange attri = new SAttributeChange();
			attri = (SAttributeChange)attrVec[i];
			PlayerAttributes.Add(attri);
		}
	}
	
	public Transform SpawnObject(Transform prefab)
	{
		Transform obj = null;
		
		if(prefab != null)
		{
			obj = UnityEngine.Object.Instantiate(prefab) as Transform;
		}
		
		return obj;
	}
	
	public Transform SpawnObject(Transform prefab, Vector3 pos, Quaternion rotation)
	{
		Transform temp = SpawnObject(prefab);
		if(temp)
		{
			temp.position = pos;
			temp.rotation = rotation;
		}
		return temp;
	}
	
}


  
