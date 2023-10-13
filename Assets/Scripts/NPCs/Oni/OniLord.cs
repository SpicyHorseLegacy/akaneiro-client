using UnityEngine;
using System.Collections;

public class OniLord : NpcBase {
	
	Renderer ClubTrailRenderer=null;
	Renderer SwordTrailRender=null;
	Transform OnilordTellSound = null;
	Transform OniWhooshSound = null;
	Transform OniDropSwordSound = null;
	Transform OniDeathKneeSound = null;
	Transform OniDeathBodySound = null;
	
	// Use this for initialization
	public override void Start () 
	{
		base.Start();
		
		AvoidanceRadius=2f; 
		
		PlayDamageAnimation = false;
	}
	
	// Update is called once per frame
	public override void Update () 
	{
		base.Update();
	}
	
	public override void PlayAttackAnim ()
	{
		SetWeaponTrailEffect(true);
		Transform[] Sounds = GetComponent<NpcSoundEffect>().MiscSoundPrefabs;
		if( Sounds != null && Sounds.Length > 0)                              
		   OnilordTellSound = PlaySound(GetComponent<NpcSoundEffect>().MiscSoundPrefabs[0],OnilordTellSound);

		base.PlayAttackAnim();
	}
	
	public override void PlayIdleAnim ()
	{
		SetWeaponTrailEffect(false);
		
		base.PlayIdleAnim ();
	}
	
	public void SetWeaponTrailEffect(bool enable)
	{
		if(ClubTrailRenderer==null || SwordTrailRender==null)
		{
			Component[] all = transform.GetComponentsInChildren<Component>();
			int find = 0;
			foreach(Component T in all)
			{
				if(T.name == "ClubTrail")
				{
					ClubTrailRenderer = T.renderer;
					find++;
				}
				if(T.name == "SwordTrail")
				{
					SwordTrailRender = T.renderer;
					find++;
				}
				
				if(find==2)
					break;
			}
		}
		
		if(ClubTrailRenderer!=null)
			ClubTrailRenderer.enabled = enable;

		if(SwordTrailRender!=null)
			SwordTrailRender.enabled = enable;
	}
	
	public void PlayWhooshSound()
	{
		Transform[] Sounds = GetComponent<NpcSoundEffect>().MiscSoundPrefabs;
		if( Sounds != null && Sounds.Length > 1)                              
		   OniWhooshSound = PlaySound(GetComponent<NpcSoundEffect>().MiscSoundPrefabs[1],OniWhooshSound);
		
	}
	
	public void PlayDropSwordSound()
	{
		Transform[] Sounds = GetComponent<NpcSoundEffect>().MiscSoundPrefabs;
		if( Sounds != null && Sounds.Length > 2)                              
		   OniDropSwordSound = PlaySound(GetComponent<NpcSoundEffect>().MiscSoundPrefabs[2],OniDropSwordSound);
		
	}
	
    public void PlayDeathKneeSound()
	{
		Transform[] Sounds = GetComponent<NpcSoundEffect>().MiscSoundPrefabs;
		if( Sounds != null && Sounds.Length > 3)                              
		   OniDeathKneeSound = PlaySound(GetComponent<NpcSoundEffect>().MiscSoundPrefabs[3],OniDeathKneeSound);
		
	}
}
