using UnityEngine;
using System.Collections;

public class _UI_CS_PopupBoxCtrl : MonoBehaviour {
	
	//Instance
	public static _UI_CS_PopupBoxCtrl Instance = null;
	
	public UIPanel 		m_CS_popUpBox;
	public UIButton 	m_popUp_BackButton;
	public SpriteText 	m_popUp_MSGText;
	
	public UIPanel 		m_CS_popUpBoxDele;
	public SpriteText 	m_popUp_MSGTextDele;
	
	public delegate   void CallbackFun();
	
	public Transform  failSound;
	
	void Awake()
	{
		Instance = this;
	}
	
	// Use this for initialization
	void Start () {
		m_popUp_BackButton.AddInputDelegate(popUp_BackButtonDelegate);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public static void PopUpError(string msg)
	{
		if(Instance != null) {
			SoundCue.PlayPrefabAndDestroy(Instance.failSound);
			Instance.m_popUp_MSGText.Text = msg;
			Instance.m_CS_popUpBox.BringIn();
		}
	}
	
    public static void PopUpError(EServerErrorType _error)
    {
		SoundCue.PlayPrefabAndDestroy(Instance.failSound);
        if (_error.Get() == EServerErrorType.eItemError_BuyNotEnoughMoneny || _error.Get() == EServerErrorType.eGS_LessMoney_karma)
        {
//            Instance.m_popUp_MSGText.Text = "You don't have enough Karma to complete this transaction.";
			LocalizeManage.Instance.GetDynamicText(Instance.m_popUp_MSGText,"eItemError_BuyNotEnoughMoneny");
			LessMoneyMsg.Instance.isKarma = true;
			LessMoneyMsg.Instance.bgPanel.BringIn();
        }
		else if (_error.Get() == EServerErrorType.eGS_LessMoney_fk)
        {
//          Instance.m_popUp_MSGText.Text = "You don't have enough Shinigami Scrolls.";
			LocalizeManage.Instance.GetDynamicText(Instance.m_popUp_MSGText,"eItemError_BuyNotEnoughMoneny");
			LessMoneyMsg.Instance.isKarma = false;
			LessMoneyMsg.Instance.bgPanel.BringIn();
        }
		else if (_error.Get() == EServerErrorType.eGS_LessReviveItem)
        {
//            Instance.m_popUp_MSGText.Text = "You don't have enough Shinigami Scrolls.";
			LocalizeManage.Instance.GetDynamicText(Instance.m_popUp_MSGText,"eGS_LessReviveItem");
        }
        else if (_error.Get() == EServerErrorType.eGS_LessLevel)
        {
//            Instance.m_popUp_MSGText.Text = "You do not meet the level requirement.";
			LocalizeManage.Instance.GetDynamicText(Instance.m_popUp_MSGText,"eGS_LessLevel");
        }
		else if (_error.Get() == EServerErrorType.eCreateCharacter_AlreadyNickName)
		{
//			Instance.m_popUp_MSGText.Text = "Character name is already in use.";
			LocalizeManage.Instance.GetDynamicText(Instance.m_popUp_MSGText,"eCreateCharacter_AlreadyNickName");
		}
		else if (_error.Get() == EServerErrorType.eCreateCharacter_CharacterFull)
		{
//			Instance.m_popUp_MSGText.Text = "You have reached the maximum number of characters.";
			LocalizeManage.Instance.GetDynamicText(Instance.m_popUp_MSGText,"eCreateCharacter_CharacterFull");
		}
		else if (_error.Get() == EServerErrorType.eCreateCharacter_ParamError)
		{
//			Instance.m_popUp_MSGText.Text = "Your name contains invalid characters.";
			LocalizeManage.Instance.GetDynamicText(Instance.m_popUp_MSGText,"eCreateCharacter_ParamError");
		}
		else if (_error.Get() == EServerErrorType.eItemError_BagFull)
		{
//			Instance.m_popUp_MSGText.Text = "Your inventory is full.";
			LocalizeManage.Instance.GetDynamicText(Instance.m_popUp_MSGText,"eItemError_BagFull");
		}
		else if (_error.Get() == EServerErrorType.eGS_SkillNotExist)
		{
//			Instance.m_popUp_MSGText.Text = "That skill no longer exists.";
			LocalizeManage.Instance.GetDynamicText(Instance.m_popUp_MSGText,"eGS_SkillNotExist");
		}
		else if (_error.Get() == EServerErrorType.eItemError_NoExist)
		{
//			Instance.m_popUp_MSGText.Text = "That item no longer exists.";
			LocalizeManage.Instance.GetDynamicText(Instance.m_popUp_MSGText,"eItemError_NoExist");
		}
		else if (_error.Get() == EServerErrorType.eUserAlreadyOnline)
		{
//			Instance.m_popUp_MSGText.Text = "You are already logged in.";
			LocalizeManage.Instance.GetDynamicText(Instance.m_popUp_MSGText,"eUserAlreadyOnline");
		}
		else if (_error.Get() == EServerErrorType.eWrongAccountOrPwd)
		{
//			Instance.m_popUp_MSGText.Text = "Account name or password are incorrect.";
			LocalizeManage.Instance.GetDynamicText(Instance.m_popUp_MSGText,"eWrongAccountOrPwd");
		}
		else if (_error.Get() == EServerErrorType.eMission_have_buy_area)
		{
//			Instance.m_popUp_MSGText.Text = "You have already purchased this area.";
			LocalizeManage.Instance.GetDynamicText(Instance.m_popUp_MSGText,"eMission_have_buy_area");
		}
		else if (_error.Get() == EServerErrorType.eGS_UserStateError)
		{
//			Instance.m_popUp_MSGText.Text = "A server error occurred. Please reload the game.";
			LocalizeManage.Instance.GetDynamicText(Instance.m_popUp_MSGText,"eGS_UserStateError");
		}
		else if (_error.Get() == EServerErrorType.eItemError_rareItemSellOut)
		{
//			Instance.m_popUp_MSGText.Text = "Sorry Hunter, this item has sold out.";
			LocalizeManage.Instance.GetDynamicText(Instance.m_popUp_MSGText,"eItemError_rareItemSellOut");
		}
		else if (_error.Get() == EServerErrorType.eItemError_BuyCantFindItem)
		{
//			Instance.m_popUp_MSGText.Text = "This item is currently out of stock.";
			LocalizeManage.Instance.GetDynamicText(Instance.m_popUp_MSGText,"eItemError_BuyCantFindItem");
		}
		else if (_error.Get() == EServerErrorType.eItemError_SellOut)
		{
//			Instance.m_popUp_MSGText.Text = "Sorry Hunter, this item has sold out.";
			LocalizeManage.Instance.GetDynamicText(Instance.m_popUp_MSGText,"eItemError_SellOut");
		}
		else if (_error.Get() == EServerErrorType.eGift_NotExist)
		{
//			Instance.m_popUp_MSGText.Text = "Invalid key. Please check that you have entered the key correctly.";
			LocalizeManage.Instance.GetDynamicText(Instance.m_popUp_MSGText,"eGift_NotExist");
		}
		else if (_error.Get() == EServerErrorType.eGift_HaveBeenUsed)
		{
//			Instance.m_popUp_MSGText.Text = "This key has already been redeemed.";
			LocalizeManage.Instance.GetDynamicText(Instance.m_popUp_MSGText,"eGift_HaveBeenUsed");
		}
		else if (_error.Get() == EServerErrorType.eGS_LessDiscipline)
		{
//			Instance.m_popUp_MSGText.Text = "This key has already been redeemed.";
			LocalizeManage.Instance.GetDynamicText(Instance.m_popUp_MSGText,"NODISCIPLINE");
		}
		else if (_error.Get() == EServerErrorType.eGS_SkillColding || _error.Get() == EServerErrorType.eMasteryError_CoolDown)
		{
			Instance.m_popUp_MSGText.Text = "You already have a skill which is in Cool down.";
		}
//		else if (_error.Get() == EServerErrorType.)
//		{
//			Instance.m_popUp_MSGText.Text = "You already have a skill which is in Cool down.";
//		}
        else
        {
            Instance.m_popUp_MSGText.Text = _error.GetString();
        }
		
		if (_error.Get() != EServerErrorType.eItemError_BuyNotEnoughMoneny && _error.Get() != EServerErrorType.eGS_LessMoney_fk && _error.Get() != EServerErrorType.eGS_LessMoney_karma)
		{
       		Instance.m_CS_popUpBox.BringIn();
		}
    }
	
	void popUp_BackButtonDelegate(ref POINTER_INFO ptr)
	{

		switch(ptr.evt)
		{
		   case POINTER_INFO.INPUT_EVENT.TAP:
				
				m_CS_popUpBox.Dismiss();
			
				break;
		   default:
				break;
		}	
	}	
	
	
	
	public void PopUpDeleBox(string msg,CallbackFun fun){
		
		m_popUp_MSGTextDele.Text = msg;
		m_CS_popUpBoxDele.BringIn();
		StartCoroutine(CallBackFun(fun));
		
	}
	
	private IEnumerator CallBackFun(CallbackFun fun)
	{
		yield return new WaitForSeconds(1.5f);
		
		if(null != fun)
			fun();
		
		m_CS_popUpBoxDele.Dismiss();
		
	}
	
}
