using UnityEngine;
using System.Collections;

public class UI_CharInfo_Tooltips_Abi : UI_CharInfo_Tooltips_BaseGroup {

    public class CharInfo_Tooltips_Ability_Data
    {
        public string AbiName;
        public Color AbiColor;
        public int CurLevel;
        public int MaxLevel;
        public string Description;
        public int EnergyRequired;
        public string BuffName;
        public string BuffDescription;
    }

    public virtual void UpdateAllInfo(CharInfo_Tooltips_Ability_Data _data){ }
}
