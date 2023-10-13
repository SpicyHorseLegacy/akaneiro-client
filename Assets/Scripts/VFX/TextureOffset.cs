using UnityEngine;
using System.Collections;

public class TextureOffset : MonoBehaviour {
	
	public float xOffset = 0;
	public float yOffset = 0;
	// Use this for initialization
	void Start () {
		renderer.material.SetTextureOffset("_MainTex",new Vector2(xOffset,yOffset));
	}
}
