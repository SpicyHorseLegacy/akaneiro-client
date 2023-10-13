using UnityEngine;
using System.Collections;

public class InGameScreenCombatHudCtrl : MonoBehaviour {

    public static InGameScreenCombatHudCtrl Instance;

    public Color DamageColor_PlayerToMonsterNormal;
    public Color DamageColor_PlayerToMonsterCrit;
    public Color DamageColor_MonsterToPlayerNormal;
    public Color DamageColor_MonsterToPlayerCrit;
    public Color DamageColor_Heal;

    void Awake()
    {
        Instance = this;
    }

    public void RegisterSingleTemplateEvent(string _templateName)
    {
    }

    public void UnregisterSingleTemplateEvent(string _templateName)
    {
    }

    public void ShowTopHPBar(GameObject _target)
    {
        if (UI_Hud_TopHPBar_Manager.Instance == null) return;

        UI_TypeDefine.UI_GameHud_TopHPBar_data _Data = null;
        if (_target.GetComponent<NpcBase>() != null)
        {
            _Data = new UI_TypeDefine.UI_GameHud_TopHPBar_data();
            _Data.TargetType = UI_TypeDefine.UI_GameHud_TopHPBar_data.EnumUI_Hud_TopHPBar_TargetType.Monster;
            _Data.TargetName = _target.GetComponent<NpcBase>().NpcName;
            _Data.CurHP = _target.GetComponent<NpcBase>().AttrMan.Attrs[EAttributeType.ATTR_CurHP];
            _Data.MAXHP = _target.GetComponent<NpcBase>().AttrMan.Attrs[EAttributeType.ATTR_MaxHP];
            _Data.MonsterRankType = UI_TypeDefine.UI_GameHud_TopHPBar_data.EnumUI_Hud_TopHPBar_MonsterRankType.Normal;
            if (_target.GetComponent<NpcBase>().IsWanted)
                _Data.MonsterRankType = UI_TypeDefine.UI_GameHud_TopHPBar_data.EnumUI_Hud_TopHPBar_MonsterRankType.Wanted;
            if (_target.GetComponent<NpcBase>().IsBoss)
            {
                _Data.TargetType = UI_TypeDefine.UI_GameHud_TopHPBar_data.EnumUI_Hud_TopHPBar_TargetType.MonsterBoss;
                _Data.MonsterRankType = UI_TypeDefine.UI_GameHud_TopHPBar_data.EnumUI_Hud_TopHPBar_MonsterRankType.Boss;
            }
            _Data.ElementalData = new UI_TypeDefine.UI_GameHud_TopHPBar_ElementalIcons_data();
            foreach (NpcBase.NpcAttackProperty attackproperty in _target.GetComponent<NpcBase>().AttackState.AttackArray)
            {
                switch (attackproperty.ElementalDamageType)
                {
                    case NpcBase.ElemDamageKind.Flame:
                        _Data.ElementalData.IsFlame = true;
                        break;
                    case NpcBase.ElemDamageKind.Frost:
                        _Data.ElementalData.IsFrost = true;
                        break;
                    case NpcBase.ElemDamageKind.Explosion:
                        _Data.ElementalData.IsBlast = true;
                        break;
                    case NpcBase.ElemDamageKind.Storm:
                        _Data.ElementalData.IsStorm = true;
                        break;
                }
            }
        }
        else if (_target.GetComponent<ShopNpc>() != null)
        {
            _Data = new UI_TypeDefine.UI_GameHud_TopHPBar_data();
            _Data.TargetType = UI_TypeDefine.UI_GameHud_TopHPBar_data.EnumUI_Hud_TopHPBar_TargetType.Friendly;

            if (_target.GetComponent<NPC_Well>())
            {
                _Data.TargetType = UI_TypeDefine.UI_GameHud_TopHPBar_data.EnumUI_Hud_TopHPBar_TargetType.Well;
                _Data.TargetName = ((int)_target.GetComponent<NPC_Well>().CurCollectKarma).ToString() + " / " + ((int)_target.GetComponent<NPC_Well>().ShouldGetKarma).ToString();
                _Data.CurHP = _target.GetComponent<NPC_Well>().CurPercent;
                _Data.MAXHP = 1;
            }
            else
            {
                _Data.TargetName = _target.GetComponent<ShopNpc>().npcName;
                _Data.CurHP = _target.GetComponent<ShopNpc>().AttrMan.Attrs[EAttributeType.ATTR_CurHP];
                _Data.MAXHP = _target.GetComponent<ShopNpc>().AttrMan.Attrs[EAttributeType.ATTR_MaxHP];

                if(_target.GetComponent<ShopNpc>().npcType == NpcInfo.NPCType.DIALOG)
                    _Data.TargetType = UI_TypeDefine.UI_GameHud_TopHPBar_data.EnumUI_Hud_TopHPBar_TargetType.Dialog;
            }
        }
        else if (_target.GetComponent<InteractiveHandler>() != null)
        {
            _Data = new UI_TypeDefine.UI_GameHud_TopHPBar_data();
            _Data.TargetType = UI_TypeDefine.UI_GameHud_TopHPBar_data.EnumUI_Hud_TopHPBar_TargetType.Interactive;
            _Data.TargetName = _target.GetComponent<InteractiveHandler>().name;
            if (!_target.GetComponent<InteractiveHandler>().IsUsed)
            {
                _Data.CurHP = _target.GetComponent<InteractiveHandler>().AttrMan.Attrs[EAttributeType.ATTR_CurHP];
                _Data.MAXHP = _target.GetComponent<InteractiveHandler>().AttrMan.Attrs[EAttributeType.ATTR_MaxHP];
            }
            else
            {
                _Data.CurHP = 1;
                _Data.MAXHP = 1;
            }
        }

        if (_Data != null)
        {
            UI_Hud_TopHPBar_Manager.Instance.Show();
            UI_Hud_TopHPBar_Manager.Instance.UpdateAllInfo(_Data);
        }
    }

