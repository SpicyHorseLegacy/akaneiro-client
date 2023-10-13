using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class _UI_CS_AbilityTrainer_Mastery : _UI_CS_AbilitiesTrainer_Base {    

    public List<UIListItemContainer> items;

    protected virtual void Awake(){}

    // Use this for initialization
    void Start()
    {
        m_AbilitiesTrainerAllCurrentIdx = 0;
        m_rect.width = 1;
        m_rect.height = 1;
    } 

    public virtual void ResetAllItems()
    {
        m_List.ClearList(true);
        items.Clear();

        Calculate();
    }

    void deleteUIItem(int discipline)
    {
        if (items.Count > 0)
        {
            for (int i = items.Count - 1; i >= 0; i--)
            {
                UIListItemContainer item = items[i];
                _UI_CS_AbilitiesTrainerItem AAInfo = item.transform.GetComponent<_UI_CS_AbilitiesTrainerRawItemCtrl>().item[0].m_AAInfo;
                if (discipline == AAInfo.m_type)
                {
                    Destroy(item.gameObject);
                    items.RemoveAt(i);
                }
            }
        }
    }

    public void UpdateMasteryInfoByID(SingleMastery mastery)
    {
        foreach (UIListItemContainer item in items)
        {
            _UI_CS_AbilitiesTrainerItem AAInfo = item.transform.GetComponent<_UI_CS_AbilitiesTrainerRawItemCtrl>().item[0].m_AAInfo;
			SingleMasteryInfo masteryInfo = mastery.NextInfo;
            bool isMaxLevel = false;
			if(mastery.Info != null && mastery.Info.MasteryLevel >= MasteryInfo.MAXLEVEL)
			{
				masteryInfo = mastery.Info;
                isMaxLevel = true;
			}
			if(AAInfo == null)
			{
				AAInfo = new _UI_CS_AbilitiesTrainerItem((int)masteryInfo.Discipline);
				item.transform.GetComponent<_UI_CS_AbilitiesTrainerRawItemCtrl>().item[0].m_AAInfo = AAInfo;
			}
			
            if ((int)masteryInfo.Discipline == AAInfo.m_type)
            {
				AAInfo.m_name = masteryInfo.Name;
		        AAInfo.m_IsAbilityOrMastery = 1;
				AAInfo.m_MasteryClass = (int)masteryInfo.Class.Get();
		        AAInfo.m_Des1 = masteryInfo.Description;
		        AAInfo.m_Des2 = masteryInfo.Description2;
		        AAInfo.m_level = masteryInfo.MasteryLevel;
		        AAInfo.m_type = (int)masteryInfo.Discipline;
		        AAInfo.m_icon = mastery.Icon;
		        AAInfo.m_skVal = masteryInfo.karmaCost;
		        AAInfo.m_playerLevel = masteryInfo.BaseStateLvNeeded;
                AAInfo.m_isMaxLevel = isMaxLevel;

                Color _bgColor = _UI_Color.Instance.color8;
                if (AAInfo.m_isMaxLevel)
                {
                    AAInfo.m_name += " (MAX)";
                    _bgColor = Color.gray;
                }
				
        		item.transform.GetComponent<_UI_CS_AbilitiesTrainerRawItemCtrl>().item[0].m_AIconButton.SetTexture(mastery.Icon);
                item.transform.GetComponent<_UI_CS_AbilitiesTrainerRawItemCtrl>().item[0].m_BgIconButton.SetColor(_bgColor);
                item.transform.GetComponent<_UI_CS_AbilitiesTrainerRawItemCtrl>().item[0].m_NameText.Text = AAInfo.m_name;

                string _karmaString = "---";
                if (!AAInfo.m_isMaxLevel && AAInfo.m_skVal > 0)
                {
                    _karmaString = AAInfo.m_skVal.ToString();
                }

                item.transform.GetComponent<_UI_CS_AbilitiesTrainerRawItemCtrl>().item[0].m_ValText.Text = _karmaString;
                return;
            }
        }
		// if there isn't a mastery container, create one.
        AddAbilitiesTrainerAllListChild(mastery);
    }

    void AddAbilitiesTrainerAllListChild(SingleMastery mastery)
    {
        UIListItemContainer item = (UIListItemContainer)m_List.CreateItem((GameObject)m_ItemContainer.gameObject);
        _UI_CS_AbilitiesTrainer_AllAItemEx itemEx = (_UI_CS_AbilitiesTrainer_AllAItemEx)item.transform.GetComponent<_UI_CS_AbilitiesTrainerRawItemCtrl>().item[0];
        items.Add(item);
		
        //Reset after manipulations
        m_List.clipContents = true;
        m_List.clipWhenMoving = true;
        Calculate();
		
		UpdateMasteryInfoByID(mastery);
	}
}
