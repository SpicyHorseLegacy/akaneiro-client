using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class _UI_CS_CreateMenu : MonoBehaviour {
	//Instance
	public static _UI_CS_CreateMenu Instance = null;
	public UIPanel		m_CS_CharacterCreatePanel;
	public UIButton 	m_CharacterCreate_ExitButton;
	public UIButton 	m_CharacterCreate_CreateButton;
	public UIRadioBtn  [] m_CharacterCreate_DisciplinesButton;
	public UIButton  [] m_CharacterCreate_DisciplinesName1Text;
	public UIButton  [] m_CharacterCreate_DisciplinesName2Text;
	public UIRadioBtn  [] m_CharacterCreate_Sex;
	public UITextField 	m_CharacterCreate_NameEditText;
	public  int  		m_Character_DisciplinesIndex = 2;
	public int 			DisciplinesType = 2;
	public bool 		m_isFirstInput = true;
	public UIButton 	m_Icon;
	public Texture2D [] m_DisTexture;
	public UIButton 	m_AbiIcon;
	public SpriteText	m_AbilNameText;
	public int 		 [] m_abilID;
	public SpriteText	m_abiInfoText;
	private int 		 m_VfxCount = 0;
	public Color RedC 	= new Color(1f,1f,1f,0.5f);
	public Color GreenC = new Color(1f,1f,1f,0.5f);
	public Color BlueC 	= new Color(1f,1f,1f,0.5f);	
	public SpriteText	m_TitleText;
	public SpriteText	m_DisText;
	public SpriteText	m_Info1Text;
	public SpriteText	m_Info2Text;
	public SpriteText	m_Info3Text;
	public SpriteText	m_Info4Text;
	public SpriteText	m_Info5Text;
	public SpriteText	m_Info6Text;
	public UIButton		m_AbiBK;
	public string 		m_createName = "";
	public  List<string> 		Info1List   = new List<string>();
	public  List<string> 		Info2List   = new List<string>();
	public  List<string> 		Info3List   = new List<string>();
	public  List<string> 		Info4List   = new List<string>();
	public  List<string> 		Info5List   = new List<string>();
	public  List<string> 		Info6List   = new List<string>();

	void Awake(){
		Instance = this;
	}
	
	// Use this for initialization
	void Start () {
		m_CharacterCreate_DisciplinesButton[0].AddInputDelegate(CharacterCreateDisciplinesCunningDelegate);
		m_CharacterCreate_DisciplinesButton[1].AddInputDelegate(CharacterCreateDisciplinesFortitudeDelegate);
		m_CharacterCreate_DisciplinesButton[2].AddInputDelegate(CharacterCreateDisciplinesProwessDelegate);
		m_CharacterCreate_Sex[0].AddInputDelegate(PressSexF);
		m_CharacterCreate_Sex[1].AddInputDelegate(PressSexM);
		m_CharacterCreate_ExitButton.AddInputDelegate(CharacterCreateExitDelegate);
		m_CharacterCreate_CreateButton.AddInputDelegate(CharacterCreateCreateDelegate);
		m_CharacterCreate_NameEditText.AddInputDelegate(CharacterCreate_NameEditTextInputDelegate);
		m_CharacterCreate_NameEditText.SetValidationDelegate(NameEditDelegate);
		ReadTextInfo();
		RestCreateInfo();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void ReadTextInfo(){
		string fileName = "CreateCharaInfo.Description";
		string _fileName = LocalizeManage.Instance.GetLangPath(fileName);
		TextAsset item = (TextAsset)Resources.Load(_fileName, typeof(TextAsset));
		string[] itemRowsList = item.text.Split('\n');
		for (int i = 3; i < itemRowsList.Length - 1; ++i){
			string pp = itemRowsList[i];
		   	string[] vals = pp.Split(new char[] { '	', '	' });	
			Info1List.Add(vals[0]);
			Info2List.Add(vals[1]);
			Info3List.Add(vals[2]);
			Info4List.Add(vals[3]);
			Info5List.Add(vals[4]);
			Info6List.Add(vals[5]);
		}
		LogManager.Log_Info("CreateCharaInfo Ok");
	}
	
	void SetCharaTextInfo(int idx){
		m_Info1Text.Text = Info1List[idx];
		m_Info2Text.Text = Info2List[idx];
		m_Info3Text.Text = Info3List[idx];
		m_Info4Text.Text = Info4List[idx];
		m_Info5Text.Text = Info5List[idx];
		m_Info6Text.Text = Info6List[idx];
	}
	
	public void PressSexM(ref POINTER_INFO ptr){
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.TAP:
				CreateMenuCtrl.Instance.SelectDis(m_Character_DisciplinesIndex-1,1);
			break;
		default:
			break;
		}
	
	}
	
	public void PressSexF(ref POINTER_INFO ptr){
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.TAP:
				CreateMenuCtrl.Instance.SelectDis(m_Character_DisciplinesIndex-1,0);
			break;
		default:
			break;
		}
	}
	
	public string NameEditDelegate(UITextField field,string text){
		m_createName = text;
		return text;
			
	}
	
	void CharacterCreateExitDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.TAP:
				PlayerInfoBar.Instance.UpdateAllyPos();
				int currentIdx = SelectChara.Instance.GetCurrentSelectIdx();
				_UI_CS_PopupBoxCtrl.Instance.m_CS_popUpBox.Dismiss();
				m_CS_CharacterCreatePanel.Dismiss();
				SelectChara.Instance.AwakeSelectChara();
				m_isFirstInput = true;
				m_CharacterCreate_NameEditText.Text = "Enter Name";
