using UnityEngine;
using System.Collections;

public class NPC_AreaHoT : NPCAbilityBaseState {
	public Transform HoTVFXPrefab;

	[HideInInspector] public Vector3 _vfxInstance;

    public override AbilityObject On_SkillObjectEnter(SSkillObjectEnter skillObjectInfo)
    {
        if (HoTVFXPrefab)
        {
            Transform _HoTVFXInstance = CS_Main.Instance.SpawnObject(HoTVFXPrefab, skillObjectInfo.pos + Vector3.one * 0.2f, Quaternion.identity);
            return _HoTVFXInstance.GetComponent<AbilityObject>();
        }
        return null;
    }
}
