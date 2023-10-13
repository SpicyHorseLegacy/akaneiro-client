using UnityEngine;
using System.Collections;

public class RendererHidden : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Renderer[] rds = GetComponentsInChildren<Renderer>();
		foreach(Renderer rd in rds)
		{
			rd.enabled=false;
		}
	}
}
