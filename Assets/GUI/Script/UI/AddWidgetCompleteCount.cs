using UnityEngine;
using System.Collections;

public class AddWidgetCompleteCount : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	[SerializeField]
	private Transform templateObj;
	[SerializeField]
	private float delayTim = 0f;
    public void AddWidgetComplete() {
		templateObj.GetComponent<TemplateWidgetInitManager>().AddWidgetComplete();
    }
	
	private IEnumerator DelayTime() {
		yield return new WaitForSeconds(delayTim);
		templateObj.GetComponent<TemplateWidgetInitManager>().AddWidgetComplete();
	}
}
