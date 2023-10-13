using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InGameScreenCtrl : BaseScreenCtrl
{
    public static InGameScreenCtrl Instance;

    protected override void Awake()
    {
        base.Awake();
        Instance = this;

        HudCtrl = GetComponent<InGameScreenHudCtrl>();
        ChaInfoCtrl = GetComponent<InGameScreencharInfoCtrl>();
        CombatCtrl = GetComponent<InGameScreenCombatHudCtrl>();
		inventoryCtrl = GetComponent<InventoryCtrl>();
		missionObjCtrl = GetComponent<MissionObjectactiveCtrl>();
    }

    void Start()
    {
        Player.Instance.ReactivePlayer();
        GameCamera.BackToPlayerCamera();
    }

    #region Interface
    public InGameScreenHudCtrl HudCtrl;
    public InGameScreencharInfoCtrl ChaInfoCtrl;
    InGameScreenCombatHudCtrl CombatCtrl;
	public InventoryCtrl inventoryCtrl;
	public MissionObjectactiveCtrl missionObjCtrl;
    #endregion

    #region event create and destory
    //MAX template count.//
    protected override void RegisterInitEvent()
    {
        base.RegisterInitEvent();
    }

    protected override void TemplateInitEnd()
    {
        base.TemplateInitEnd();
    }

    protected override void DestoryAllEvent()
    {
        base.DestoryAllEvent();
    }

    protected override void RegisterTemplateEvent()
    {
        base.RegisterTemplateEvent();
    }

    protected override void RegisterSingleTemplateEvent(string _templateName)
    {
        base.RegisterSingleTemplateEvent(_templateName);

        HudCtrl.RegisterSingleTemplateEvent(_templateName);
        ChaInfoCtrl.RegisterSingleTemplateEvent(_templateName);
        CombatCtrl.RegisterSingleTemplateEvent(_templateName);
		inventoryCtrl.RegisterSingleTemplateEvent(_templateName);
		missionObjCtrl.RegisterSingleTemplateEvent(_templateName);
    }

    protected override void UnregisterSingleTemplateEvent(string _templateName)
    {
        base.UnregisterSingleTemplateEvent(_templateName);

        HudCtrl.UnregisterSingleTemplateEvent(_templateName);
        ChaInfoCtrl.UnregisterSingleTemplateEvent(_templateName);
        CombatCtrl.UnregisterSingleTemplateEvent(_templateName);
		inventoryCtrl.UnregisterSingleTemplateEvent(_templateName);
		missionObjCtrl.UnregisterSingleTemplateEvent(_templateName);
    }

    #endregion

   
}
