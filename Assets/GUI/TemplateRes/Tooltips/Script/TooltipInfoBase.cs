using UnityEngine;
using System.Collections;

public abstract class TooltipInfoBase : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	#region Interface
	public abstract void Show(Vector3 v,DataBase data);
	
	public abstract void Hide();
	#endregion
}

public abstract class DataBase{
	
}