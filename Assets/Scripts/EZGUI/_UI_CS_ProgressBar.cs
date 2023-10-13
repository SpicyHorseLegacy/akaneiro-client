using UnityEngine;
using System.Collections;

public class _UI_CS_ProgressBar : MonoBehaviour {
	public UIProgressBar PlayerHPBarE;
	public UIProgressBar PlayerEnBarE;
	public UIProgressBar PlayerHPBar;
	public UIProgressBar PlayerEnBar;
	
	public  float effectDelayHp 	= 5;
	private float StartTimeDelayHp  = 0;
	public  float effectDelayEn 	= 5;
	private float StartTimeDelayEn  = 0;
	
	private bool isStartCalcTimeHp  = true;
	private bool isStartCalcTimeEn  = true;
	
	private float effectSpeedHp 	= 1f;
	private float effectSpeedEn 	= 1f;
	private float currentHp 		= 0;
	private float currentEn 		= 0;
	private float currentHpE 		= 0;
	private float currentEnE 		= 0;
	public int 	  barAniSpeedVal	= 5;
	
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(null != Player.Instance){
			UpdateProgressBarEffect();
		}
	}
	
	public void UpdateProgressBarEffect(){
        float playerhp = Player.Instance.AttrMan.Attrs[EAttributeType.ATTR_CurHP];
        float playeren = Player.Instance.AttrMan.Attrs[EAttributeType.ATTR_CurMP];
		effectSpeedHp  = Player.Instance.AttrMan.Attrs[EAttributeType.ATTR_MaxHP]/(100/barAniSpeedVal);
		effectSpeedEn  = Player.Instance.AttrMan.Attrs[EAttributeType.ATTR_MaxMP]/(100/barAniSpeedVal);
		//player hp
//		if(currentHp != playerhp){
//			if(playerhp < currentHp){
//				currentHp -= effectSpeedHp;
//				if(currentHp < playerhp){
//					currentHp = playerhp;
//				}
//			}else{
//				currentHp += effectSpeedHp;
//				if(currentHp > playerhp){
//					currentHp = playerhp;
//				}
//			}
//		}
//		
//		//player en
//		if(currentEn != playeren){
//			if(playeren < currentEn){
//				currentEn -= effectSpeedEn;
//				if(currentEn < playeren){
//					currentEn = playeren;
//				}
//			}else{
//				currentEn += effectSpeedEn;
//				if(currentEn > playeren){
//					currentEn = playeren;
//				}
//			}
//		}
        PlayerHPBar.Value = (playerhp * 1.0f) / Player.Instance.AttrMan.Attrs[EAttributeType.ATTR_MaxHP];
        PlayerEnBar.Value = (playeren * 1.0f) / Player.Instance.AttrMan.Attrs[EAttributeType.ATTR_MaxMP];
		
		//player hp effect
		if(currentHpE != playerhp){
			if(playerhp < currentHpE){
				if(isStartCalcTimeHp){
					StartTimeDelayHp = Time.time;
					isStartCalcTimeHp = false;
				}
				if((Time.time - StartTimeDelayHp) >= effectDelayHp){
					currentHpE -= effectSpeedHp;
					if(currentHpE < playerhp){
						currentHpE = playerhp;
					}
				}
			}else{
				currentHpE = playerhp;
//				if(isStartCalcTimeHp){
//					StartTimeDelayHp = Time.time;
//					isStartCalcTimeHp = false;
//				}
//				if((Time.time - StartTimeDelayHp) >= effectDelayHp){
//					currentHpE += effectSpeedHp;
//					if(currentHpE > playerhp){
//						currentHpE = playerhp;
//					}
//				}
			}
		}else{
			StartTimeDelayHp = 0;
			isStartCalcTimeHp =true;
		}
		//player en effect
		if(currentEnE != playeren){
			if(playeren < currentEnE){
				if(isStartCalcTimeEn){
					StartTimeDelayEn = Time.time;
					isStartCalcTimeEn = false;
				}
				if((Time.time - StartTimeDelayEn) >= effectDelayHp){
					currentEnE -= effectSpeedEn;
					if(currentEnE < playeren){
						currentEnE = playeren;
					}
				}
			}else{
				currentEnE = playeren;
//				if(isStartCalcTimeEn){
//					StarTimeDelayEn = Time.time;
//					isStartCalcTimeEn = false;
//				}
//				if((Time.time - StartTimeDelayEn) >= effectDelayHp){
//					currentEnE += effectSpeedEn;
//					if(currentEnE > playeren){
//						currentEnE = playeren;
//					}
//				}
			}
		}else{
			StartTimeDelayEn = 0;
			isStartCalcTimeEn =true;
		}
		PlayerHPBarE.Value = (currentHpE * 1.0f) / Player.Instance.AttrMan.Attrs[EAttributeType.ATTR_MaxHP];
        PlayerEnBarE.Value = (currentEnE * 1.0f) / Player.Instance.AttrMan.Attrs[EAttributeType.ATTR_MaxMP];
	}
}
