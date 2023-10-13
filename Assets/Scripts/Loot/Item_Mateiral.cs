using UnityEngine;
using System.Collections;

public class Item_Mateiral : Item
{
    public enum EnumLootMaterailType
    {
        Core = 0,
        Gem,
        Material
    }

    public EnumLootMaterailType MaterialType;

    public Transform LootVFX;

}
