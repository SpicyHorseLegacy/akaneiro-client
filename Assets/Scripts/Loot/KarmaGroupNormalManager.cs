using UnityEngine;
using System.Collections;

public class KarmaGroupNormalManager : KarmaGroupManager
{
    // the karma group manages all karmas which spawned from one object. One object spawns one karma group. Any karma from group is picked up, all karmas from this group are picked up.
    bool isGotKarma = false;

    protected override void Awake()
    {
        base.Awake();
        isGotKarma = false;
    }

    public override void PlayerGetKarma(KarmaController _karma)
    {
        base.PlayerGetKarma(_karma);

        if (!isGotKarma)
            CS_Main.Instance.g_commModule.SendMessage(ProtocolBattle_SendRequest.PickupMoneyReq(Info.objectID));

        isGotKarma = true;
    }
}
