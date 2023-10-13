using UnityEngine;
using System.Collections;

public class TreasureChest : BreakableActor {
	
	Vector3 offset=Vector3.zero;
	
	Transform glowEffect;
	float glowEffectValue = 1f;
	Color glowColor = Color.white; 
	
	//public int AppearChance = 100;
	
	public override void Start () 
	{
		base.Start();
		
		animation["GAM_TreasureChest_Open"].layer=-1;
		animation["GAM_TreasureChest_Open"].wrapMode = WrapMode.Once;
	
		//if(Random.Range(0,100) > AppearChance)
			//gameObject.SetActiveRecursively(false);
	}
	
	void Update () {
		
		if(Health == 0 && glowEffectValue > 0.001f)
		{
			if(glowEffect == null)
				return;
			
			glowEffectValue -= Time.deltaTime * 0.5f;
			glowEffectValue = Mathf.Clamp(glowEffectValue,0f,1f);
			
			Renderer[] render_list = glowEffect.GetComponentsInChildren<Renderer>();
			foreach(Renderer render in render_list)
			{
				for(int i = 0; i < render.materials.Length;i++)
				{
					Material mtl =  render.materials[i];
			    	if(mtl.HasProperty("_TintColor"))
			        {
						glowColor = mtl.GetColor("_TintColor");
						glowColor.a -= Time.deltaTime * 0.5f;
						glowColor.a = Mathf.Clamp(glowColor.a,0f,1f);
			           	mtl.SetColor("_TintColor",glowColor);
			        }
				}
			}
		}
		
	}
	
	public override void Active (int damage)
	{
		if(IsUsed) return;
		
		PlayBreakSound();
		
		animation.CrossFade("GAM_TreasureChest_Open");
		//StartCoroutine("DropItem");
		Health = 0;
		IsUsed = true;
		
		if(BreakParticle)
		{
			glowEffect = Object.Instantiate(BreakParticle) as Transform;
			glowEffect.position = transform.position;
			glowEffect.rotation = transform.rotation;
			glowEffect.localScale = transform.localScale;
		}
		
		Player.Instance.AttackTarget=null;
		
	}
	/*
	IEnumerator DropItem()
	{
		yield return new WaitForSeconds(0.5f);
		
		Transform drop1 = GetComponent<LootDrop>().GenerateRandomArmor(Player.Instance.Level);
		offset.x = -1f;
		offset.z = 1f;
		drop1.position = transform.position + offset;
		Transform drop2 = GetComponent<LootDrop>().GenerateRandomArmor(Player.Instance.Level);
		offset.x = 1f;
		offset.z = -1f;
		drop2.position = transform.position + offset;
		
		//glowEffect.gameObject.SetActiveRecursively(false);
		
		Renderer[] render_list = transform.GetComponentsInChildren<Renderer>();
		foreach(Renderer render in render_list)
		{
			for(int i = 0; i < render.materials.Length;i++)
			{
				Material mtl =  render.materials[i];
		    	if(mtl.HasProperty("_PulsingMorph"))
		        {
		           mtl.SetFloat("_PulsingMorph",1f);
		        }
			}
		}	
		
	}
	*/

}
