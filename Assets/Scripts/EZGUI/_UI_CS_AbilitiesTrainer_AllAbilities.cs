using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class _UI_CS_AbilitiesTrainer_AllAbilities : _UI_CS_AbilitiesTrainer_Base {
	
	public static _UI_CS_AbilitiesTrainer_AllAbilities Instance;
	
	void Awake()
	{
		Instance = this;
	}
	
	// Use this for initialization
	void Start () {
		m_AbilitiesTrainerAllCurrentIdx = 0;
		m_rect.width = 1;
		m_rect.height = 1;
		m_count = 0;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void AddAbilitiesAllListElement(_UI_CS_AbilitiesTrainerItem element){
		_UI_CS_AbilitiesTrainerItem temp = new _UI_CS_AbilitiesTrainerItem();
		temp = element;
		m_AAItemList.Add(temp);
	}
	
	//初始化列表 
	public void InitAbilitiesAllList(){
		
		Calculate();
		
		m_count = m_AAItemList.Count;
		
		for(int j =0;j<m_count;j++){	
			AddAbilitiesTrainerAllListChild(1);
		}
		
	}
	
	public void AddAbilitiesTrainerAllListChild(int childCount){
		
		UIListItemContainer item;

		item = (UIListItemContainer)m_List.CreateItem((GameObject)m_ItemContainer.gameObject);
		//Reset after manipulations
		m_List.clipContents 	= true;
		m_List.clipWhenMoving 	= true;
		Calculate();
		 
		for(int i = 0;i<2;i++){
			if(childCount > i){
				
				item.transform.GetComponent<_UI_CS_AbilitiesTrainerRawItemCtrl>().item[i].m_AAInfo = m_AAItemList[m_AbilitiesTrainerAllCurrentIdx];
				
				item.transform.GetComponent<_UI_CS_AbilitiesTrainerRawItemCtrl>().item[i].transform.GetComponent<_UI_CS_AbilitiesTrainer_AllAItemEx>().m_ListID = m_AbilitiesTrainerAllCurrentIdx;
				
				item.transform.GetComponent<_UI_CS_AbilitiesTrainerRawItemCtrl>().item[i].m_NameText.Text = m_AAItemList[m_AbilitiesTrainerAllCurrentIdx].m_name;
				
				switch(m_AAItemList[m_AbilitiesTrainerAllCurrentIdx].m_type){
				case 1:
					item.transform.GetComponent<_UI_CS_AbilitiesTrainerRawItemCtrl>().item[i].m_BgIconButton.SetColor(_UI_Color.Instance.color14);
					break;
				case 2:
					item.transform.GetComponent<_UI_CS_AbilitiesTrainerRawItemCtrl>().item[i].m_BgIconButton.SetColor(_UI_Color.Instance.color15);
					break;
				case 4:
					item.transform.GetComponent<_UI_CS_AbilitiesTrainerRawItemCtrl>().item[i].m_BgIconButton.SetColor(_UI_Color.Instance.color16);
					break;
				default:
					break;
				}
				
				if(-2 !=  m_AAItemList[m_AbilitiesTrainerAllCurrentIdx].m_skVal && -2 !=  m_AAItemList[m_AbilitiesTrainerAllCurrentIdx].m_fkVal ){
					
					if(-1 !=  m_AAItemList[m_AbilitiesTrainerAllCurrentIdx].m_skVal){
						
						item.transform.GetComponent<_UI_CS_AbilitiesTrainerRawItemCtrl>().item[i].m_ValText.Text = m_AAItemList[m_AbilitiesTrainerAllCurrentIdx].m_skVal.ToString();
						
					}else{
						
						item.transform.GetComponent<_UI_CS_AbilitiesTrainerRawItemCtrl>().item[i].m_ValText.Text = m_AAItemList[m_AbilitiesTrainerAllCurrentIdx].m_fkVal.ToString();
						
					}
					
					item.transform.GetComponent<_UI_CS_AbilitiesTrainerRawItemCtrl>().item[i].m_LevelText.Text = m_AAItemList[m_AbilitiesTrainerAllCurrentIdx].m_level.ToString();
					item.transform.GetComponent<_UI_CS_AbilitiesTrainerRawItemCtrl>().item[i].m_PlayerLevelText.Text = m_AAItemList[m_AbilitiesTrainerAllCurrentIdx].m_playerLevel.ToString();
					
				}else{

//					item.transform.GetComponent<_UI_CS_AbilitiesTrainerRawItemCtrl>().item[i].m_LevelText.Text = "Max";
					LocalizeManage.Instance.GetDynamicText(item.transform.GetComponent<_UI_CS_AbilitiesTrainerRawItemCtrl>().item[i].m_LevelText,"MAX");
//					item.transform.GetComponent<_UI_CS_AbilitiesTrainerRawItemCtrl>().item[i].m_PlayerLevelText.Text = "Max";
					LocalizeManage.Instance.GetDynamicText(item.transform.GetComponent<_UI_CS_AbilitiesTrainerRawItemCtrl>().item[i].m_PlayerLevelText,"MAX");
					
				}
				
//				if(_PlayerData.Instance.playerLevel <= m_AAItemList[m_AbilitiesTrainerAllCurrentIdx].m_playerLevel){
//					item.transform.GetComponent<_UI_CS_AbilitiesTrainerRawItemCtrl>().item[i].m_BgIconButton.SetColor(_UI_Color.Instance.color3);
//					item.transform.GetComponent<_UI_CS_AbilitiesTrainerRawItemCtrl>().item[i].m_BgIconButton.enabled = false;
//				}
				
				item.transform.GetComponent<_UI_CS_AbilitiesTrainerRawItemCtrl>().item[i].m_AIconButton.SetUVs(m_rect);
				item.transform.GetComponent<_UI_CS_AbilitiesTrainerRawItemCtrl>().item[i].m_AIconButton.SetTexture(m_AAItemList[m_AbilitiesTrainerAllCurrentIdx].m_icon);
				
				m_AbilitiesTrainerAllCurrentIdx++;
				if(m_AbilitiesTrainerAllCurrentIdx >=  m_AAItemList.Count){
					m_AbilitiesTrainerAllCurrentIdx = 0;
					
				}

			}else{
//				item.transform.GetComponent<_UI_CS_AbilitiesTrainerRawItemCtrl>()
//					.item[i].transform.position = new Vector3(item.transform.GetComponent<_UI_CS_AbilitiesTrainerRawItemCtrl>().item[i].transform.position.x,
//					                                       item.transform.GetComponent<_UI_CS_AbilitiesTrainerRawItemCtrl>().item[i].transform.position.y,
//					                                        20f);
			}
		}

	}
}