    public void DisposeTopHpBar()
    {
        if (UI_Hud_TopHPBar_Manager.Instance)
            UI_Hud_TopHPBar_Manager.Instance.Dispoose();
    }

    public void ShowDamageAtPos(int damage, Transform owner, bool iscrit, EStatusElementType elementType)
    {
        UI_TypeDefine.UI_GameHud_DamageTXT_data _newdata = new UI_TypeDefine.UI_GameHud_DamageTXT_data();
        _newdata.IsCritical = iscrit;

        if (elementType.Get() == EStatusElementType.StatusElement_Invalid)
        {
            _newdata.AniType = UI_TypeDefine.UI_GameHud_DamageTXT_data.EnumDamageTXTAnimationType.Jump;
        }
        else
        {
            _newdata.AniType = UI_TypeDefine.UI_GameHud_DamageTXT_data.EnumDamageTXTAnimationType.Jump;
        }

        // set color of damage text.
        if (damage < 0)
        {
            if (!owner.GetComponent<Player>())
            {
                _newdata.DamageColor = DamageColor_PlayerToMonsterNormal;

                if (owner.GetComponent<AllyNpc>())
                    _newdata.DamageColor = DamageColor_MonsterToPlayerNormal;
            }
            else
            {
                _newdata.DamageColor = DamageColor_MonsterToPlayerNormal;
            }
        }
        else
            _newdata.DamageColor = DamageColor_Heal;

        _newdata.DamageTEXT = "";
        _newdata.LifeTime = 0.5f;
        _newdata.ScaleSize = Vector3.one;

        if (iscrit)
        {
            _newdata.DamageTEXT = "Crit! ";
            _newdata.LifeTime = 0.75f;
            _newdata.ScaleSize = Vector3.one * 1.2f;
        }

        _newdata.DamageTEXT += Mathf.Abs(damage);

        if(UI_Hud_DamageTXT_Manager.Instance)
            UI_Hud_DamageTXT_Manager.Instance.ShowDamageAtPos(_newdata, owner.transform.position);
    }
}
