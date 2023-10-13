using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class ScreenInfoEditor : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnGUI()
    {
        const float delta = 0.03f;
        float ap = ((float) Screen.width) / (float)Screen.height;
        string apName = string.Empty;

        
        if (ap < 4f / 3f - delta)
            apName = "< 4:3";
        else if (Mathf.Abs(ap - 4f / 3f) <= delta)
            apName = "4:3";
        else if (ap < 3f / 2f - delta)
            apName = "< 3:2";
        else if (Mathf.Abs(ap - 3f / 2f) <= delta)
            apName = "3:2";
        else if (ap < 16f / 10f - delta)
            apName = "< 16:10";
        else if (Mathf.Abs(ap - 16f / 10f) <= delta)
            apName = "16:10";
        else if (ap < 16f / 9f - delta)
            apName = "< 16:9";
        else if (Mathf.Abs(ap - 16f / 9f) <= delta)
            apName = "16:9";
        else
            apName = "> 16:9";

        GUI.Label(new Rect(0, 0, 300, 200), "ScreenSize: " + Screen.width + "x" + Screen.height + "\nScreenAspect: " + apName + "\n ap: "+ap);
    }
}
