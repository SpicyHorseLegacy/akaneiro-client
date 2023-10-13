using UnityEngine;
using System.Collections;

public class AbilityObject : BaseObject {
	[HideInInspector] public int TypeID;

    [HideInInspector] public AbilityBaseState DestAbility;            // which ability make this object
    public SSkillObjectEnter SkillObjectInfo;       // object info from server

    // when get enter callback from server, some object should be shown on screen to help player avoid. Like
    public Transform PrepareSoundPrefab;
    public Transform PrepareVFXPrefab;
	protected Transform PrepareVFXPrefabInstance;
	
	public Transform ActiveSoundPrefab;
	public Transform ActiveVFXPrefab;
	protected Transform ActiveVFXPrefabInstance;

	public virtual void Init(){}

    public virtual void Prepare()
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

    public virtual void Active()
    {
		if (ActiveSoundPrefab)
        {
            SoundCue.PlayPrefabAndDestroy(ActiveSoundPrefab, transform.position);
        }

        if (!ActiveVFXPrefabInstance && ActiveVFXPrefab)
        {
            ActiveVFXPrefabInstance = Instantiate(ActiveVFXPrefab, transform.position, transform.rotation) as Transform;
            ActiveVFXPrefabInstance.parent = transform;
        }
    }
}
