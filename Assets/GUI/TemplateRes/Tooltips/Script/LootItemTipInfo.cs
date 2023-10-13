using UnityEngine;
using System.Collections;

public class LootItemData{
	public string name;
	public Color color;
}

public enum LootTipStyle{
	always = 0,
	atFirst,
	MAX,
}

public class LootItemTipInfo : MonoBehaviour {
	
	private LootTipStyle lootStyle = LootTipStyle.always;
	
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		TimeOut();
	}
	
	
	#region Interface
	[SerializeField]
	private UISprite background;
	[SerializeField]
	private UILabel lootName;
	
	public void SetLootName(LootItemData data){
		lootName.text = data.name;
		lootName.color = data.color;
		background.transform.localScale = new Vector3(lootName.relativeSize.x * lootName.transform.localScale.x,lootName.relativeSize.y * lootName.transform.localScale.y,1);	
	}
	
	private float time = 5.0f;
	public void SetTimeOut(float val){
		time = val;
	}
	
	private bool isShowing = false;
	public void Show(Vector3 v,LootItemData data){
		isShowing = true;
		GetTime();
        gameObject.transform.position = v; 
	}
	
	public void Hide(){
		isShowing = false;
		gameObject.transform.position = new Vector3(999f,999f,-2f); 
	}
	#endregion
	
	#region Local
	private void TimeOut(){
		if(isShowing){
			if(lootStyle == LootTipStyle.atFirst ){
				GetTime();
				if(curTime>time){
					Hide();
				}
			}
		}
	}
	
	private float curTime;
	private void GetTime(){
		curTime = Time.time;
	}
	#endregion
}
