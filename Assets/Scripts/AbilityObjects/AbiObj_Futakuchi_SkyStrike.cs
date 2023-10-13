using UnityEngine;
using System.Collections;

public class AbiObj_Futakuchi_SkyStrike : AbilityObject {

	public float PrepareTime = 2;
	float delaytime;
	
	void Update()
	{
		delaytime -= Time.deltaTime;
		if(delaytime < PrepareTime)
			_prepare();
	}
	
	public override void Prepare()
    {
		if(AbilityInfo.Instance && AbilityInfo.GetAbilityObjectInfomationByID(TypeID) != null)
			delaytime = AbilityInfo.GetAbilityObjectInfomationByID(TypeID).DelayTime;
    }
	
	void _prepare()
	{
		if (PrepareSoundPrefab)
        {
            SoundCue.PlayPrefabAndDestroy(PrepareSoundPrefab, transform.position);
        }

        if (!PrepareVFXPrefabInstance && PrepareVFXPrefab)
        {
            PrepareVFXPrefabInstance = Instantiate(PrepareVFXPrefab, transform.position, transform.rotation) as Transform;
            PrepareVFXPrefabInstance.parent = transform;
        }
	}
}
