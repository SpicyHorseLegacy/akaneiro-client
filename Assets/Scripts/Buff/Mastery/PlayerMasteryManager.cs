using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerMasteryManager : MonoBehaviour {
	
	public SingleMastery[] Masteries;

    public void UpdateMasteryInfo(vectorMasteryLevelInfo MasteryLevelInfoVec)
	{
        foreach (SMasteryLevelInfo masteryInfo in MasteryLevelInfoVec)
        {
            for (int i = Masteries.Length - 1; i >= 0; i--)
            {
                SingleMastery _singleMastery = Masteries[i];
                if (_singleMastery.MasteryClass.Get() == masteryInfo.masteryType.Get())
                {
                    _singleMastery.UpdateInfoFromServer(masteryInfo);
                    break;
                }
            }
        }
	}

    public SingleMastery GetMasteryByType(EMasteryType _type)
    {
        foreach (SingleMastery _mastery in Masteries)
        {
            if (_type.Get() == _mastery.MasteryClass.Get())
                return _mastery;
        }
        return null;
    } 
}
