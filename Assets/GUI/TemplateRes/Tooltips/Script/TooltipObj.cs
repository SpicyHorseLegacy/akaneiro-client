using UnityEngine;
using System.Collections;

public enum TooltipType{
	Materials = 0,
	Equipments,
	Abilities,
	Max,
};

public abstract class TooltipObj : MonoBehaviour {
	
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public DataBase  data;
	#region Interface
	public abstract TooltipType GetType();
	public abstract DataBase GetData();
	public abstract void SetData(DataBase d);
	#endregion
}
