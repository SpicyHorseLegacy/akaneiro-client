using UnityEngine;
using System.Collections;

public class UI_Hud_DragItem_Ability : UI_Hud_BaseDragItem {

	public AbilityDetailInfo.EnumAbilityType AbilityType;
	
	void Start()
	{
		if(UI_Hud_AbilitySlot_Manager.Instance)
			UI_Hud_AbilitySlot_Manager.Instance.HighLightAllSlots();
	}
	
	protected override void Update ()
	{
		if(UI_Hud_AbilitySlot_Manager.Instance)
			UI_Hud_AbilitySlot_Manager.Instance.CheckIfInAnySlot(this, Input.mousePosition);
		
		base.Update ();
	}
	
	protected override void DragFinished ()
	{
		if(UI_Hud_AbilitySlot_Manager.Instance)
			UI_Hud_AbilitySlot_Manager.Instance.CheckEquipSlot(this, Input.mousePosition);
		
		if(UI_AbiInfo_Manager.Instance)
			UI_AbiInfo_Manager.Instance.ExitDragging();
		
		base.DragFinished ();
	}
}
