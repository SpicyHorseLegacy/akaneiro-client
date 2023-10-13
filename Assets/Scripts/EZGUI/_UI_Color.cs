using UnityEngine;
using System.Collections;

public class _UI_Color : MonoBehaviour {
	
	public static _UI_Color Instance = null;
	
	public Color color1;
	public Color color2;
	public Color color3;
	public Color color4;
	public Color color5;
	public Color color6;
	public Color color7;
	public Color color8;
	public Color color9;
	public Color color10;
	public Color color11;
	public Color color12;
	public Color color13;
	public Color color14;
	public Color color15;
	public Color color16;
	public Color color17;
	public Color color18;
	public Color color19;
	public Color color20;
	public Color color21;
	public Color color22;
	public Color color23;
	public Color color24;
	public Color color25;
	public Color color26;
	public Color color27;
	public Color color28;
	public Color color29;
	public Color color30;
	
	void Awake(){
		
		Instance = this;
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void SetNameColor(float val,SpriteText name) {
		if(name == null) {
			LogManager.Log_Error("SetNameColor err,text is null. text name:",name.name);
			return;
		}
		if(val < _UI_CS_ItemVendor.Instance.greenVal){
			name.SetColor(_UI_Color.Instance.color1);
		}else if( (_UI_CS_ItemVendor.Instance.greenVal - 0.01) < val && val  < _UI_CS_ItemVendor.Instance.blueVal){
			name.SetColor(_UI_Color.Instance.color15);
		}else if( (_UI_CS_ItemVendor.Instance.blueVal - 0.01) < val && val < _UI_CS_ItemVendor.Instance.purpleVal){
			name.SetColor(_UI_Color.Instance.color16);
		}else if( (_UI_CS_ItemVendor.Instance.purpleVal - 0.01) < val && val < _UI_CS_ItemVendor.Instance.brownVal){
			name.SetColor(_UI_Color.Instance.color17);
		}else if( (_UI_CS_ItemVendor.Instance.brownVal - 0.01) < val){
			name.SetColor(_UI_Color.Instance.color18);
		}		
	}
	
	public void SetNameColor(float val,UIButton bg) {
		if(bg == null) {
			LogManager.Log_Error("SetNameColor err,bg is null. bg name:",bg.name);
			return;
		}
		if(val < _UI_CS_ItemVendor.Instance.greenVal){
			bg.SetColor(_UI_Color.Instance.color1);
		}else if( (_UI_CS_ItemVendor.Instance.greenVal - 0.01) < val && val  < _UI_CS_ItemVendor.Instance.blueVal){
			bg.SetColor(_UI_Color.Instance.color15);
		}else if( (_UI_CS_ItemVendor.Instance.blueVal - 0.01) < val && val < _UI_CS_ItemVendor.Instance.purpleVal){
			bg.SetColor(_UI_Color.Instance.color16);
		}else if( (_UI_CS_ItemVendor.Instance.purpleVal - 0.01) < val && val < _UI_CS_ItemVendor.Instance.brownVal){
			bg.SetColor(_UI_Color.Instance.color17);
		}else if( (_UI_CS_ItemVendor.Instance.brownVal - 0.01) < val){
			bg.SetColor(_UI_Color.Instance.color18);
		}		
	}
}
