using UnityEngine;
using System.Collections;

public class TemplateWidgetInitManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	[SerializeField]
	private int widgetMax = 1;
	[SerializeField]
	private int curWidgetCount = 0;
	public void AddWidgetComplete() {
		curWidgetCount++;
		if(curWidgetCount >= widgetMax) {
			GUIManager.Instance.AddTemplateInitEnd();
		}
	}
}
