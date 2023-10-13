using UnityEngine;
using System.Collections;

public class _UI_CS_SpiritInfo : MonoBehaviour {
	
	public static _UI_CS_SpiritInfo Instance;
	
	public Texture2D [] spirirtIcon;
	
	public Texture2D [] spirirtSmallIcon;
	
	public Color 	 [] spirirtColor;

	void Awake()
	{
		Instance = this;
	}
}
