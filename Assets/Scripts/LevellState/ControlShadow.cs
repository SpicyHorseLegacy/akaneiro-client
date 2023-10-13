using UnityEngine;
using System.Collections;

public class ControlShadow : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
#if  UNITY_ANDROID
  gameObject.SetActiveRecursively(true);		
#else
  gameObject.SetActiveRecursively(false);
		
  if(Application.platform == RuntimePlatform.Android)
     gameObject.SetActiveRecursively(true);
  else
     gameObject.SetActiveRecursively(false);
			
  
#endif
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
