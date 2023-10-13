using UnityEngine;
using System.Collections;

public class AbilitiesTip : TooltipObj {

	public override TooltipType GetType(){
		return TooltipType.Abilities;
	}
	public override DataBase GetData(){
		return data;
	}
	public override void SetData(DataBase d){
		data = d;
	}
}
