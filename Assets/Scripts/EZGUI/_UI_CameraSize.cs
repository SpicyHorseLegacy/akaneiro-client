using UnityEngine;
using System.Collections;

public class _UI_CameraSize : MonoBehaviour {
	
	public float K = 18f;
	
	public static _UI_CameraSize Instance = null;
	
// Use this for initialization
	void Awake ()
    {
        //if ( !Application.isEditor )
        {
			Instance = this;
            Camera camMain = this.transform.GetComponent<Camera>();

            camMain.AutoResize();
        }
    }
	
	void Update () {
		
		Camera camMain1 = this.transform.GetComponent<Camera>();
		camMain1.AutoResize();
	
	}
	
}


public static class CameraAutoResize
{
    //Add instructions

    //Camera Component overrides:
    /// <summary>
    /// Tell the Camera to resize it's orthographic size according to the current screen size. 
    /// </summary>
    /// <param name="target">
    /// A <see cref="Camera"/>
    /// </param>
    /// 

    public static void AutoResize(this Camera target)
    {
        target.AutoResize(Screen.width, Screen.height);
    }

    public static void AutoResize(this Camera target, float width, float height)
    {
        if (target.orthographic)
        {
//            float orthSize = 23.99f / (width / height);
			
			
			
			float orthSize;
			
			float rat = width / height;
			if ( rat < 1f ) rat = 1f / rat;
			rat = Mathf.Clamp( rat, rat, 16f/ 9f);
			
			orthSize = _UI_CameraSize.Instance.K / rat;

//            orthSize = Mathf.Clamp(orthSize, 13.49f, orthSize);

            target.orthographicSize = orthSize;
        }
    }
}
