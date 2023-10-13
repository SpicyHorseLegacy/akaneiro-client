using UnityEngine;
using System.Collections;

public class InteractiveObj : BaseHitableObject {

	public float radius = 1f;
	
	//[HideInInspector]
	public bool IsUsed = false;

    public int Health = 1;
	
	public int TypeID = 0;
	
	public bool IsShowDamageText = false;
	
	public DeathResult[] DeathResults = new DeathResult[0];

    public virtual void Active(int damage) { }

	public override void TakeDamage (int damage)
	{
		base.TakeDamage (damage);
		Active(damage);
	}

	// for interactive obj, there is a bool to control if show damage text!
	public override void TakeDamage (int damage, DamageSource source, bool isCrit, EStatusElementType elementType, bool isShowDamageText)
	{
        base.TakeDamage(damage, source, isCrit, elementType);

#if NGUI
        if(IsShowDamageText && InGameScreenCombatHudCtrl.Instance != null)
            InGameScreenCombatHudCtrl.Instance.ShowDamageAtPos(damage, transform, isCrit, elementType);
        //if(IsShowDamageText && DamageTextManager.Instance)
		//	DamageTextManager.Instance.ShowDamageText(damage, transform, isCrit, elementType);
#else
        // Please notice this "IsShowDamageText" is a public bool. Not the "isShowDamageText".
		if(IsShowDamageText && DamageTextManager.Instance)
			DamageTextManager.Instance.ShowDamageText(damage, transform, isCrit, elementType);
#endif
	}

    public override void GoToHell()
    {
        base.GoToHell();
    }
}
