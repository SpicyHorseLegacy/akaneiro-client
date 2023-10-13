using UnityEngine;
using System.Collections;

public class MaterialTip : TooltipObj {

	public override TooltipType GetType(){
		return TooltipType.Materials;
	}
	public override DataBase GetData(){
		return data;
	}
	public override void SetData(DataBase d){
		data = d;
	}
}
