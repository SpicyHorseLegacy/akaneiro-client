using UnityEngine;
using System.Collections;

public class PlayerFirebombTrap : PlayerAbilityTrap {
	
	public override void AcitveAbilityWithMousePos (Vector3 mousePos)
	{
		base.AcitveAbilityWithMousePos (mousePos);
		Player.Instance.FSM.ChangeState(Player.Instance.IS);
	}
	
}
