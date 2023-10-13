using UnityEngine;
using System.Collections;

public class PopUpBox : MonoBehaviour {
	
	//Instance
	public static PopUpBox Instance = null;
	
	void Awake() {
		Instance = this;
	}
	
	// Use this for initialization
	void Start () {
		Hide(true);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public static bool isShowDisConnectText = true;
	
	#region Interface
	public Transform  failSound;
	public UILabel msg;
	public static void PopUpErr(string msg) {
		if(Instance != null) {
			Hide(false);
			SoundCue.PlayPrefabAndDestroy(Instance.failSound);
			Instance.msg.text = msg;
		}
	}
	
	public static void PopUpErr(EServerErrorType _error) {
		isShowDisConnectText = true;
		//bring panel.//
		Hide(false);
		SoundCue.PlayPrefabAndDestroy(Instance.failSound);
        if (_error.Get() == EServerErrorType.eItemError_BuyNotEnoughMoneny || _error.Get() == EServerErrorType.eGS_LessMoney_karma) {
			Instance.msg.text = LocalizeManage.Instance.GetDynamicText("eItemError_BuyNotEnoughMoneny");
			if (Steamworks.activeInstance) {
				Steamworks.activeInstance.ShowShop("karma");
			}else {
				GUIManager.Instance.AddTemplate("KarmaRecharge");
			}
			Hide(true);
			return;
        }else if (_error.Get() == EServerErrorType.eGS_LessMoney_fk) {
			if (Steamworks.activeInstance) {
				Steamworks.activeInstance.ShowShop("crystal");
			}else {
				GUIManager.Instance.AddTemplate("CrystalRecharge");
			}
			Hide(true);
			return;
        }else if (_error.Get() == EServerErrorType.eGS_LessReviveItem) {
			Instance.msg.text = LocalizeManage.Instance.GetDynamicText("eGS_LessReviveItem");
        }else if (_error.Get() == EServerErrorType.eCreateCharacter_InvalidPlayer) {
			Instance.msg.text = "Please First Create chatacter.";
        }else if (_error.Get() == EServerErrorType.eGS_LessLevel) {
			Instance.msg.text = LocalizeManage.Instance.GetDynamicText("eGS_LessLevel");
        }else if (_error.Get() == EServerErrorType.eCreateCharacter_AlreadyNickName) {
			Instance.msg.text = LocalizeManage.Instance.GetDynamicText("eCreateCharacter_AlreadyNickName");
		}else if (_error.Get() == EServerErrorType.eCreateCharacter_CharacterFull) {
			Instance.msg.text = LocalizeManage.Instance.GetDynamicText("eCreateCharacter_CharacterFull");
		}else if (_error.Get() == EServerErrorType.eCreateCharacter_ParamError) {
			Instance.msg.text = LocalizeManage.Instance.GetDynamicText("eCreateCharacter_ParamError");
		}else if (_error.Get() == EServerErrorType.eItemError_BagFull) {
			Instance.msg.text = LocalizeManage.Instance.GetDynamicText("eItemError_BagFull");
		}else if (_error.Get() == EServerErrorType.eGS_SkillNotExist) {
			Instance.msg.text = LocalizeManage.Instance.GetDynamicText("eGS_SkillNotExist");
		}else if (_error.Get() == EServerErrorType.eItemError_NoExist) {
			Instance.msg.text = LocalizeManage.Instance.GetDynamicText("eItemError_NoExist");
		}else if (_error.Get() == EServerErrorType.eUserAlreadyOnline) {
			Instance.msg.text = LocalizeManage.Instance.GetDynamicText("eUserAlreadyOnline");
			isShowDisConnectText = false;
		}else if (_error.Get() == EServerErrorType.eWrongAccountOrPwd) {
			Instance.msg.text = LocalizeManage.Instance.GetDynamicText("eWrongAccountOrPwd");
			isShowDisConnectText = false;
		}else if (_error.Get() == EServerErrorType.eMission_have_buy_area) {
			Instance.msg.text = LocalizeManage.Instance.GetDynamicText("eMission_have_buy_area");
		}else if (_error.Get() == EServerErrorType.eGS_UserStateError) {
			Instance.msg.text = LocalizeManage.Instance.GetDynamicText("eGS_UserStateError");
		}else if (_error.Get() == EServerErrorType.eItemError_rareItemSellOut) {
			Instance.msg.text = LocalizeManage.Instance.GetDynamicText("eItemError_rareItemSellOut");
		}else if (_error.Get() == EServerErrorType.eItemError_BuyCantFindItem) {
			Instance.msg.text = LocalizeManage.Instance.GetDynamicText("eItemError_BuyCantFindItem");
		}else if (_error.Get() == EServerErrorType.eItemError_SellOut) {
			Instance.msg.text = LocalizeManage.Instance.GetDynamicText("eItemError_SellOut");
		}else if (_error.Get() == EServerErrorType.eGift_NotExist) {
			Instance.msg.text = LocalizeManage.Instance.GetDynamicText("eGift_NotExist");
		}else if (_error.Get() == EServerErrorType.eGift_HaveBeenUsed) {
			Instance.msg.text = LocalizeManage.Instance.GetDynamicText("eGift_HaveBeenUsed");
		}else if (_error.Get() == EServerErrorType.eGS_LessDiscipline) {
			Instance.msg.text = LocalizeManage.Instance.GetDynamicText("NODISCIPLINE");
		}else if(_error.Get() == EServerErrorType.eGS_SkillColding) {
			Instance.msg.text = LocalizeManage.Instance.GetDynamicText("eGS_SkillColding");
		}else if(_error.Get() == EServerErrorType.eMasteryError_CoolDown) {
			Instance.msg.text = LocalizeManage.Instance.GetDynamicText("eMasteryError_CoolDown");
		}else if(_error.Get() == EServerErrorType.eItemCraftError_ParamError) {
			Instance.msg.text = LocalizeManage.Instance.GetDynamicText("eItemCraftError_ParamError");
		}else if(_error.Get() == EServerErrorType.ePetError_repeatPet) {
			Instance.msg.text = LocalizeManage.Instance.GetDynamicText("ePetError_repeatPet");
		}else{
            Instance.msg.text = _error.GetString();
        }
	}
	
	public static void SetMsg(string msg) {
		Instance.msg.text = msg;
	}
	
	public static void Hide(bool isHide) {
		NGUITools.SetActive(Instance.gameObject,!isHide);
	}
	#endregion
	#region Local
	private void OKDelegate() {
		Hide(true);
		if(GameManager.Instance.GetDisconnectFlag()) {
			GUIManager.Instance.ChangeUIScreenState("LoginScreen");
			Application.LoadLevelAsync("EmptyScenes");
			if(	CS_SceneInfo.Instance != null) {
		   		CS_SceneInfo.Instance.ClearALLObjects();
			}
		}
	}
	#endregion
	
	public delegate void CallbackFun();
	public static void PopUpDeleBox(string msg,CallbackFun fun){
		PopUpErr(msg);
		Instance.StartCoroutine(Instance.CallBackFun(fun));
	}
	
	private IEnumerator CallBackFun(CallbackFun fun)
	{
		yield return new WaitForSeconds(1.5f);
		
		if(null != fun)
			fun();
		
		Hide(true);
	}
}
