using UnityEngine;
using System.Collections;

public class _UI_CS_Option : MonoBehaviour {
	
	//Instance
	public static _UI_CS_Option Instance = null;
	
	//Options
	public UIButton 	m_Options_CancelBtn;
	public UIButton 	m_Options_SaveBtn;
	public UIPanel		m_OptinsPanel;
	
	void Awake()
	{
		Instance = this;
	}
	
	// Use this for initialization
	void Start () {
		m_Options_CancelBtn.AddInputDelegate(OptionsCancelDelegate);
		m_Options_SaveBtn.AddInputDelegate(OptionsSaveDelegate);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OptionsSaveDelegate(ref POINTER_INFO ptr)
	{
		switch(ptr.evt)
		{
		   case POINTER_INFO.INPUT_EVENT.TAP:
				m_OptinsPanel.Dismiss();
		  	 break;
		   default:
				break;
		}	
	}
		
	void OptionsCancelDelegate(ref POINTER_INFO ptr)
	{
		switch(ptr.evt)
		{
		   case POINTER_INFO.INPUT_EVENT.TAP:
				m_OptinsPanel.Dismiss();
		  	 break;
		   default:
				break;
		}	
	}
	
}