//				LocalizeManage.Instance.GetDynamicText(m_CharacterCreate_NameEditText,"ENTERNAME");
				RestCreateInfo();
				m_Character_DisciplinesIndex = 2;
				DisciplinesType = 2;
				SelectDis(1);
				if(-1 != currentIdx){
					SelectChara.Instance.SetActiveCharaBtn(currentIdx);
				}else{
                    SelectChara.Instance.Model.Hide(true);
				}
				LeaveCreaterScenes(false);
				_UI_CS_ScreenCtrl.Instance.SetNextScreenType(_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_SELECT);
					
				break;
		   default:
				break;
		}	
	}
	
	void CharacterCreateDisciplinesCunningDelegate(ref POINTER_INFO ptr){
		int index;
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.TAP:
				SelectDis(2);
				break;
		   default:
				break;
		}	
	}
	
	void CharacterCreateDisciplinesFortitudeDelegate(ref POINTER_INFO ptr){
		int index;
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.TAP:
				SelectDis(1);
				break;
		   default:
				break;
		}	
	}
	
	void CharacterCreateDisciplinesProwessDelegate(ref POINTER_INFO ptr)
	{
		int index;
		switch(ptr.evt)
		{
		   case POINTER_INFO.INPUT_EVENT.TAP:
				
			
				SelectDis(0);

				break;
		   default:
				break;
		}	
	}
	
	void CharacterCreateCreateDelegate(ref POINTER_INFO ptr){	
		SCharacterCreationData scd = new SCharacterCreationData();
		mapIntInt avatarTraits = new mapIntInt();
		switch(ptr.evt){
		  case POINTER_INFO.INPUT_EVENT.PRESS:
//				Transform Vfx = UnityEngine.Object.Instantiate(_UI_CS_Ctrl.Instance.playVfx)as Transform;
//				Vfx.transform.position = new  Vector3(m_CharacterCreate_CreateButton.transform.position.x+ 3.1f,m_CharacterCreate_CreateButton.transform.position.y,m_CharacterCreate_CreateButton.transform.position.z - 0.1f);
		   break;
		   case POINTER_INFO.INPUT_EVENT.TAP:
				LogManager.Log_Debug("---  CharacterCreate ---");
				if(m_createName.Length < 3 || m_createName.Length > 12){
//					_UI_CS_PopupBoxCtrl.Instance.m_popUp_MSGText.Text = "Character names must be at least 3 characters long with no spaces or symbols.";
					LocalizeManage.Instance.GetDynamicText(_UI_CS_PopupBoxCtrl.Instance.m_popUp_MSGText,"CREATECHARINPUTERR");
					_UI_CS_PopupBoxCtrl.Instance.m_CS_popUpBox.BringIn();
					return;
				}
				PlayerInfoBar.Instance.UpdateAllyPos();
				scd.nickname = m_CharacterCreate_NameEditText.Text;
				scd.style	 = DisciplinesType;
				scd.avatarTraits = avatarTraits;
				scd.sex = new ESex();
				if(m_CharacterCreate_Sex[1].Value){
					scd.sex.Set(0);
				}else{	
					scd.sex.Set(1);
				}
				m_isFirstInput = true;
				m_CharacterCreate_NameEditText.Text = "Enter Name";
//				LocalizeManage.Instance.GetDynamicText(m_CharacterCreate_NameEditText,"ENTERNAME");
				RestCreateInfo();
				m_Character_DisciplinesIndex = 2;
				DisciplinesType = 2;
				SelectDis(1);
				CS_Main.Instance.g_commModule.SendMessage(
			   		ProtocolGame_SendRequest.UserCreateCharacter(scd)
				);
//				_UI_CS_PopupBoxCtrl.Instance.m_popUp_MSGText.Text = "Creating Character...";
				LocalizeManage.Instance.GetDynamicText(_UI_CS_PopupBoxCtrl.Instance.m_popUp_MSGText,"CREATEINGCHARA");
				_UI_CS_PopupBoxCtrl.Instance.m_CS_popUpBox.BringIn();
				break;
		   default:
				break;
		}	
	}
	
	void CharacterCreate_NameEditTextInputDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
			case POINTER_INFO.INPUT_EVENT.PRESS:	
				if(m_isFirstInput){
					m_isFirstInput = false;
					m_CharacterCreate_NameEditText.Text = "";
				}
				break;
		   default:
				break;
		}	
	}

	public void RestCreateInfo(){
		int index;
		m_CharacterCreate_DisciplinesButton[0].Value = false;
		m_CharacterCreate_DisciplinesButton[1].Value = true;
		m_CharacterCreate_DisciplinesButton[2].Value = false;
		m_CharacterCreate_DisciplinesName2Text[0].Hide(true);
		m_CharacterCreate_DisciplinesName2Text[1].Hide(false);
		m_CharacterCreate_DisciplinesName2Text[2].Hide(true);
		m_CharacterCreate_DisciplinesName1Text[0].SetColor(_UI_Color.Instance.color4);
		m_CharacterCreate_DisciplinesName1Text[1].SetColor(_UI_Color.Instance.color1);
		m_CharacterCreate_DisciplinesName1Text[2].SetColor(_UI_Color.Instance.color4);
		m_CharacterCreate_Sex[0].Value = true;
		m_CharacterCreate_Sex[1].Value = false;
		m_Icon.SetUVs(new Rect(0,0,1,1));
		m_Icon.SetTexture(m_DisTexture[1]);
		for(index = 0; index < AbilityInfo.Instance.PlayerAbilityPool.Count;index++){
					AbilityBaseState ability = AbilityInfo.Instance.PlayerAbilityPool[index];
					if(ability.id == m_abilID[1]){	
						m_AbiIcon.SetUVs(new Rect(0,0,1,1));
						m_AbiIcon.SetTexture(ability.icon);
						m_AbilNameText.Text = ability.name;
                        m_abiInfoText.Text = ability.Info.Description1;
					}
		}
		SetCharaTextInfo(1);
	}
	
	public void GotoCreaterScenes(){
	
		StartCoroutine(WaitPanelPass1());
	
	}

	public void LeaveCreaterScenes(bool isFromCreateOK){
        if (CreateMenuCtrl.Instance)
            CreateMenuCtrl.Instance.DisableBGM();
		    StartCoroutine(WaitPanelPass2(isFromCreateOK));
	}
	
	public IEnumerator WaitPanelPass1()
	{	
		yield return null;

		AsyncOperation async= Application.LoadLevelAsync("CharacterCreate");
		
		yield return async;
	
	}

    public IEnumerator WaitPanelPass2(bool isFromCreateOK){
        yield return null;
        AsyncOperation async = Application.LoadLevelAsync("EmptyScenes");
        yield return async;
        if (!isFromCreateOK)
            SelectChara.Instance.UpdateModelEquip(SelectChara.Instance.GetCurrentSelectIdx());
    }
	
	public void SelectDis(int idx){	
		int index;
		SetCharaTextInfo(idx);
		switch(idx){	
		case 0:	
			m_Character_DisciplinesIndex = 1;
			DisciplinesType = 1;
				m_Icon.SetUVs(new Rect(0,0,1,1));
				m_Icon.SetTexture(m_DisTexture[2]);
				for(index = 0; index < AbilityInfo.Instance.PlayerAbilityPool.Count;index++){
					AbilityBaseState ability = AbilityInfo.Instance.PlayerAbilityPool[index];
					if(ability.id == m_abilID[2]){	
						m_AbiIcon.SetUVs(new Rect(0,0,1,1));
						m_AbiIcon.SetTexture(ability.icon);
						m_AbilNameText.Text = ability.name;
                        m_abiInfoText.Text = ability.Info.Description1;
					}
				}
				m_TitleText.SetColor(RedC);
				m_DisText.SetColor(RedC);
				m_Info3Text.SetColor(RedC);
				m_Info4Text.SetColor(RedC);
				m_Info5Text.SetColor(RedC);
				m_Info6Text.SetColor(RedC);
//				m_TitleText.Text = "Prowess";
				LocalizeManage.Instance.GetDynamicText(m_TitleText,"PROWESS");
//				m_DisText.Text   = "Prowess";	
				LocalizeManage.Instance.GetDynamicText(m_DisText,"PROWESS");
				m_AbiBK.SetColor(RedC);
				if(null != CreateMenuCtrl.Instance){
					if(m_CharacterCreate_Sex[1].Value){
						CreateMenuCtrl.Instance.SelectDis(0,1);
					}else{
						CreateMenuCtrl.Instance.SelectDis(0,0);
					}
				}
				m_CharacterCreate_DisciplinesName2Text[0].Hide(false);
				m_CharacterCreate_DisciplinesName2Text[1].Hide(true);
				m_CharacterCreate_DisciplinesName2Text[2].Hide(true);
				m_CharacterCreate_DisciplinesName1Text[0].SetColor(_UI_Color.Instance.color1);
				m_CharacterCreate_DisciplinesName1Text[1].SetColor(_UI_Color.Instance.color4);
				m_CharacterCreate_DisciplinesName1Text[2].SetColor(_UI_Color.Instance.color4);
			break;		
		case 1:	
			m_Character_DisciplinesIndex = 2;
			DisciplinesType = 2;
				m_Icon.SetUVs(new Rect(0,0,1,1));
				m_Icon.SetTexture(m_DisTexture[1]);
				for(index = 0; index < AbilityInfo.Instance.PlayerAbilityPool.Count;index++){
					AbilityBaseState ability = AbilityInfo.Instance.PlayerAbilityPool[index];
					if(ability.id == m_abilID[1]){			
						m_AbiIcon.SetUVs(new Rect(0,0,1,1));
						m_AbiIcon.SetTexture(ability.icon);
						m_AbilNameText.Text = ability.name;
                        m_abiInfoText.Text = ability.Info.Description1;
					}
				}
				m_TitleText.SetColor(_UI_Color.Instance.color6);
				m_DisText.SetColor(_UI_Color.Instance.color6);
				m_Info3Text.SetColor(_UI_Color.Instance.color6);
				m_Info4Text.SetColor(_UI_Color.Instance.color6);
			    m_Info5Text.SetColor(_UI_Color.Instance.color6);
				m_Info6Text.SetColor(_UI_Color.Instance.color6);
				m_TitleText.Text = "Fortitude";
				LocalizeManage.Instance.GetDynamicText(m_TitleText,"FORTITUDE");
				m_DisText.Text   = "Fortitude";	
				LocalizeManage.Instance.GetDynamicText(m_DisText,"FORTITUDE");
				m_AbiBK.SetColor(_UI_Color.Instance.color6);
				if(null != CreateMenuCtrl.Instance){
					if(m_CharacterCreate_Sex[1].Value){
						CreateMenuCtrl.Instance.SelectDis(1,1);
					}else{
						CreateMenuCtrl.Instance.SelectDis(1,0);
					}
				}
				m_CharacterCreate_DisciplinesName2Text[0].Hide(true);
				m_CharacterCreate_DisciplinesName2Text[1].Hide(false);
				m_CharacterCreate_DisciplinesName2Text[2].Hide(true);
				m_CharacterCreate_DisciplinesName1Text[0].SetColor(_UI_Color.Instance.color4);
				m_CharacterCreate_DisciplinesName1Text[1].SetColor(_UI_Color.Instance.color1);
				m_CharacterCreate_DisciplinesName1Text[2].SetColor(_UI_Color.Instance.color4);
			break;	
		case 2:
			m_Character_DisciplinesIndex = 3;
			DisciplinesType = 4;	
				m_Icon.SetUVs(new Rect(0,0,1,1));
				m_Icon.SetTexture(m_DisTexture[0]);
				for(index = 0; index < AbilityInfo.Instance.PlayerAbilityPool.Count;index++){
					AbilityBaseState ability = AbilityInfo.Instance.PlayerAbilityPool[index];
					if(ability.id == m_abilID[0]){	
						m_AbiIcon.SetUVs(new Rect(0,0,1,1));
						m_AbiIcon.SetTexture(ability.icon);
						m_AbilNameText.Text = ability.name;
                        m_abiInfoText.Text = ability.Info.Description1;
					}
				}
				m_TitleText.SetColor(BlueC);
				m_DisText.SetColor(BlueC);
				m_Info3Text.SetColor(BlueC);
				m_Info4Text.SetColor(BlueC);
			    m_Info5Text.SetColor(BlueC);
				m_Info6Text.SetColor(BlueC);
//				m_TitleText.Text = "Cunning";
				LocalizeManage.Instance.GetDynamicText(m_TitleText,"CUNNING");
//				m_DisText.Text   = "Cunning";
				LocalizeManage.Instance.GetDynamicText(m_DisText,"CUNNING");
				m_AbiBK.SetColor(BlueC);
				if(null != CreateMenuCtrl.Instance){
					if(m_CharacterCreate_Sex[1].Value){
						CreateMenuCtrl.Instance.SelectDis(2,1);
					}else{
						CreateMenuCtrl.Instance.SelectDis(2,0);
					}
				}
				m_CharacterCreate_DisciplinesName2Text[0].Hide(true);
				m_CharacterCreate_DisciplinesName2Text[1].Hide(true);
				m_CharacterCreate_DisciplinesName2Text[2].Hide(false);
				m_CharacterCreate_DisciplinesName1Text[0].SetColor(_UI_Color.Instance.color4);
				m_CharacterCreate_DisciplinesName1Text[1].SetColor(_UI_Color.Instance.color4);
				m_CharacterCreate_DisciplinesName1Text[2].SetColor(_UI_Color.Instance.color1);
			break;
		default:
			break;	
		}
	}
}
