using UnityEngine;
using System.Collections;

public class BuyKarmaPanel : MonoBehaviour {
	
	public static BuyKarmaPanel Instance = null;
	
	void Awake() {
		Instance = this;
	}
	
	// Use this for initialization
	void Start () {
		exitBtn.AddInputDelegate(ExitBtnDelegate);	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public UIButton exitBtn;
	void ExitBtnDelegate(ref POINTER_INFO ptr) {
		switch(ptr.evt) {
		   case POINTER_INFO.INPUT_EVENT.TAP:
				Exit();
				break;
		}	
	}
	
	public UIPanel basePanel;
	public void AwakePanel(){
		Player.Instance.FreezePlayer();
		basePanel.BringIn();
	}
	
	private void Exit(){
		Player.Instance.ReactivePlayer();
		basePanel.Dismiss();
	}
	
	public SpriteText InfoText;
	public void ChangeInfoText(int idx){
		if(idx == 1){
			InfoText.Text = BuyKarma.Instance.shardInfo;
		}else{
			InfoText.Text = BuyKarma.Instance.carystalsInfo;
		}
	}
}
