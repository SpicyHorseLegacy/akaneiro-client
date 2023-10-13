using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MeteorRainObj : AbilityObject {
	public GameObject RockPrefab;
	public float Interval;
	bool isActived = false;
	float Area;

	private float nextRockTime;
	
	void Update () {
		if(isActived)
		{
			nextRockTime -= Time.deltaTime;
			if(nextRockTime < 0){
				createARock();
				nextRockTime = Interval;
			}
		}
	}
	
	public override void Prepare ()
	{
		base.Prepare ();
		
		Area = AbilityInfo.Instance.AbilityObjectInfomation.GetObjectInfoByID(SkillObjectInfo.objectTypeID).Param;
		if(PrepareVFXPrefabInstance && PrepareVFXPrefabInstance.GetComponent<MeteorRangeController>())
		{
	    	PrepareVFXPrefabInstance.GetComponent<MeteorRangeController>().ResizeCircle(Area);
		}
	}
	
	public override void Active ()
	{
		base.Active ();
		
		isActived = true;
		
		if (CameraEffectManager.Instance)
        	CameraEffectManager.Instance.PlayMeteorShakingEffect();
	}
	
	private void createARock()
	{
		float tempRadiu = Random.Range(0,Area);
		float x = Random.Range(-tempRadiu,tempRadiu);
		tempRadiu -= Mathf.Abs(x);
		float z = Random.Range(-tempRadiu,tempRadiu);
		
		Vector3 dropPoint = transform.position + new Vector3(x,0,z);
		Vector3 startPoint = dropPoint + new Vector3(1,10,0);
		
		GameObject rock = Instantiate(RockPrefab,startPoint,RockPrefab.transform.rotation) as GameObject;
		rock.GetComponent<MeteorRock>().GoTo(dropPoint);
		
	}
}
