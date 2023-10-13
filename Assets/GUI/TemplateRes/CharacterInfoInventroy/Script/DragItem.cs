using UnityEngine;
using System.Collections;

public class DragItem : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		UpdatePos();
	}
	
	[SerializeField]
	private UITexture icon;
	public UITexture GetIcon() {
		return icon;
	}
	public void SetIcon(Texture2D img) {
		icon.enabled = false;
		icon.mainTexture = img;
		icon.enabled = true;
	}
	
	private void UpdatePos() {
		transform.position = UICamera.currentCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y,-1));
	}
}
