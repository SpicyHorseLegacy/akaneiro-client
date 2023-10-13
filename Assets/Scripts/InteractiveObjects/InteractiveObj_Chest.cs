using UnityEngine;
using System.Collections;

public class InteractiveObj_Chest : InteractiveHandler {

    public enum EChestType
    {
        Small = 0,
        Noraml,
        Giant
    }

    public EChestType ChestType = EChestType.Small;

}
