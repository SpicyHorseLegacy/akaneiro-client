using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;
using System.IO;

public class aPlayerIcon : MonoBehaviour {
	
	[SerializeField]
	private UISprite icon;
	public SelectElementInfo data;
	
	private int type;
	private int sex;
	
	public static string thePlayerIconName ;
	public static string thePlayerName ;
	
	// Use this for initialization
	void Start () {
		SetTypeIcon (thePlayerIconName);
		//Debug.Log ("*************///////******************>>>>>>" + thePlayerIconName);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	#region Interface
	public void SetTypeIcon(string imgName) {
		icon.spriteName = imgName;
	}
	#endregion
	
	#region Local
	private void BgDelegate() {
		SelectListManager.Instance.ChangeSelectCharaDelegate(data);
	}
	#endregion
}
