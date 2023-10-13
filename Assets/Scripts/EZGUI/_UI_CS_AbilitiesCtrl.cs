using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class _UI_CS_AbilitiesCtrl : MonoBehaviour {
	
	public List<_UI_CS_AbilitiesItem> 	m_AbilitiesProwess   = new List<_UI_CS_AbilitiesItem>();
	public List<_UI_CS_AbilitiesItem> 	m_AbilitiesFortitude = new List<_UI_CS_AbilitiesItem>();
	public List<_UI_CS_AbilitiesItem> 	m_AbilitiesCunning   = new List<_UI_CS_AbilitiesItem>();
	
	public List<_UI_CS_AbilitiesItemEx> 	m_AbilitiesProwessEx   = new List<_UI_CS_AbilitiesItemEx>();
	public List<_UI_CS_AbilitiesItemEx> 	m_AbilitiesFortitudeEx = new List<_UI_CS_AbilitiesItemEx>();
	public List<_UI_CS_AbilitiesItemEx> 	m_AbilitiesCunningEx   = new List<_UI_CS_AbilitiesItemEx>();
	
	public int						[] 	m_EachDisciplineCount;
	//private _UI_CS_AbilitiesItem tempAItem = new _UI_CS_AbilitiesItem();
	
	public _UI_CS_UseAbilities		[]  m_UseAbilities;
	public int 		  				 	m_UseAbilitiesCount;
	
	public int 		   					m_UseAbilitiesGroupIndex;
	
	public UIScrollList 			[] 	m_IngameMenu_AbilitiesList;
	public UIListItemContainer 			m_IngameMenu_AbilitiesItemContainer;
	private UIScrollList 				m_IngameMenu_AbilitiesTempList;
	
	private int 						m_AbilitiesCurrentIdx;
	
	public Rect 					 	m_rect;
	
	public static _UI_CS_AbilitiesCtrl Instance;
	
	public int m_UseAbilitiesGroupIndex_Ingame = 0;
	
	void Awake()
	{
		Instance = this;
	}
	
	// Use this for initialization
	void Start () {
		m_AbilitiesCurrentIdx = 0;
		m_rect.width = 1;
		m_rect.height = 1;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	//初始化技能项
	public void InitAbilitiesItem(){
		
	}
	
	//初始化技能列表-
	public void InitAbilitiesList(){
		int n,m;
	
		for(int i = 0;i<3;i++){	
			if(0 == i){
				m_EachDisciplineCount[i] = m_AbilitiesProwess.Count;
			}else if(1 == i){
				m_EachDisciplineCount[i] = m_AbilitiesFortitude.Count;
			}else if(2 == i){
				m_EachDisciplineCount[i] = m_AbilitiesCunning.Count;
			}
		}
		
		for(int i = 0;i<3;i++){
			n =	m_EachDisciplineCount[i]/4;
			m = m_EachDisciplineCount[i]%4;
			if(m > 0)
				n++;
			
			for(int j =0;j<n;j++){	
				
				if(j+1 == n){
					AddAbilitiesListChild(i,m);
				}else{
					AddAbilitiesListChild(i,4);
				}	
			}
		}
		
		Calculate(0);
		Calculate(1);
		Calculate(2);
	}
	
	public void AddAbilitiesItem(int abilitiesID,int level){
		
		_UI_CS_AbilitiesItem tempAItem = new _UI_CS_AbilitiesItem();
		int index;
		
		int aID = (abilitiesID/100) * 100 + 1;
		int aLevel = abilitiesID % 100;
		
		for(index = 0; index < AbilityInfo.Instance.PlayerAbilityPool.Count;index++){
			AbilityBaseState ability = AbilityInfo.Instance.PlayerAbilityPool[index];
			if(ability.id == aID){
				tempAItem.m_AbilitieID = abilitiesID;//(int)ability.id;
				tempAItem.m_type       = (int)ability.Info.Discipline;
				tempAItem.m_name       = ability.name;
                tempAItem.m_details1   = ability.Info.Description1;
                tempAItem.m_details2   = ability.Info.Description1;
                tempAItem.m_details3   = ability.Info.Description1;
                tempAItem.m_details4   = ability.Info.Description1;
				tempAItem.m_Cooldown   = ability.Info.CoolDown;
				tempAItem.m_EnergyCost = ability.Info.ManaCost;
				tempAItem.m_level 	   = aLevel;
				
				switch(tempAItem.m_type){
				case 1:
						m_AbilitiesProwess.Add(tempAItem);
						break;
				case 2:
						m_AbilitiesFortitude.Add(tempAItem);
						break;
				case 4:
						m_AbilitiesCunning.Add(tempAItem);
						break;
					default:
						break;
				}
				return;
			}else{
				LogManager.Log_Warn("Service send unKnow Skill id"+abilitiesID);
			}
		}
	}
	
	//返回当前技能ID
	//idx 当前技能槽索引
	public _UI_CS_UseAbilities GetUseAbilitiesID(int idx){

		int tempIdx = idx + m_UseAbilitiesGroupIndex_Ingame * 3;
		
		if( true == m_UseAbilities[tempIdx].m_isEmpty)
			return null;
		
		return m_UseAbilities[tempIdx];
	}

	public void AddAbilitiesListChild(int DisciplinesIdx,int childCount){
		
		UIListItemContainer item;
		int i = 0;
			
		switch(DisciplinesIdx){
		//Prowess
		case 0:
			item = (UIListItemContainer)m_IngameMenu_AbilitiesList[0].CreateItem((GameObject)m_IngameMenu_AbilitiesItemContainer.gameObject);
			//Reset after manipulations
			m_IngameMenu_AbilitiesList[0].clipContents = true;
			m_IngameMenu_AbilitiesList[0].clipWhenMoving = true;
			//Calculate(0);
			 
			for(i = 0;i<4;i++){
				if(childCount > i){
					item.transform.GetComponent<_UI_CS_RawItemCtrl>().m_raw[i].transform.GetComponent<_UI_CS_AbilitiesItemEx>().m_abilitiesInfo = m_AbilitiesProwess[m_AbilitiesCurrentIdx];
					item.transform.GetComponent<_UI_CS_RawItemCtrl>().m_raw[i].transform.GetComponent<_UI_CS_AbilitiesItemEx>().m_ListID = m_AbilitiesCurrentIdx;
					item.transform.GetComponent<_UI_CS_RawItemCtrl>().m_raw[i].SetUVs(m_rect);
//					item.transform.GetComponent<_UI_CS_RawItemCtrl>().m_raw[i].SetTexture(
//				    _UI_CS_Resource.Instance.m_AbilitiesIcon[m_AbilitiesProwess[m_AbilitiesCurrentIdx].m_iconID]
//					                                                                               );
					item.transform.GetComponent<_UI_CS_RawItemCtrl>().m_raw[i].SetTexture(
				    AbilityInfo.Instance.GetAbilityByID((uint)m_AbilitiesProwess[m_AbilitiesCurrentIdx].m_AbilitieID/100*100+1).icon
					                                                                               );
					m_AbilitiesCurrentIdx++;
					if(m_AbilitiesCurrentIdx >=  m_AbilitiesProwess.Count){
						m_AbilitiesCurrentIdx = 0;
						//LogManager.Log_Debug("m_AbilitiesCurrentIdx >=  m_AbilitiesProwess.Count");
					}
					
				m_AbilitiesProwessEx.Add(item.transform.GetComponent<_UI_CS_RawItemCtrl>()
					.m_raw[i].transform.GetComponent<_UI_CS_AbilitiesItemEx>());
				}else{
					item.transform.GetComponent<_UI_CS_RawItemCtrl>().m_raw[i].transform.GetComponent<UIButton>().controlIsEnabled = false;
					Destroy (item.transform.GetComponent<_UI_CS_RawItemCtrl>().m_raw[i].transform.GetComponent<MeshRenderer>());
				}
			}
			
			break;
		//Fortitude
		case 1:
			item = (UIListItemContainer)m_IngameMenu_AbilitiesList[1].CreateItem((GameObject)m_IngameMenu_AbilitiesItemContainer.gameObject);			
			//Reset after manipulations
			m_IngameMenu_AbilitiesList[1].clipContents = true;
			m_IngameMenu_AbilitiesList[1].clipWhenMoving = true;
			//Calculate(1);
			
			for(i = 0;i<4;i++){
				if(childCount > i){
					item.transform.GetComponent<_UI_CS_RawItemCtrl>()
					.m_raw[i].transform.GetComponent<_UI_CS_AbilitiesItemEx>()
					.m_abilitiesInfo = m_AbilitiesFortitude[m_AbilitiesCurrentIdx];
					item.transform.GetComponent<_UI_CS_RawItemCtrl>()
					.m_raw[i].transform.GetComponent<_UI_CS_AbilitiesItemEx>()
					.m_ListID = m_AbilitiesCurrentIdx;
					item.transform.GetComponent<_UI_CS_RawItemCtrl>().m_raw[i].SetUVs(m_rect);
//					item.transform.GetComponent<_UI_CS_RawItemCtrl>().m_raw[i].SetTexture(
//				    _UI_CS_Resource.Instance.m_AbilitiesIcon[m_AbilitiesFortitude[m_AbilitiesCurrentIdx].m_iconID]
//					                                                                               );
					item.transform.GetComponent<_UI_CS_RawItemCtrl>().m_raw[i].SetTexture(
			        AbilityInfo.Instance.GetAbilityByID((uint)m_AbilitiesFortitude[m_AbilitiesCurrentIdx].m_AbilitieID/100*100+1).icon
					                                                                               );
					
					
					m_AbilitiesCurrentIdx++;
					if(m_AbilitiesCurrentIdx >=  m_AbilitiesFortitude.Count){
						m_AbilitiesCurrentIdx = 0;
						//LogManager.Log_Debug("m_AbilitiesCurrentIdx >=  Fortitude.Count");
					}
					
				m_AbilitiesFortitudeEx.Add(item.transform.GetComponent<_UI_CS_RawItemCtrl>()
					.m_raw[i].transform.GetComponent<_UI_CS_AbilitiesItemEx>());
				}else{
					item.transform.GetComponent<_UI_CS_RawItemCtrl>().m_raw[i].transform.GetComponent<UIButton>().controlIsEnabled = false;
					Destroy (item.transform.GetComponent<_UI_CS_RawItemCtrl>().m_raw[i].transform.GetComponent<MeshRenderer>());
				}
			}
			
			break;
		//Cunning
		case 2:
			item = (UIListItemContainer)m_IngameMenu_AbilitiesList[2].CreateItem((GameObject)m_IngameMenu_AbilitiesItemContainer.gameObject);
			//Reset after manipulations
			m_IngameMenu_AbilitiesList[2].clipContents = true;
			m_IngameMenu_AbilitiesList[2].clipWhenMoving = true;
			//Calculate(2);
			
			for( i = 0;i<4;i++){
				if(childCount > i){
					item.transform.GetComponent<_UI_CS_RawItemCtrl>()
					.m_raw[i].transform.GetComponent<_UI_CS_AbilitiesItemEx>()
					.m_abilitiesInfo = m_AbilitiesCunning[m_AbilitiesCurrentIdx];
					item.transform.GetComponent<_UI_CS_RawItemCtrl>()
					.m_raw[i].transform.GetComponent<_UI_CS_AbilitiesItemEx>()
					.m_ListID = m_AbilitiesCurrentIdx;
					item.transform.GetComponent<_UI_CS_RawItemCtrl>().m_raw[i].SetUVs(m_rect);
//					item.transform.GetComponent<_UI_CS_RawItemCtrl>().m_raw[i].SetTexture(
//				    _UI_CS_Resource.Instance.m_AbilitiesIcon[m_AbilitiesCunning[m_AbilitiesCurrentIdx].m_iconID]
//					                                                                               );
					item.transform.GetComponent<_UI_CS_RawItemCtrl>().m_raw[i].SetTexture(
					AbilityInfo.Instance.GetAbilityByID((uint)m_AbilitiesCunning[m_AbilitiesCurrentIdx].m_AbilitieID/100*100+1).icon
					                                                                               );
					m_AbilitiesCurrentIdx++;
					if(m_AbilitiesCurrentIdx >=  m_AbilitiesCunning.Count){
						m_AbilitiesCurrentIdx = 0;
						//LogManager.Log_Debug("m_AbilitiesCurrentIdx >=  m_AbilitiesCunning.Count");
					}
					
				m_AbilitiesCunningEx.Add(item.transform.GetComponent<_UI_CS_RawItemCtrl>()
					.m_raw[i].transform.GetComponent<_UI_CS_AbilitiesItemEx>());
				}else{
					item.transform.GetComponent<_UI_CS_RawItemCtrl>().m_raw[i].transform.GetComponent<UIButton>().controlIsEnabled = false;
					Destroy (item.transform.GetComponent<_UI_CS_RawItemCtrl>().m_raw[i].transform.GetComponent<MeshRenderer>());
				}
			}
			
			break;
		default:
			break;
		}
		
	}
	
	public void Calculate(int DisciplinesIdx)
	{
		switch(DisciplinesIdx){
		//Prowess
		case 0:
			m_IngameMenu_AbilitiesTempList = m_IngameMenu_AbilitiesList[0];
			break;
		//Fortitude
		case 1:
			m_IngameMenu_AbilitiesTempList = m_IngameMenu_AbilitiesList[1];
			break;
		//Cunning
		case 2:
			m_IngameMenu_AbilitiesTempList = m_IngameMenu_AbilitiesList[2];
			break;
		default:
			break;
		}
		
		if (m_IngameMenu_AbilitiesTempList != null && m_IngameMenu_AbilitiesTempList.slider != null)
        {

            // Ask scroll list to position items
            m_IngameMenu_AbilitiesTempList.PositionItems();

            // Var to hold new knob size
            Vector2 newKnobSize;

            // Determine the new knob size as a percentage of the size of the viewable area
            // If the content is smaller than the viewable size then we won't show a knob
            if (m_IngameMenu_AbilitiesTempList.ContentExtents > m_IngameMenu_AbilitiesTempList.viewableArea.y)
            {
                float ratio = m_IngameMenu_AbilitiesTempList.ContentExtents / m_IngameMenu_AbilitiesTempList.viewableArea.y;
                newKnobSize = new Vector2((m_IngameMenu_AbilitiesTempList.viewableArea.y / ratio), m_IngameMenu_AbilitiesTempList.slider.knobSize.y);
				m_IngameMenu_AbilitiesTempList.slider.Hide(false);
            }
            else
            {
                newKnobSize = new Vector2(0f, 0f);
				m_IngameMenu_AbilitiesTempList.slider.Hide(true);
            }

            // Get a handle to the knob so we can change it
            UIScrollKnob theKnob = m_IngameMenu_AbilitiesTempList.slider.GetKnob();
            //Debug.Log(theKnob);
            // Set the knob size based on our previous calculation
            theKnob.SetSize(newKnobSize.x, newKnobSize.y);

            // Now we need to make sure the knob doesn't go past the ends of the scrollview window size
            m_IngameMenu_AbilitiesTempList.slider.stopKnobFromEdge = newKnobSize.x / 2;
            //Vector3 newStartPos = m_IngameMenu_AbilitiesTempList.slider.CalcKnobStartPos();
			Vector3 newStartPos = m_IngameMenu_AbilitiesTempList.slider.CalcKnobStartPos();
            theKnob.SetStartPos(newStartPos);
            theKnob.SetMaxScroll(m_IngameMenu_AbilitiesTempList.slider.width - (m_IngameMenu_AbilitiesTempList.slider.stopKnobFromEdge * 2f));

            // Make sure the new text is scrolled to the top of the viewable area
            m_IngameMenu_AbilitiesTempList.ScrollListTo(0f);
            // Added by me.
            theKnob.SetPosition(0f);
        }
	}
	
	public int FindAbilitiesSlot(){
		
		int index = m_UseAbilitiesGroupIndex*3; 
		
		for(int i = index;i<index+3;i++){
			if(m_UseAbilities[i].m_isEmpty == true){
				return i;
			}
		}	
		return -1;
	}
	
	public void UpDateIngameAbilitiesIcon(){
		
		int index = m_UseAbilitiesGroupIndex_Ingame*3;
		int IconId;
		
		for(int i = 0;i<3;i++){
			
			if(m_UseAbilities[index+i].m_isEmpty == false){
				//IconId = m_UseAbilities[index+i].m_abilitiesInfo.m_iconID;
				_UI_CS_FightScreen.Instance.m_IngameNormal_AbilitiesBtn[i].SetUVs(m_rect);
				_UI_CS_FightScreen.Instance.m_IngameNormal_AbilitiesBtn[i].SetTexture(
				AbilityInfo.Instance.GetAbilityByID((uint)m_UseAbilities[index+i].m_abilitiesInfo.m_AbilitieID/100*100+1).icon                                                                      
				                                                                      );
				
				_UI_CS_FightScreen.Instance.m_abiMaskEffest[i].InitMesh();
				_UI_CS_FightScreen.Instance.m_abiMaskEffest[i].StartEffect(GetUseAbilitiesID(i).m_coolDownTime);
				
			}else{
				_UI_CS_FightScreen.Instance.m_IngameNormal_AbilitiesBtn[i].SetUVs(m_rect);
				_UI_CS_FightScreen.Instance.m_IngameNormal_AbilitiesBtn[i].SetTexture(_UI_CS_Resource.Instance.m_EquipmentIcon[0]);
			}
		}
	}
	
	public void AddUseAbilitiesItem(int abilitiesID,int idx){
		
		_UI_CS_AbilitiesItem tempAItem = new _UI_CS_AbilitiesItem();
		int index;
		int aID = (abilitiesID/100) * 100 + 1;
		int aLevel = abilitiesID % 100;
//		string fileAddress = Application.dataPath  + "//AbilitiesItem.AbilitiesItemSolt.txt";
//      	
//		 if (!File.Exists(fileAddress)) {
//			LogManager.Log_Debug("fileAddress not Exists:"+fileAddress);
//			 return;
//		 }
//		
//		StreamReader  sReader = new StreamReader(fileAddress);
//		
//		if(sReader != null){
//		   string pp = sReader.ReadLine();
//		   while (pp != null){
//             if(pp.Contains(abilitiesID.ToString())) {
//			   string[] vals = pp.Split(new char[] { '	', '	' });	 
//				//abi id
//			    index = int.Parse(vals[0]);	
//				tempAItem.m_AbilitieID      = 	index;	
//				//type id
//			    index = int.Parse(vals[1]);	
//				tempAItem.m_type      = 	index;
//				//name	
//				tempAItem.m_name      = 	vals[2];
//			    //icon id
//			    index = int.Parse(vals[3]);	
//				tempAItem.m_iconID    = 	index;
//				//Details	
//				tempAItem.m_details   = 	vals[4];
//				//Energy Cost
//				 index = int.Parse(vals[5]);	
//				tempAItem.m_EnergyCost    		= 	index;
//				//Ability Duration for Akaneiro
//				 index = int.Parse(vals[6]);	
//				tempAItem.m_AbilityDuration    	= 	index;
//				//Status Effect 1
//				 index = int.Parse(vals[7]);	
//				tempAItem.m_StatusEffect    	= 	index;
//				//Cooldown
//				index = int.Parse(vals[8]);	
//				tempAItem.m_Cooldown    		= 	index;
//				//Range
//				index = int.Parse(vals[9]);	
//				tempAItem.m_Range    			= 	index;	
//				//Dmg 2 (max)
//				index = int.Parse(vals[10]);	
//				tempAItem.m_Dmg2_Max    		= 	index;				
//				break;
//			 }
//                pp = sReader.ReadLine();
//            }
//		    sReader.Close();
//		}
///////////////////////////////////////////////////////////////////////////////////////////////////////////////		
//		TextAsset item = (TextAsset)Resources.Load("AbilitiesItem.AbilitiesItemSolt", typeof(TextAsset));
//
//		string[] itemRowsList = item.text.Split('\n');
//
//		//Skip first three lines.
//		for (int i = 0; i < itemRowsList.Length; ++i)
//		{
//			 string pp = itemRowsList[i];
//			if(pp.Contains(abilitiesID.ToString())) {
//				if(pp.Contains(abilitiesID.ToString())) {
//			   	string[] vals = pp.Split(new char[] { '	', '	' });	 
//				//abi id
//			    index = int.Parse(vals[0]);	
//				tempAItem.m_AbilitieID      = 	index;	
//				//type id
//			    index = int.Parse(vals[1]);	
//				tempAItem.m_type      = 	index;
//				//name	
//				tempAItem.m_name      = 	vals[2];
//			    //icon id
//			    index = int.Parse(vals[3]);	
//				tempAItem.m_iconID    = 	index;
//				//tempAItem.m_iconButton.SetUVs(m_rect);
//				//tempAItem.m_iconButton.SetTexture(_UI_CS_Resource.Instance.m_AbilitiesIcon[index]);
//				//Details	
//				tempAItem.m_details   = 	vals[4];
//				//Energy Cost
//				 index = int.Parse(vals[5]);	
//				tempAItem.m_EnergyCost    		= 	index;
//				//Ability Duration for Akaneiro
//				 index = int.Parse(vals[6]);	
//				tempAItem.m_AbilityDuration    	= 	index;
//				//Status Effect 1
//				 index = int.Parse(vals[7]);	
//				tempAItem.m_StatusEffect    	= 	index;
//				//Cooldown
//				index = int.Parse(vals[8]);	
//				tempAItem.m_Cooldown    		= 	index;
//				//Range
//				index = int.Parse(vals[9]);	
//				tempAItem.m_Range    			= 	index;	
//				//Dmg 2 (max)
//				index = int.Parse(vals[10]);	
//				tempAItem.m_Dmg2_Max    		= 	index;		
//				}
//			}
//		}
		
//		for(index = 0; index < AbilityInfo.Instance.PlayerAbilityPool.Count;index++){
//		//for(index = 0;  index < Player.Instance.AbilityInstanceList.Count; index++){
//			AbilityBaseState ability = AbilityInfo.Instance.PlayerAbilityPool[index];
//			//AbilityBaseState ability = Player.Instance.AbilityInstanceList[index].GetComponent<AbilityBaseState>();
//			if(ability.id == aID){
//				tempAItem.m_AbilitieID = abilitiesID;//(int)ability.id;
//				tempAItem.m_type       = (int)ability.DisciplineType;
//				tempAItem.m_name       = ability.name;
//				tempAItem.m_details    = ability.description;
//				tempAItem.m_Cooldown   = ability.AbilityCoolDown;
//				tempAItem.m_level 	   = aLevel;
//				tempAItem.m_EnergyCost = ability.EnergyCost;
//				break;
//			}
//		}
//		
//		m_UseAbilities[idx].m_abilitiesInfo = tempAItem;
//		m_UseAbilities[idx].m_isEmpty = false;
//		m_UseAbilities[idx].m_iconBtn.SetUVs(m_rect);
////		m_UseAbilities[idx].m_iconBtn.SetTexture(_UI_CS_Resource.Instance.m_AbilitiesIcon[tempAItem.m_iconID]);
//		m_UseAbilities[idx].m_iconBtn.SetTexture(AbilityInfo.Instance.GetAbilityByID((uint)tempAItem.m_AbilitieID/100*100+1).icon);
//		m_UseAbilities[idx].m_level.Text = tempAItem.m_level.ToString();
//		m_UseAbilities[idx].m_name.Text  = tempAItem.m_name;
//		//<listID 为 全技能列表的对饮对应关系，目前可不填写,因为目前之关联是否使用。而那变量目前无作用>
//		//m_UseAbilities[idx].m_ListID     = m_ListID;
		
		
	}
	
	public void RefreshAllAbilitiesInfo()
	{
		foreach(_UI_CS_UseAbilities uiab in m_UseAbilities)
		{
			if(uiab)
			{
				foreach(AbilityBaseState abi in Player.Instance.abilityManager.Abilities)
				{
					if(uiab.m_abilitiesInfo != null)
					{
						if(uiab.m_abilitiesInfo.m_AbilitieID/100*100+1 == abi.id)
						{
							uiab.m_abilitiesInfo.m_type        =  (int)abi.Info.Discipline;
							uiab.m_abilitiesInfo.m_name        = abi.name;
                            uiab.m_abilitiesInfo.m_details1    = abi.Info.Description1;
                            uiab.m_abilitiesInfo.m_details2    = abi.Info.Description1;
                            uiab.m_abilitiesInfo.m_details3    = abi.Info.Description1;
                            uiab.m_abilitiesInfo.m_details4    = abi.Info.Description1;
							uiab.m_abilitiesInfo.m_Cooldown    = abi.Info.CoolDown;
							uiab.m_iconBtn.SetTexture(abi.icon);
//							uiab.m_name.Text  = abi.name;
							break;
						}
					}
				}
			}
		}
	}
}
