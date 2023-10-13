using UnityEngine;
using System.Collections;

public class Menu_Flag_Manager : MonoBehaviour {

    public static Menu_Flag_Manager Instance;

    void Awake()
    {
        Instance = this;
    }

    #region Interface

    public enum EnumFlagState
    {
        NONE,
        Flag_CharacterInfo,
        Flag_Inventory,
        Flag_Ability,
        Flag_Setting,
        MAX,
    }

    private EnumFlagState CurState;
    [SerializeField]
    private UILabel Title;

    public void UpdateFlagState(EnumFlagState _state)
    {
        CurState = _state;
        switch (CurState)
        {
            case EnumFlagState.Flag_CharacterInfo:
                {
                    Title.text = "Player Stats";
                }
                break;
            case EnumFlagState.Flag_Inventory:
                {
                    Title.text = "Inventory";
                }
                break;
            case EnumFlagState.Flag_Ability:
                {
                    Title.text = "Abilities";
                }
                break;
            case EnumFlagState.Flag_Setting:
                {
                    Title.text = "Option";
                }
                break;
        }
    }

    #endregion
}
