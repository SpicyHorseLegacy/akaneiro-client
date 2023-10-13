using UnityEngine;
using System.Collections;

public class CraftingPanel : MonoBehaviour 
{
	static public CraftingPanel Instance = null;
	private UIPanel myPanel = null;
	[SerializeField]
	private UIPanel upgradePanel = null;
	[SerializeField]
	private GameObject bagPanelPos = null;
	[SerializeField]
	private UIButton backBtn = null;
	
	// Use this for initialization
	void Awake () 
	{
		Instance = this;
		myPanel = GetComponent<UIPanel>();
		backBtn.AddInputDelegate(BackBtnDelegate);
	}
	
	public void AwakeCrafting()
	{
		_UI_CS_ScreenCtrl.Instance.SetNextScreenType(_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_CRAFTING);
		myPanel.BringIn();
		StartCoroutine(BringInInv());
		MoneyBadgeInfo.Instance.Hide(false);
		Player.Instance.FreezePlayer();
		CraftingWeaponPanel.Instance.Init();
	}
	
	public IEnumerator BringInInv()
	{
		upgradePanel.Dismiss();
		yield return null;
		_UI_CS_BagCtrl.Instance.ob.transform.position = bagPanelPos.transform.position;
	}
	
	public void DissmissInv()
	{
		upgradePanel.BringIn();
		_UI_CS_BagCtrl.Instance.Hide(true);
	}
	
	void BackBtnDelegate(ref POINTER_INFO ptr)
	{
		switch(ptr.evt)
		{
		   case POINTER_INFO.INPUT_EVENT.TAP:
				_UI_CS_ScreenCtrl.Instance.SetNextScreenType(_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_INGAME_NORMAL);
				_UI_CS_FightScreen.Instance.m_CS_Ingame_NormalPanel.BringIn();
				
				myPanel.Dismiss();
				DissmissInv();

                Player.Instance.ReactivePlayer();
                GameCamera.BackToPlayerCamera();
				break;
		}	
	}
}
