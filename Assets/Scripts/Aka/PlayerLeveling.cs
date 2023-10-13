using UnityEngine;
using System.Collections;

public class PlayerLeveling : MonoBehaviour 
{
	static public PlayerLeveling Instance;
	
	public int[] AbilitySlots = {1,1,1,2,2,2,3,3,3,4,4,4,5,5,5,6,6,6,6,6};
	public int[] PassiveSlots = {0,0,1,1,1,1,1,1,1,1,1,1,2,2,2,2,2,2,2,3};
	public int[] ComboSlots   = {0,1,1,1,2,2,2,3,3,3,4,4,4,5,5,5,5,5,5,5};
	public int[] BaseHitpoints = {100,120,140,160,180,200,220,240,260,280,300,320,340,360,380,400,420,440,460,500};
    //int[] KarmaToNextLevel = {0,100,100,200,300,600,600,600,600,900,900,900,900,900,900,1200,1200,1200,1200,1200};
	public int[] KarmaTotalRequired = {0,100,200,400,700,1300,1900,2500,3100,4000,4900,5800,6700,7600,8500,9700,10900,12100,13300,14500};
	
	// Use this for initialization
	void Start () 
	{
		Instance = this;
	}
	
	public int curAbilitySlots
	{
		get 
		{
			return AbilitySlots[Player.Instance.Level-1];
		}
	}
	
	public int curPassiveSlots
	{
		get
		{
			return PassiveSlots[Player.Instance.Level-1];
		}
	}
	
	public int curComboSlots
	{
		get
		{
			return ComboSlots[Player.Instance.Level-1];
		}
	}
	
	public int curBaseHitPoints
	{
		get
		{
			return BaseHitpoints[Player.Instance.Level-1];
		}
	}
	
	public int KarmaRequiredToNext
	{
		get
		{
			//return KarmaToNextLevel[Player.Instance.Level];
			return KarmaRequiredTotaltoNext - KarmaRequiredTotal;
		}
	}	
	
	public int KarmaRequiredTotal
	{
		get
		{
			return KarmaTotalRequired[Player.Instance.Level-1];
		}
	}
	
	public int KarmaRequiredTotaltoNext
	{
		get
		{
			int nextLevel = Player.Instance.Level;
		    if(nextLevel >= 20)
			    nextLevel -= 1;
			
		    return KarmaTotalRequired[nextLevel];
		}
	
	}
}
