using UnityEngine;
using System.Collections;

public class WerewolfElite : Wolf {

	public Transform weapon;
	
	Transform MyWeapon;
	
	// Use this for initialization
	public override void Start () 
	{
		base.Start();
		
		AvoidanceRadius=2f; 
		
        /*
		//attach weapon
		if(weapon)
		{
			Transform RightHand=null;
			Component[] all = transform.GetComponentsInChildren<Component>();
			foreach(Component T in all)
			{
				if(T.name == "Bip001 Prop1")
				{
					RightHand = T.transform;
					break;
				}
			}			
			
			if(RightHand)
			{
				MyWeapon = Object.Instantiate(weapon,RightHand.position,RightHand.rotation) as Transform;
				MyWeapon.parent = RightHand;
			}			
		}		
		*/
	}
	
	// Update is called once per frame
	public override void Update () 
	{
		base.Update();
	}
}
