using UnityEngine;
using System.Collections;

public class cleaninInInventory : MonoBehaviour {

    private float heapBefore;
    private float heapAfter;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
    void OnBecameVisible()
    {
        heapBefore = Profiler.GetMonoUsedSize();
        System.GC.Collect();
        heapAfter = Profiler.GetMonoUsedSize();
        Debug.Log("the memory cleaned from : [" + heapBefore + "]Bit. And became : [" + heapAfter + "]Bit");
	}
}
