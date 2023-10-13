using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DrawOutline : MonoBehaviour {

    static public DrawOutline Instance = null;

    public Color Color_Enemy = Color.red;
    public Color Color_PassiveInteractive = Color.yellow;
    public Color Color_FriendlyNPC = Color.green;
    public Color Color_Loot = Color.blue;
    public Color Color_Teleport = Color.cyan;

    public Color Color_Wanted = new Color(23.0f/255.0f, 0, 38.0f/255.0f);

    //shader npc
    Transform OutlineColorNpc;
    List<RestoreColor> RestoreColors = new List<RestoreColor>();

    class RestoreColor
    {
        public string Name;
        public Color Color;
    }

    void Awake(){Instance = this;}

    public void Execute(){NPCOutlineColor();}

    public void NPCOutlineColor()
    {
        if (Time.timeScale == 0)
            return;

#if NGUI
		if(!GUIManager.IsInUIState("IngameScreen") || Player.Instance.IsClickOnUI())
			return;
#endif
		
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        int layer = 1 << LayerMask.NameToLayer("NPC") | 1 << LayerMask.NameToLayer("InteractiveOBJ") | 1 << LayerMask.NameToLayer("Breakable") | 1 << LayerMask.NameToLayer("DropItem");

        RestoreNpcColor();

        Transform _tempTarget = Player.Instance.FindTarget();
        if (!_tempTarget)
        {
            if (Physics.Raycast(ray, out hit, 100f, layer))
            {
                _tempTarget = hit.transform;
            }
        }

        if (_tempTarget)
        {
            Player.Instance.HoverTarget = _tempTarget;
            DrawOulineForObject(_tempTarget);
#if NGUI
            if (InGameScreenCombatHudCtrl.Instance != null)
                InGameScreenCombatHudCtrl.Instance.ShowTopHPBar(_tempTarget.gameObject);
#else
            _UI_CS_TopState.Instance.ShowTargetBar(_tempTarget);
#endif
        }
        else
        {
            Player.Instance.HoverTarget = null;
#if NGUI
            if (InGameScreenCombatHudCtrl.Instance != null)
                InGameScreenCombatHudCtrl.Instance.DisposeTopHpBar();
#else
            _UI_CS_TopState.Instance.ShowTargetBar(null);
#endif
        }
    }

    public void DrawOulineForObject(Transform targetObj)
    {
        RestoreNpcColor();
		
        if (targetObj == OutlineColorNpc)
        {
            return;
        }

        if (targetObj.GetComponent<InteractiveObj>() != null && targetObj.GetComponent<InteractiveObj>().IsUsed) return;

        OutlineColorNpc = targetObj;

        Color tempColor = new Color();
        if (targetObj.gameObject.layer == LayerMask.NameToLayer("NPC"))
        {
            if (targetObj.GetComponent<NpcBase>())
                tempColor = Color_Enemy;
            else
                tempColor = Color_FriendlyNPC;
        }
        else if (targetObj.gameObject.layer == LayerMask.NameToLayer("DropItem"))
        {
            tempColor = Color_Loot;
			if(null != targetObj.GetComponent<Item>()){
				if(null != targetObj.GetComponent<Item>().itemTip){
					targetObj.GetComponent<Item>().itemTip.ShowObj();
				}
			}
#if NGUI 
#else
			if(!_UI_CS_IngameMenu.Instance.isTransmute) {
				MouseCtrl.Instance.SetMouseStats(MouseIconType.PALM);
			}
#endif
        }
        else if (targetObj.GetComponent<TriggerTeleport>())
        {
            tempColor = Color_Teleport;
			
//            _UI_CS_ToolsTip.Instance.IsPopUpInGameToolsTip(false);
        }
        else
        {
            tempColor = Color_PassiveInteractive;
			
//            _UI_CS_ToolsTip.Instance.IsPopUpInGameToolsTip(false);
        }
		
        ReColorObj(OutlineColorNpc, tempColor);
    }

    public void RestoreNpcColor()
    {
        if (!OutlineColorNpc) return;
		
		if(null != OutlineColorNpc.GetComponent<Item>()){
			if(null != OutlineColorNpc.GetComponent<Item>().itemTip){
				OutlineColorNpc.GetComponent<Item>().itemTip.HideObj();
			}
		}
#if NGUI 
#else
		if(MouseCtrl.Instance.iconType != MouseIconType.SELL && !_UI_CS_ScreenCtrl.Instance.IsScreenType(_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_BOUNTY_MASTER) && !_UI_CS_IngameMenu.Instance.isTransmute){
			MouseCtrl.Instance.SetMouseStats(MouseIconType.SWARD1);
		}
#endif		
        if (OutlineColorNpc.GetComponent<NpcBase>() && OutlineColorNpc.GetComponent<NpcBase>().IsWanted)
        {
            foreach (RestoreColor _rc in RestoreColors)
            {
                _rc.Color = Color_Wanted;
            }
        }

        Renderer[] NpcRenderers = OutlineColorNpc.GetComponentsInChildren<Renderer>();
        foreach (Renderer NpcRenderer in NpcRenderers)
        {
            for (int i = 0; i < NpcRenderer.materials.Length; i++)
            {
                Material mtl = NpcRenderer.materials[i];
                if (mtl.HasProperty("_EmissiveColor"))
                {
                    foreach (RestoreColor _rc in RestoreColors)
                    {
                        if (_rc.Name == NpcRenderer.transform.name)
                        {
                            mtl.SetColor("_EmissiveColor", _rc.Color);
                            break;
                        }
                    }
                    
                }
                if (mtl.HasProperty("_EdgeWidth"))
                {
                    mtl.SetFloat("_EdgeWidth", mtl.GetFloat("_EdgeWidth") * 2);
                }
            }
        }

        RestoreColors.Clear();
        OutlineColorNpc = null;
    }

    void ReColorObj(Transform _targetObj, Color _color)
    {
        Renderer[] NpcRenderers = _targetObj.GetComponentsInChildren<Renderer>();
        foreach (Renderer NpcRenderer in NpcRenderers)
        {
            for (int i = 0; i < NpcRenderer.materials.Length; i++)
            {
                Material mtl = NpcRenderer.materials[i];
                if (mtl.HasProperty("_EmissiveColor"))
                {
                    RestoreColor _rc = new RestoreColor();
                    _rc.Name = NpcRenderer.transform.name;
                    _rc.Color = mtl.GetColor("_EmissiveColor");
                    RestoreColors.Add(_rc);
                    mtl.SetColor("_EmissiveColor", _color);
                }
                if (mtl.HasProperty("_EdgeWidth"))
                    mtl.SetFloat("_EdgeWidth", mtl.GetFloat("_EdgeWidth") / 2);
            }
        }
    }
}
