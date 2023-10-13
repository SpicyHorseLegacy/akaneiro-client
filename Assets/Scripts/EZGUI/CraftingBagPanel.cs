using UnityEngine;
using System.Collections;

public class CraftingBagPanel : MonoBehaviour 
{
	UIPanel myPanel = null;
	void Awake()
	{
		myPanel = GetComponent<UIPanel>();
	}
}
