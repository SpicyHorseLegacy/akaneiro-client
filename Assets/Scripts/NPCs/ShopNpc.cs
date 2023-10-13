using UnityEngine;
using System.Collections;

public class ShopNpc : BaseHitableObject {

	// Use this for initialization
	public float distance = 1.0f;
	
    public NpcInfo.NPCType npcType;
	
	public string npcName = "";
    public Transform ActiveSound;

    public bool OverrideCamera = false;
    public Transform CameraPos;
    public Transform CameraLookPos;
    public float CameraFOV;
	
	void Start () {
		ObjType = ObjectType.NPC;
        if(Player.Instance)
            Player.Instance.AllEnemys.Add(transform);
	}

    void OnDisable()
    {
        if (Player.Instance && Player.Instance.AllEnemys.Contains(transform))
        {
            Player.Instance.AllEnemys.Remove(transform);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
		if(CS_SceneInfo.Instance != null)
		   CS_SceneInfo.Instance.PopNewContent(transform);

	}

	public virtual void PopMenu()
	{
        if ((int)npcType >= (int)NpcInfo.NPCType.NOTHING)	return;
		
		Player.Instance.FreezePlayer();

        if (ActiveSound)
            SoundCue.PlayPrefabAndDestroy(ActiveSound);

        if (npcType == NpcInfo.NPCType.SHOPNPC ||
            npcType == NpcInfo.NPCType.SKILLNPC ||
            npcType == NpcInfo.NPCType.PETNPC ||
            npcType == NpcInfo.NPCType.MAILNPC ||
            npcType == NpcInfo.NPCType.DAYREWARDNPC ||
            npcType == NpcInfo.NPCType.SHOPRARE ||
            npcType == NpcInfo.NPCType.MISSIONPC||
			npcType == NpcInfo.NPCType.STEELNPC ||
			npcType == NpcInfo.NPCType.STASH ||
			npcType == NpcInfo.NPCType.DIALOG)
        {
            GameCamera.Instance.target = transform;
            GameCamera.Instance.ChangeCameraState(GameCamera.Instance.NPC_CS);
        }

		BGMInfo.Instance.isPlayUpGradeEffectSound = false;
#if NGUI
#else
		_UI_CS_ScreenCtrl.Instance.SetNextScreenType(_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_WAIT);
#endif
		StartCoroutine(Wait05Sec());
	}
	
	private IEnumerator Wait05Sec() {
		yield return new WaitForSeconds(0.3f);
		switch(npcType){
            case NpcInfo.NPCType.MISSIONPC:
#if NGUI
			CS_Main.Instance.g_commModule.SendMessage(ProtocolGame_SendRequest.MissionListReq());
#else
				BGManager.Instance.PlayOutsideBGM(MissionPanel.Instance.MissionSound);
                MissionPanel.Instance.AwakeMissionMap();
#endif
                break;
            case NpcInfo.NPCType.SHOPNPC:
#if NGUI
				PlayerDataManager.Instance.ClearInitShopData();
				if(PlayerDataManager.Instance.isDownLoadShopData) {
					PlayerDataManager.Instance.AddInitShopDataFlag();
				}else {
					CS_Main.Instance.g_commModule.SendMessage(ProtocolGame_SendRequest.RequestPlayerShopInfo(false));
				}
				CS_Main.Instance.g_commModule.SendMessage(ProtocolGame_SendRequest.RequestPlayerShopInfo(true));
#else
				_UI_CS_ItemVendor.Instance.UpdateShopList();
#endif
				break;
            case NpcInfo.NPCType.SKILLNPC:
#if NGUI
//                if (InGameScreenShopAbilityCtrl.Instance)
//                {
//                    InGameScreenShopAbilityCtrl.Instance.InitShopAbility();
//                }
				if(GUIManager.Instance != null)
					GUIManager.Instance.ChangeUIScreenState("Shop_Ability_Screen");
#else
				_UI_CS_FightScreen.Instance.m_CS_Ingame_NormalPanel.Dismiss();
//				_UI_CS_AbilitiesTrainer.Instance.InitAbilitiesTrainer();
//				_UI_CS_AbilitiesTrainer.Instance.AwakeAbilitiesTrainer();
                // ask for mastery information from server.//
                CS_Main.Instance.g_commModule.SendMessage(ProtocolGame_SendRequest.RoleMasteryListReq());
				AbilitiesShop.Instance.AwakeAbilitiesShop();
#endif
				break;
            case NpcInfo.NPCType.PETNPC:
				CS_Main.Instance.g_commModule.SendMessage(
			   		ProtocolGame_SendRequest.RolePetListReq()
				);
				break;
            case NpcInfo.NPCType.MAILNPC:
#if NGUI
                if (GUIManager.Instance != null)
                    GUIManager.Instance.ChangeUIScreenState("Shop_ConsumableItem_Screen");
#else
				_UI_CS_FightScreen.Instance.m_CS_Ingame_NormalPanel.Dismiss();
				_UI_CS_Consumable.Instance.AwakeConsumable();
#endif
				break;
            case NpcInfo.NPCType.DAYREWARDNPC:

				_UI_CS_FightScreen.Instance.m_CS_Ingame_NormalPanel.Dismiss();
				_UI_CS_Consumable.Instance.AwakeConsumable();

				break;
            case NpcInfo.NPCType.STEELNPC:
#if NGUI
				if (InGameScreenShopCraftingCtrl.Instance)
                {
                    //InGameScreenShopCraftingCtrl.Instance.InitShopCrafting();
                }
                if (GUIManager.Instance != null)
                    GUIManager.Instance.ChangeUIScreenState("Shop_Crafting_Screen");
#else
				_UI_CS_FightScreen.Instance.m_CS_Ingame_NormalPanel.Dismiss();
				CraftingPanel.Instance.AwakeCrafting();
#endif
				break;
            case NpcInfo.NPCType.AUCTIONNPC:
                Player.Instance.ReactivePlayer();
				break;
            case NpcInfo.NPCType.SHOPRARE:
#if NGUI
#else
				CS_Main.Instance.g_commModule.SendMessage(
			   			ProtocolGame_SendRequest.RequestPlayerShopInfo(true)
				);
#endif
				break;
            case NpcInfo.NPCType.WELL:
                // action in NPC_Well script
			case NpcInfo.NPCType.STASH:
				// TODO : Open Stash UI
#if NGUI
			if(InGameScreenCtrl.Instance) {
				InGameScreenCtrl.Instance.inventoryCtrl.AwakeStash();
			}
#else
				Stash.Instance.AwakeStash();
#endif
				break;
			case NpcInfo.NPCType.DIALOG:
				GetComponent<PopUpDialog>().PopMenu();
#if NGUI
#else
				_UI_CS_ScreenCtrl.Instance.SetNextScreenType(_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_INGAME_NORMAL);
#endif
				break;
		}
	}
	
	Transform GetNpcTransform(Transform obj)
	{
		Transform T = obj;
		while(T != null)
		{
			if(T.GetComponent<ShopNpc>() != null)
				return T;
			else
				T = T.parent;
		}
		
		return null;
	}
	
	public override string DoExport()
	{
        //Debug.Log(npcType.ToString());
        if (npcType != NpcInfo.NPCType.NOTHING)
        {
			
			XMLStringWriter xmlWriter = new XMLStringWriter();
		
			xmlWriter.NodeBegin("Npc");
			
			xmlWriter.AddAttribute("ID",(int)npcType);
			
			///xmlWriter.NodeBegin("Position");
			
		    xmlWriter.AddAttribute("PosX",transform.position.x);
			
		    xmlWriter.AddAttribute("PosY",transform.position.y);
			
		    xmlWriter.AddAttribute("PosZ",transform.position.z);
			
			//xmlWriter.NodeEnd("Position");
	        
			xmlWriter.NodeEnd("Npc");
			
			return xmlWriter.Result;

			}
			return null;
	}
}
