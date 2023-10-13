using UnityEngine;
using System.Collections;

public class EquipmentTip : TooltipObj {

	public override TooltipType GetType(){
		return TooltipType.Equipments;
	}
	public override DataBase GetData(){
		return data;
	}
	public override void SetData(DataBase d){
		data = d;
	}
}
