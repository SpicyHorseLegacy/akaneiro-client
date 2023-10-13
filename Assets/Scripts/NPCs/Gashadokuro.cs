using UnityEngine;
using System.Collections;

public class Gashadokuro : NpcBase {
	
	public Transform weapon;
	
	Transform MyWeapon;
	
	// Use this for initialization
	public override void Start () 
	{
		base.Start();

		AvoidanceRadius=3f; 
		
//		//attach weapon
//		if(weapon)
//		{
//			Transform RightHand=null;
//			Component[] all = transform.GetComponentsInChildren<Component>();
//			foreach(Component T in all)
//			{
//				if(T.name == "Bone_WP")
//				{
//					RightHand = T.transform;
//					break;
//				}
//			}			
//			
//			if(RightHand)
//			{
//				MyWeapon = Object.Instantiate(weapon,RightHand.position,RightHand.rotation) as Transform;
//				MyWeapon.parent = RightHand;
//			}			
//		}		
	}
	
	// Update is called once per frame
	public override void Update () 
	{
		base.Update();
	}
}
