using UnityEngine;
using System.Collections;

public class AbilitiesData : DataBase{
	public Color abilitiesColor;
	public string name;
	public string info;
	public string other;
}

public class AbilitiesTipInfo : TooltipInfoBase {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	#region Interface
	public void SetAbilitiesTip(AbilitiesData data){
		SetBackground(data.abilitiesColor);
		SetName(data.abilitiesColor,data.name);
		SetInfo(data.info);
		SetOther(data.other);
	}
	#endregion
	
	#region Local
	[SerializeField]
	private UISlicedSprite background;
	private void SetBackground(Color c){
		background.color = c;
	}
	
	[SerializeField]
	private UILabel abilitiesName;
	private void SetName(Color c,string name){
		abilitiesName.text =name;
		abilitiesName.color = c;
	}
		
	[SerializeField]
	private UILabel abilitiesInfo;
	private void SetInfo(string info){
		abilitiesInfo.text = info;
	}
	
	[SerializeField]
	private UILabel abilitiesOther;
	private void SetOther(string other){
		abilitiesOther.text = other;
	}
	#endregion
	
	#region override
	public override void Show(Vector3 v,DataBase data){
		SetAbilitiesTip((AbilitiesData)data);
        gameObject.transform.position = v; 
		
	}
	
	public override void Hide(){
		gameObject.transform.position = new Vector3(999f,999f,-2f); 
	}
	#endregion
}
