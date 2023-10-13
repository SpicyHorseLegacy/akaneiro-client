using UnityEngine;
using System.Collections;


public class MaterialData : DataBase{
	public string name;
	public string info;
}
public class MaterialTipInfo : TooltipInfoBase {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	#region Interface
	public  void setMaterialData(MaterialData data){
		setTitleName(data.name);
		setMaterialInfo(data.info);
	}
	
	[SerializeField]
	private UILabel titleName;
	private void setTitleName(string name){
		titleName.text = name;
	}
	
	[SerializeField]
	private UILabel materialInfo;
	private void setMaterialInfo(string text){
		materialInfo.text = text;
	}
	#endregion
	
	#region override
	public override void Show(Vector3 v,DataBase data){
		setMaterialData((MaterialData)data);
        gameObject.transform.position = v; 
	}
	
	public override void Hide(){
		gameObject.transform.position = new Vector3(999f,999f,-2f); 
	}
	#endregion
}
