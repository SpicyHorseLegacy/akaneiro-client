using UnityEngine;
using System.Collections;

public class TextureRandom : MonoBehaviour {
	
	public float randTimer = 1f;
	float timer = 1f;
	public int xTile = 0;
	public int yTile = 0;
	float dx,dy;
	// Use this for initialization
	void Start () {
		 timer = 0f;
		dx = 1f/(float)xTile;
		dy = 1f/(float)yTile;
	}
	
	// Update is called once per frame
	void Update () {
		if(timer>0)
		{
			timer -= Time.deltaTime;
		}else{
			timer = randTimer;
			renderer.material.SetTextureOffset("_MainTex",new Vector2(Random.Range(0,xTile) * dx,Random.Range(0,yTile) * dy));
		}
	}
}
