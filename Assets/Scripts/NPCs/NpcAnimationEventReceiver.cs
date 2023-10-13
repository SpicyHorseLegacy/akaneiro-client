using UnityEngine;
using System.Collections;

public class NpcAnimationEventReceiver : MonoBehaviour {
	
	public NpcBase Owner;
	
	public void AttackPlayer( int side )
	{
		// side to active impact sound from weapon which side
		Owner.AttackPlayer(side);
	}
	
	public void PlayWhooshSound()
	{
		Owner.SendMessage("PlayWhooshSound",SendMessageOptions.DontRequireReceiver);
	}
	
	public void PlayBodyFallSound()
	{
		Owner.PlayBodyFallSound();
	}
	
	public void PlayDeathKneeSound()
	{
		Owner.SendMessage("PlayDeathKneeSound",SendMessageOptions.DontRequireReceiver);
	}
	
	public void PlayDropSwordSound()
	{
		Owner.SendMessage("PlayDropSwordSound",SendMessageOptions.DontRequireReceiver);
	}
	
	public void PlayShockWaveVFX()
	{
		//Owner.SendMessage("PlayShockWaveVFX",SendMessageOptions.DontRequireReceiver);
	}
	
	public void Throw()
	{
		Owner.AttackPlayer(0);
		// for werewolf ranged
		Owner.SendMessage("Throw",SendMessageOptions.DontRequireReceiver);
	}
	
	public void WalkSound()
	{
		Owner.SendMessage("PlayWalkSound",SendMessageOptions.DontRequireReceiver);
	}
	
	public void StartChargeEffect()
	{
		// for werewolf ranged
		Owner.SendMessage("StartChargeEffect",SendMessageOptions.DontRequireReceiver);
	}

    public void AttackNotify() { }

    public void WhirlWindEnd()
    {
        if (Owner.abilityManager)
        {
            NPC_WhirlWind _whirlwindAbi = (NPC_WhirlWind)Owner.abilityManager.GetAbilityByID(30021);
            _whirlwindAbi.StopVFX();
        }
    }
}
