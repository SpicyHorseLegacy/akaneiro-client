using UnityEngine;
using System.Collections;

public class _UI_CS_ScreenCtrl : MonoBehaviour {
	
	//Instance
	public static _UI_CS_ScreenCtrl Instance = null;
	
	public enum EM_SCREEN_TYPE {
		EM_LOGIN  = 0,
		EM_SELECT,
		EM_CREATE,
		EM_LOADING,
		EM_INGAME_MENU,
		EM_INGAME_NORMAL,
		EM_LEVELUP,
		EM_ITEM_VENDOR,
		EM_ITEM_VENDOR_RARE,
		EM_EVENT_REWARDS,
		EM_EVENT_NEWS,
		EM_SPIRT_TRAINER,
		EM_BOUNTY_MASTER,
		EM_MISSION_SELECT,
		EM_ABILITIES_TRAINER,
		EM_UPGRADE_VENDOR,
		EM_AINU_ELDER,
		EM_MISSION_SUMMARY,
		EM_REVIVAL_MENU,
		EM_CONSUMABLE_MENU,
		EM_MAIL_SYSYTEM,
		EM_INGAME_MENU_ABI,
		EM_INGAME_MENU_INV,
		EM_INGAME_MENU_INFO,
		EM_INGAME_MENU_ACH,
		EM_INGAME_MENU_OPT,
		EM_SUMMON_REWARD,
		EM_TAKE_GIFT,
		EM_CRAFTING,
		EM_STASH,
		EM_WAIT,
		EM_MAX
	}
	
	public EM_SCREEN_TYPE currentScreenType;
	
	void Awake()
	{
		Instance = this;
	}
	
	// Use this for initialization
	void Start () {
		currentScreenType = _UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_LOGIN;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void SetNextScreenType(EM_SCREEN_TYPE type){
		currentScreenType = type;
	}
	
	public bool IsScreenType(EM_SCREEN_TYPE type){
		if(currentScreenType == type){
			return true;
		}else{
			return false;
		}
	}
}
