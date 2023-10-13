using UnityEngine;
using System.Collections;

public class PlayerBuffManager : BuffManager {

    public override BaseBuff AddBuffByID(int id, Transform _sourceTransform)
    {
        //Debug.LogError("AddBuff");
        foreach (BaseBuff buff in Buffs)
        {
            if (id == buff.ID)
            {
                //buff.RefreshBuff();
                buff.AddStack();
#if NGUI
                if (UI_Hud_BuffBar_Manager.Instance)
                {
                    UI_Hud_BuffBar_Manager.Instance.UpdateBuff(buff);
                }
#else
                PlayerInfoBar.Instance.UpdateBuffState();
#endif
                return buff;
            }
        }

        BaseBuff newBuff = BuffInfo.Instance.GetBuffPrefab(id);
        if (newBuff)
        {
            newBuff.transform.parent = transform;
            newBuff.transform.localPosition = Vector3.zero;
            newBuff.Owner = Owner;
            newBuff.SourceObj = _sourceTransform;
            newBuff.AddStack();
            Buffs.Add(newBuff);
        }
#if NGUI
        if (UI_Hud_BuffBar_Manager.Instance && newBuff != null)
        {
            UI_Hud_BuffBar_Manager.Instance.UpdateBuff(newBuff);
        }
#else
        PlayerInfoBar.Instance.UpdateBuffState();
#endif
        return newBuff;
    }

    public override void DeleteBuffByID(int id)
    {
        for (int i = Buffs.Count - 1; i >= 0; i--)
        {
            if (id == Buffs[i].ID)
            {
                Buffs[i].RemoveStack();
            }
        }
#if NGUI

#else
		PlayerInfoBar.Instance.UpdateBuffState();
#endif
    }
}
