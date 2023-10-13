using UnityEngine;
using System.Collections;

public class _UI_CS_TestModecItem : MonoBehaviour {
	
	public UIListItem item;
	
	// Use this for initialization
	void Start () {
		item.AddInputDelegate(ItemDelegate);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void ItemDelegate(ref POINTER_INFO ptr)
	{
		switch(ptr.evt)
		{
		   case POINTER_INFO.INPUT_EVENT.TAP:
				
				_UI_CS_TestModeCtrl.Instance.mapName.Text = item.text;

				break;
		   default:
				break;
		}	
	}
}
