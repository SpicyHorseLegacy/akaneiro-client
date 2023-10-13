using UnityEngine;
using System.Collections;

public class _UI_CS_ElementsInfo : MonoBehaviour {
	
	public static _UI_CS_ElementsInfo Instance = null;
	
	public Texture2D [] EleIcon;
	public Texture2D [] EncIcon;
	public Texture2D [] GemIcon;
	public Transform	consumableFoodSound;
	public Transform	consumableDrinkSound;
	
	void Awake(){
		Instance = this;
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
