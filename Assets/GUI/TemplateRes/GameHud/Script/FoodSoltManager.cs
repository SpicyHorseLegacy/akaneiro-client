using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class FoodSoltManager : MonoBehaviour
{
    public static FoodSoltManager Instance;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            list[0]._DoubleClickDelegate();
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            list[1]._DoubleClickDelegate();
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            list[2]._DoubleClickDelegate();
        }
    }

    #region Interface
    [SerializeField]
    public List<InventorySlot> list = new List<InventorySlot>();

    public InventorySlot GetFoodItemData(int solt)
    {
        foreach (InventorySlot data in list)
            if (data.GetSlot() == solt)
                return data;

        return null;
    }

    public int InFoodSlot(Vector3 pos, int type, int slot)
    {
        foreach (InventorySlot invSlot in list)
        {
            if (type == 5 && slot == invSlot.GetSlot())
                continue;

            float left = invSlot.transform.position.x - 0.05f;
            float top = invSlot.transform.position.y + 0.05f;
            float right = invSlot.transform.position.x + 0.05f;
            float bootom = invSlot.transform.position.y - 0.05f;

            if (left <= pos.x && top >= pos.y && right >= pos.x && bootom <= pos.y)
                return invSlot.GetSlot();
        }

        return -1;
    }

    public int GetEmptySlotIdx()
    {
        foreach (InventorySlot invSlot in list)
            if (invSlot.GetEmptyFlag())
                return invSlot.GetSlot();

        return -1;
    }
    #endregion
}
