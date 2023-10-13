using UnityEngine;
using System.Collections;

public class BuffIcons : MonoBehaviour {
	
	public static BuffIcons Instance;
	
	public Texture2D [] icons;
	
	void Awake(){
		Instance = this;
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public Texture2D GetIcon(int idx){
		if(idx < 0 || idx > (icons.Length-1)){
			return null;
		}
		return icons[idx];
	}
}
