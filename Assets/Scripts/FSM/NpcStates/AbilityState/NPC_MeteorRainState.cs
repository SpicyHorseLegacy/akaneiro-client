using UnityEngine;
using System.Collections;

public class NPC_MeteorRainState : NPCAbilityBaseState {
	public Transform MeteorRainPrefab;

	[HideInInspector] public Vector3 meteorPoint;
    Transform _rangeObj;

    public override void UseAbilityOK(SUseSkillResult useSkillResult)
    {
        base.UseAbilityOK(useSkillResult);

        /*
        if (_rangeObj)
            DestructAfterTime.DestructGameObjectNow(_rangeObj.gameObject);

        if (MeteorRainPrefab && MeteorRainPrefab.GetComponent<MeteorRainObj>())
        {
            Transform _meteorVFXPrefab = MeteorRainPrefab.GetComponent<MeteorRainObj>().MeteorVFXPrefab;
            if (_meteorVFXPrefab)
            {
                _rangeObj = Instantiate(_meteorVFXPrefab, transform.position, transform.rotation) as Transform;
                //_rangeObj.parent = transform;
                _rangeObj.position = CS_SceneInfo.pointOnTheGround(useSkillResult.pos) + Vector3.up * 0.2f; 
                if (_rangeObj.GetComponent<MeteorRangeController>())
                {
                    float size = AbilityInfo.Instance.AbilityObjectInfomation.GetObjectInfoByID(Info.ObjectID).Param;
                    _rangeObj.GetComponent<MeteorRangeController>().ResizeCircle(size);
                }
            }
        }
        */
    }
	
	public override AbilityObject On_SkillObjectEnter (SSkillObjectEnter skillObjectInfo)
	{
        if (MeteorRainPrefab)
        {
            Transform meteorRain = CS_Main.Instance.SpawnObject(MeteorRainPrefab, skillObjectInfo.pos + Vector3.one * 0.2f, Quaternion.identity);
            MeteorRainObj mrobj = meteorRain.GetComponent<MeteorRainObj>();
            mrobj.ObjID = skillObjectInfo.objectID;
            mrobj.DestAbility = this;
            mrobj.SkillObjectInfo = skillObjectInfo;
            return mrobj;
        }
        return null;
	}
}
