using UnityEngine;
using System.Collections;

public class NPC_Well : ShopNpc {

    public KarmaGroupWellManager WellKarmaGroupManager;
    public Transform FullVFX;
    Transform _fullVFX;
    public SWellData WellData
    {
        get
        {
            return _wellData;
        }
        set
        {
            _wellData = value;

            if (CurPercent < 1)
            {
                if (_fullVFX)
                    DestructAfterTime.DestructGameObjectNow(_fullVFX.gameObject);
            }

            if (CurPercent >= 1)
            {
                if(!_fullVFX)
                    _fullVFX = Instantiate(FullVFX) as Transform;
                
                _fullVFX.parent = transform;
                _fullVFX.localPosition = Vector3.zero;
                _fullVFX.localEulerAngles = Vector3.zero;
            }
        }
    }
    SWellData _wellData;

    public ulong LastGetDataTime
    {
        set
        {
//            Debug.LogError(value);
            _lastgetdataServertime = value;
            _lastgetdataClienttime = Time.realtimeSinceStartup;
        }
    }
    ulong _lastgetdataServertime = 0;
    float _lastgetdataClienttime = 0;

    bool canGetKarmaFromWell
    {
        get
        {
            if (CurPercent >= 1.0f)
                return true;
            else
                return false;
        }
    }

    public float CurPercent
    {
        get
        {
            int _LV = 1;
            if (WellData != null) _LV = WellData.speedLevel;
            WellInfo _wellinfo = NpcInfo.WellInfoFromTable.GetWellInfoByLevel(_LV);
            float time = (Time.realtimeSinceStartup - _lastgetdataClienttime) + (_lastgetdataServertime - (ulong)WellData.beginTime);
            //Debug.LogError("time : " + time);
            float _per = time / (60.0f * _wellinfo.iTime);
			if(_per > 1)_per = 1;
            return _per;
        }
    }

    public float CurCollectKarma
    {
        get
        {
            return CurPercent * ShouldGetKarma;
        }
    }

    public float ShouldGetKarma
    {
        get
        {
            int _LV = 1;
            if (WellData != null) _LV = WellData.speedLevel;
            WellInfo _wellinfo = NpcInfo.WellInfoFromTable.GetWellInfoByLevel(_LV);
            return _wellinfo.KarmaPerHour;
        }
    }

    protected override void Awake()
    {
        base.Awake();
		if(CS_Main.Instance)
        	CS_Main.Instance.g_commModule.SendMessage(ProtocolGame_SendRequest.GetWellData());
    }

    void Update()
    {
//        if (Input.GetKeyDown(KeyCode.Y))
//        {
//            Debug.LogError("well percent : " + CurPercent);
//            Debug.LogError("well collect : " + CurCollectKarma + " / " + ShouldGetKarma);
//        }
    }

    public override void PopMenu()
    {
        Player.Instance.ReactivePlayer();

        if (CurPercent >= 1)
            CS_Main.Instance.g_commModule.SendMessage(ProtocolGame_SendRequest.GetWellKarma());
        else
            _UI_CS_PopupBoxCtrl.PopUpError("Well is not ready.");
    }

    public void PopKarma()
    {
        KarmaGroupWellManager _group = Instantiate(WellKarmaGroupManager, transform.position, Quaternion.identity) as KarmaGroupWellManager;
        SMoneyEnter _info = new SMoneyEnter();
        _info.pos = transform.position;
        vectorServerMapMoney _mapmoneyVec = new vectorServerMapMoney();
        SServerMapMoney _mapmoney = new SServerMapMoney();
        _mapmoney.Value = Random.Range(6, 10);
        _mapmoney.ID = (int)KarmaGroupManager.EnumKarmaType.Normal;
        _mapmoneyVec.Add(_mapmoney);
        _info.Distribution = _mapmoneyVec;
        _group.CreateKarmaWithMoneyInfo(_info);
        _group.Target = Player.Instance.transform;

        CS_Main.Instance.g_commModule.SendMessage(ProtocolGame_SendRequest.GetWellData());
    }

    public void GetKarmaSuccess()
    {
        AnimationModel.animation.Play("GAM_Well_Pull");

        if (ActiveSound)
            SoundCue.PlayPrefabAndDestroy(ActiveSound);

    }
}
