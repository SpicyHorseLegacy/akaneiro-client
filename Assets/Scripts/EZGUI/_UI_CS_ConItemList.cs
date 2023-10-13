using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class _UI_CS_ConItemList : MonoBehaviour {

	public  UIListItemContainer  				ItemContainer;
	public  List<ItemDropStruct> 				InfoList   = new List<ItemDropStruct>();
	public  UIScrollList						List;
	private int 								CurrentIdx;
	private Rect 					 			rect;
	public  int 								count;
	
	void Awake(){
	}
	
	// Use this for initialization
	void Start () {
		CurrentIdx 	= 0;
		rect.x 		= 0;
		rect.y 		= 0;
		rect.width  = 1;
		rect.height = 1;
		count 		= 0;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void AddElement(ItemDropStruct element){
		ItemDropStruct temp = new ItemDropStruct();
		temp = element;
		InfoList.Add(temp);
	}
	
	public void ClearList(){
		InfoList.Clear();
		List.ClearList(true);
	}
	
	//init list
	public void InitItemList(){
		int n,m;
		count = InfoList.Count;
		n =	count/3;
		m = count%3;
		if(m > 0){
			n++;
		}
		for(int j =0;j<n;j++){	
			if(m > 0){
				if(j+1 == n){
					AddItemListChild(m);
				}else{
					AddItemListChild(3);
				}	
			}else{
				AddItemListChild(3);
			}
		}
	}
	
	public void AddItemListChild(int childCount){
		UIListItemContainer item;
		item = (UIListItemContainer)List.CreateItem((GameObject)ItemContainer.gameObject);
		//Reset after manipulations
		List.clipContents 		= true;
		List.clipWhenMoving 	= true;
		Calculate();
		for(int i = 0;i<3;i++){
			if(childCount > i){
				item.transform.GetComponent<_UI_CS_ItemRawCtrl_Consumable>().item[i].ItemInfo 		= InfoList[CurrentIdx];
				item.transform.GetComponent<_UI_CS_ItemRawCtrl_Consumable>().item[i].ListID 		= CurrentIdx;
				item.transform.GetComponent<_UI_CS_ItemRawCtrl_Consumable>().item[i].ValText.Text 	= InfoList[CurrentIdx]._SaleVal.ToString();
				item.transform.GetComponent<_UI_CS_ItemRawCtrl_Consumable>().item[i].ItemIcon.SetUVs(rect);
				ItemPrefabs.Instance.GetItemIcon(
				                                       item.transform.GetComponent<_UI_CS_ItemRawCtrl_Consumable>().item[i].ItemInfo._ItemID,                                                                                
				                                       item.transform.GetComponent<_UI_CS_ItemRawCtrl_Consumable>().item[i].ItemInfo._TypeID,
				                                       item.transform.GetComponent<_UI_CS_ItemRawCtrl_Consumable>().item[i].ItemInfo._PrefabID,
													   item.transform.GetComponent<_UI_CS_ItemRawCtrl_Consumable>().item[i].ItemIcon
												);
				if(InfoList[CurrentIdx]._isUseRealMoney == 1){
					 item.transform.GetComponent<_UI_CS_ItemRawCtrl_Consumable>().item[i].SpiceBg.gameObject.layer = LayerMask.NameToLayer("Default");
					 item.transform.GetComponent<_UI_CS_ItemRawCtrl_Consumable>().item[i].CpiceBg.gameObject.layer = LayerMask.NameToLayer("EZGUI_CanTouch");
				}else{
					 item.transform.GetComponent<_UI_CS_ItemRawCtrl_Consumable>().item[i].SpiceBg.gameObject.layer = LayerMask.NameToLayer("EZGUI_CanTouch");
					 item.transform.GetComponent<_UI_CS_ItemRawCtrl_Consumable>().item[i].CpiceBg.gameObject.layer = LayerMask.NameToLayer("Default");
				}
				CurrentIdx++;
				if(CurrentIdx >=  InfoList.Count){
					CurrentIdx = 0;
				}
			}else{
				item.transform.GetComponent<_UI_CS_ItemRawCtrl_Consumable>().item[i].transform.position = 
					new Vector3(item.transform.GetComponent<_UI_CS_ItemRawCtrl_Consumable>().item[i].transform.position.x,item.transform.GetComponent<_UI_CS_ItemRawCtrl_Consumable>().item[i].transform.position.y,20f);
			}
		}
	}
	
	public void Calculate()
	{
		if (List != null && List.slider != null)
        {
            // Ask scroll list to position items
            List.PositionItems();

            // Var to hold new knob size
            Vector2 newKnobSize;

            // Determine the new knob size as a percentage of the size of the viewable area
            // If the content is smaller than the viewable size then we won't show a knob
            if (List.ContentExtents > List.viewableArea.y)
            {
                float ratio = List.ContentExtents / List.viewableArea.y;
                newKnobSize = new Vector2((List.viewableArea.y / ratio), List.slider.knobSize.y);
				List.slider.Hide(false);
            }
            else
            {
                newKnobSize = new Vector2(0f, 0f);
				List.slider.Hide(true);
            }

            // Get a handle to the knob so we can change it
            UIScrollKnob theKnob = List.slider.GetKnob();
            //Debug.Log(theKnob);
            // Set the knob size based on our previous calculation
            theKnob.SetSize(newKnobSize.x, newKnobSize.y);

            // Now we need to make sure the knob doesn't go past the ends of the scrollview window size
            List.slider.stopKnobFromEdge = newKnobSize.x / 2;
            //Vector3 newStartPos = m_IngameMenu_AbilitiesTempList.slider.CalcKnobStartPos();
			Vector3 newStartPos = List.slider.CalcKnobStartPos();
            theKnob.SetStartPos(newStartPos);
            theKnob.SetMaxScroll(List.slider.width - (List.slider.stopKnobFromEdge * 2f));

            // Make sure the new text is scrolled to the top of the viewable area
            List.ScrollListTo(0f);
            // Added by me.
            theKnob.SetPosition(0f);
        }
	}
}
