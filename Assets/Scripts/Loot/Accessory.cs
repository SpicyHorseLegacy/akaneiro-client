using UnityEngine;
using System.Collections;

public class Accessory : Item 
{
	public enum AccessoryType
	{
		isRing = 0,
		isNecklace,
		isBoots,
		isGloves,
	};
	public AccessoryType accessoryType;
}
