using UnityEngine;
using System.Collections;

public class simpleAllocator : MonoBehaviour {

	// Use this for initialization
	void Start () {
        object[] tmp = new System.Object[2048];
        for (int i = 0; i < 2048; i++)
        {
            tmp[i] = new byte[2048];
        }
        tmp = null;
	}
}
