using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class LootDrop : MonoBehaviour
{
    public bool CanDropLoot = true;

    [System.Serializable]
    public class RandomDropItemSet
    {
        public int ItemNumber = 1;
        public int DropChance = 20;
    }
    public RandomDropItemSet[] RandomDropItemList;

    [System.Serializable]
    public class cUniqueItem
    {
        public int DropChance = 100;
        public int ItemID = 0;
        public int PerfabID = 1;
    }
    public cUniqueItem[] UniqueItemsPrefab;

    [System.Serializable]
    public class cUniqueItemFromRareItemList
    {
        public int DropChance = 100;
        public int UniqueID;
    }
    public cUniqueItemFromRareItemList[] UniqueItemList;

    [System.Serializable]
    public class MaterialDropSet
    {
        public Transform MaterialPrefab;
        public int MaterialNumber = 1;
        public int DropChance = 100;
    }
    public MaterialDropSet[] MaterialDropList;

	[System.Serializable]
	public class ConsumableItemDropSet
	{
		public int DropAmount = 0;
		public int DropChance = 100;

		public ConsumableItemDropSet(int _amount, int _chance)
		{
			DropAmount = _amount;
			DropChance = _chance;
		}
	}
	public ConsumableItemDropSet[] ConsumableDropList = {new ConsumableItemDropSet(0, 85), new ConsumableItemDropSet(1, 15)};

    public void DropRandomItem()
    {


    }

    //for more info, see weapon.csv
    void GenerateRandomWeapon(int level)
    {


    }

    public Transform GenerateRandomArmor(int level)
    {
        return null;
    }

    public void DoOtherExport(XMLStringWriter theWriter)
    {
        XMLStringWriter xmlWriter = theWriter;

        xmlWriter.NodeBegin("LootDrop");

        xmlWriter.AddAttribute("CanDropLoot", CanDropLoot);

        if (RandomDropItemList != null && RandomDropItemList.Length > 0)
        {
            xmlWriter.NodeBegin("RandomDropItemList");

            foreach (RandomDropItemSet it in RandomDropItemList)
            {
                xmlWriter.NodeBegin("RandomDropItem");
                xmlWriter.AddAttribute("ItemNumber", it.ItemNumber);
                xmlWriter.AddAttribute("DropChance", it.DropChance);
                xmlWriter.NodeEnd("RandomDropItem");
            }

            xmlWriter.NodeEnd("RandomDropItemList");
        }

        if (UniqueItemsPrefab != null && UniqueItemsPrefab.Length > 0)
        {

            xmlWriter.NodeBegin("UniqueDropItemList");
            foreach (cUniqueItem it in UniqueItemsPrefab)
            {
                if (it == null) continue;

                xmlWriter.NodeBegin("UniqueDropItem");
                xmlWriter.AddAttribute("ItemID", it.ItemID);
                xmlWriter.AddAttribute("PrefabID", it.PerfabID);
                xmlWriter.AddAttribute("DropChance", it.DropChance);
                xmlWriter.NodeEnd("UniqueDropItem");

            }
            xmlWriter.NodeEnd("UniqueDropItemList");
        }

        if (UniqueItemList != null && UniqueItemList.Length > 0)
        {
            xmlWriter.NodeBegin("UniqueDropItemFromRaraItemList");
            foreach (cUniqueItemFromRareItemList it in UniqueItemList)
            {
                if (it == null) continue;

                xmlWriter.NodeBegin("UniqueDropItemFromRaraItem");
                xmlWriter.AddAttribute("UniqueID", it.UniqueID);
                xmlWriter.AddAttribute("DropChance", it.DropChance);
                xmlWriter.NodeEnd("UniqueDropItemFromRaraItem");

            }
            xmlWriter.NodeEnd("UniqueDropItemFromRaraItemList");
        }

        if (MaterialDropList != null && MaterialDropList.Length > 0)
        {
            xmlWriter.NodeBegin("MaterialDropList");

            foreach (MaterialDropSet it in MaterialDropList)
            {
                if (it == null || it.MaterialPrefab == null) continue;

                Item theItem = it.MaterialPrefab.GetComponent<Item>();

                if (theItem != null)
                {
                    xmlWriter.NodeBegin("MaterialDrop");

                    xmlWriter.AddAttribute("ItemID", theItem.TypeID);

                    xmlWriter.AddAttribute("PrefabID", theItem.PrefabID);

                    xmlWriter.AddAttribute("ItemCount", it.MaterialNumber);

                    xmlWriter.AddAttribute("DropChance", it.DropChance);

                    xmlWriter.NodeEnd("MaterialDrop");
                }
            }

            xmlWriter.NodeEnd("MaterialDropList");
        }

		if (ConsumableDropList != null && ConsumableDropList.Length > 0)
		{
			xmlWriter.NodeBegin("ConsumableDropList");
			
			foreach (ConsumableItemDropSet _drop in ConsumableDropList)
			{
				xmlWriter.NodeBegin("ConsumableDrop");
				
				xmlWriter.AddAttribute("DropAmount", _drop.DropAmount);
				
				xmlWriter.AddAttribute("DropChance", _drop.DropChance);
				
				xmlWriter.NodeEnd("ConsumableDrop");
			}
			
			xmlWriter.NodeEnd("ConsumableDropList");
		}
		xmlWriter.NodeEnd("LootDrop");
    }

}


