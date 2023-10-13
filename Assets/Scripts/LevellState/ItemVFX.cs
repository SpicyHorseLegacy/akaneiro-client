using UnityEngine;
using System.Collections;

public class ItemVFX : MonoBehaviour {
	
	
	public Transform[] VFXList;

    public Transform[] GemVFXList;
    public Transform[] CoreVFXList;
    public Transform[] MaterialVFXList;
	
	public static ItemVFX Instance = null;
	
	void Awake () {
		Instance = this;
		DontDestroyOnLoad(gameObject);
		
	}
}
