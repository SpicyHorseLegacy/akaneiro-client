using UnityEngine;
using System.Collections;

public class _UI_CS_SpiritTrainer_Cost : MonoBehaviour {
	
	//Instance
	public static _UI_CS_SpiritTrainer_Cost Instance = null;
	public UIButton 	bG;
	public UIButton 	cost;
	public SpriteText 	costText;
	public UIButton 	spiritBigIcon;
	public UIButton 	spiritSmallIcon;
	public SpriteText 	nameText;
	public UIButton 	buffSmallIcon;
	public UIButton 	TitleBg;
	public SpriteText 	buffDescriptText;
	public SpriteText 	DurationText;
	public SpriteText 	rightDescptionText;
	public int 			PetID;
	public Transform	spiritCostPos;
	
	void Awake()
	{
		Instance = this;
	}
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void CostDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.TAP:
				CS_Main.Instance.g_commModule.SendMessage(
			   		ProtocolGame_SendRequest.BuyPet(_UI_CS_SpiritTrainer_Cost.Instance.PetID, buypetTime.Prop_kshalfH_money)
				);
				SoundCue.PlayPrefabAndDestroy(_UI_CS_SpiritShopSound.Instance.BGMS[_UI_CS_SpiritTrainer.Instance.IconIdx]);
				break;
		   default:
				break;
		}	
	}
	

	public void SpiritCostBring(_UI_CS_SpiritHelperItem info){
		if(null == info){
			LogManager.Log_Warn("When update abi tips , abi Dont find info");
				return;
		}
		RightClickLogic(info);
		bG.transform.position = spiritCostPos.position;
		spiritSmallIcon.SetUVs(new Rect(0,0,1,1));
		spiritBigIcon.SetUVs(new Rect(0,0,1,1));
		if(_PlayerData.Instance.playerLevel >= info.m_levelReq){
			cost.transform.position 				= new Vector3(cost.transform.position.x,cost.transform.position.y,bG.transform.position.z - 0.1f);
			PetID 									= info.m_type;
			costText.Text							= info.m_PayMoney.ToString();
            buffDescriptText.renderer.material = LocalizeFontManager.Instance.GetCurrentMat();
			buffDescriptText.SetFont(LocalizeFontManager.Instance.GetCurrentFont(),LocalizeFontManager.Instance.GetCurrentMat());
			buffDescriptText.Text 					= info.m_Buffdescript;
			spiritBigIcon.gameObject.layer			= LayerMask.NameToLayer("Default");
			spiritSmallIcon.SetTexture(_UI_CS_SpiritInfo.Instance.spirirtSmallIcon[info.m_iconID]);
			nameText.renderer.material = LocalizeFontManager.Instance.GetCurrentMat();
			nameText.SetFont(LocalizeFontManager.Instance.GetCurrentFont(),LocalizeFontManager.Instance.GetCurrentMat());
			nameText.Text							= info.m_name;
			buffSmallIcon.gameObject.layer			= LayerMask.NameToLayer("EZGUI_CanTouch");
			DurationText.SetColor(_UI_Color.Instance.color6);
			LocalizeManage.Instance.GetDynamicText(DurationText,"DURATION30M");
			TitleBg.SetColor(_UI_CS_SpiritInfo.Instance.spirirtColor[info.m_iconID]);
			_UI_Spirit3DmodelCtrl.Instance.Hide(_UI_CS_SpiritTrainer.Instance.IconIdx);
			_UI_CS_SpiritTrainer.Instance.IconIdx = info.m_iconID;
			_UI_Spirit3DmodelCtrl.Instance.Show(info.m_iconID);
			if(info.m_IsSummoned||info.m_IsFreeDay){
				LocalizeManage.Instance.GetDynamicText(rightDescptionText,"RCC");
			}else{
				LocalizeManage.Instance.GetDynamicText(rightDescptionText,"RCSU");
			}
		}else{
			cost.transform.position = new Vector3(cost.transform.position.x,cost.transform.position.y,999f);
			spiritBigIcon.gameObject.layer			= LayerMask.NameToLayer("EZGUI_CanTouch");
			spiritBigIcon.SetTexture(_UI_CS_SpiritInfo.Instance.spirirtIcon[7]);
			spiritSmallIcon.SetTexture(_UI_CS_SpiritInfo.Instance.spirirtSmallIcon[7]);
			buffSmallIcon.gameObject.layer			= LayerMask.NameToLayer("Default");
			buffDescriptText.Text					= ""; 
			DurationText.SetColor(_UI_Color.Instance.color1);
			LocalizeManage.Instance.GetDynamicText(DurationText,"REQLEVEL");
			DurationText.Text += info.m_levelReq.ToString();
			TitleBg.SetColor(_UI_CS_SpiritInfo.Instance.spirirtColor[7]);
			nameText.Text							= "";
			_UI_Spirit3DmodelCtrl.Instance.Hide(_UI_CS_SpiritTrainer.Instance.IconIdx);
		}
	}
	
	public void SpiritCostDismiss(){
		_UI_Spirit3DmodelCtrl.Instance.Hide(_UI_CS_SpiritTrainer.Instance.IconIdx);
		bG.transform.position = new Vector3(999f,999f,999f);
	}
	
	public void RightClickLogic(_UI_CS_SpiritHelperItem info){
		if(_PlayerData.Instance.playerLevel >= info.m_levelReq){
			if(Input.GetButtonDown("Fire2")){
				if(info.m_IsSummoned||info.m_IsFreeDay){
					CS_Main.Instance.g_commModule.SendMessage(
				   		ProtocolGame_SendRequest.choosePet(info.m_type)
					);
				}else{
					CS_Main.Instance.g_commModule.SendMessage(
											ProtocolGame_SendRequest.BuyPet(PetID,3)
															  );   
				}
			}
		}
	}
	
}
