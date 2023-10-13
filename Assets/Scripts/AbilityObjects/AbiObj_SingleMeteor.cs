using UnityEngine;
using System.Collections;

public class AbiObj_SingleMeteor : AbilityObject {
	
	public Transform MeteorObjectPrefab;
	
	public override void Prepare ()
	{
		base.Prepare ();
		
		float _area = AbilityInfo.Instance.AbilityObjectInfomation.GetObjectInfoByID(SkillObjectInfo.objectTypeID).Param;
		if(PrepareVFXPrefabInstance && PrepareVFXPrefabInstance.GetComponent<MeteorRangeController>())
		{
	    	PrepareVFXPrefabInstance.GetComponent<MeteorRangeController>().ResizeCircle(_area);
		}
	}
	
	public override void Active ()
	{
		base.Active ();
		
		Vector3 dropPoint = transform.position;
		Vector3 startPoint = dropPoint + new Vector3(1,10,0);
		Transform rock = Instantiate(MeteorObjectPrefab,startPoint,MeteorObjectPrefab.transform.rotation) as Transform;
		rock.GetComponent<MeteorRock>().GoTo(dropPoint);
	}
	
}
