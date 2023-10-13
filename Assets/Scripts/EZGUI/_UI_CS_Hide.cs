using UnityEngine;
using System.Collections;

public class _UI_CS_Hide : MonoBehaviour {
	
	public SurveillanceCamera CharacterCarema;
	
	// Use this for initialization
	void Start () {
		Hide(true); 
	}
	
	public void Hide(bool tf){
		print("hide player : " + tf);
		
		if(!_UI_CS_ScreenCtrl.Instance.IsScreenType(_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_SELECT))
			tf = true;

        //if (!tf)
            //CharacterCarema.ShowAt(new Vector2(Screen.width / 2, Screen.height / 2), new Vector2(Screen.width, Screen.height));
            //CharacterCarema.Show();
        //else
           // CharacterCarema.ShutDown();

        if (!tf)
        {
            transform.localPosition = Vector3.zero;
            transform.localScale = Vector3.one * 7;
        }
        else
        {
            transform.position = Vector3.one * 500;
        }
	}
}
