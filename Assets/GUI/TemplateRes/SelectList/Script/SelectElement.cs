using UnityEngine;
using System.Collections;

public class SelectElement : MonoBehaviour {
	
	[SerializeField]
	private NGUIButton bg;
	[SerializeField]
	private UILabel name;
	[SerializeField]
	private NGUISlider objLst;
	[SerializeField]
	private UILabel level;
	[SerializeField]
	private UISprite icon;
	public SelectElementInfo data;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	#region Interface
	public void SetName(string sName) {
		name.text = sName;
		//-------->>mm
		aPlayerIcon.thePlayerName = sName;
		//-------->>#mm
	}
	public void SetLevel(int ilevel) {
		level.text = ilevel.ToString();
	}
	public void SetLevelBarVal(float val) {
		objLst.sliderValue = val;
	}
	public void SetTypeIcon(string imgName) {
		icon.spriteName = imgName;
		//-------->>mm
		aPlayerIcon.thePlayerIconName = imgName;
		//-------->>#mm
	}
	#endregion
	
	#region Local
	private void BgDelegate() {
		SelectListManager.Instance.ChangeSelectCharaDelegate(data);
		aPlayerIcon.thePlayerIconName = icon.spriteName;
	}
	#endregion
}
