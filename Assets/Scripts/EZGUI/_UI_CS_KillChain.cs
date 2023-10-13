using UnityEngine;
using System.Collections;

public class _UI_CS_KillChain : MonoBehaviour {
	
	//Instance
	public static _UI_CS_KillChain Instance = null;
	
	public UIPanel AllPanel;
	
	public UIPanel GoodPanel;
	public UIPanel LethalPanel;
	public UIPanel SlayerPanel;

	public int KillChainCount = 0;
	
	public int goodFlag = 5;
	public int lethalFlag = 12;
	public int slayerFlag = 25;

	public UIPanel    TextPanel;
	public UIButton   Bg;
	public SpriteText killChainValText;
	public SpriteText killsText;
	
	public UIPanel    RewardTextPanel;
	public SpriteText RewardText;
	
	public Texture2D [] bloodImg;
	
	public bool isServerCalc = false;
	
	void Awake(){
		Instance = this;
	}
	
	// Use this for initialization
	void Start () {
	
		killChainValText.Text = "";
		killsText.Text = "";
		TextPanel.Dismiss();
		RewardTextPanel.Dismiss();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void CalcKillChainTime(){
		
		if(!isServerCalc)
			return;
		
		if(0 == KillChainCount ){
			KillChainCount += 1;
		}else{
			KillChainCount++;
		}

		killChainValText.Text = KillChainCount.ToString();
		killsText.Text = "Kills";
		
		
		AllPanel.BringIn();
		Bg.Hide(false);
		Bg.SetUVs(new Rect(0,0,1,1));
		Bg.SetTexture(bloodImg[KillChainCount%2]);
		
		TextPanel.BringIn();
		
	}
	
	
	// good 1 lethal 2 slayer 3
	public void CallKillChain(int idx,int exp){

		switch(idx){
			
		case 1:
				
				_UI_CS_MissionLogic.Instance.SetMissionScore(exp);			
				GoodPanel.BringIn();
				RewardTextPanel.BringIn();
				RewardText.Text = exp.ToString();
				StartCoroutine(CallGoodPanDismissBg());
			
				break;
			
		case 2:

				_UI_CS_MissionLogic.Instance.SetMissionScore(exp);			
				LethalPanel.BringIn();
				RewardTextPanel.BringIn();
				RewardText.Text = exp.ToString();
				StartCoroutine(CallLethalPanDismissBg());
				
				break;
			
		case 3:

				_UI_CS_MissionLogic.Instance.SetMissionScore(exp);
				SlayerPanel.BringIn();
				RewardTextPanel.BringIn();
				RewardText.Text = exp.ToString();
				StartCoroutine(CallSlayerPanDismissBg());
			
				break;
			
		default:
				break;
		}
		
		
	}

	public IEnumerator CallBgDismissBg()
	{	
		yield return new WaitForSeconds(1);
		Bg.Hide(true);
		AllPanel.Dismiss();
		KillChainCount = 0;

	}
	
	public void HideCombInfo(){
		AllPanel.Dismiss();	
		GoodPanel.Dismiss();
		LethalPanel.Dismiss();
		SlayerPanel.Dismiss();
		RewardTextPanel.Dismiss();
		Bg.Hide(true);
		TextPanel.Dismiss();
		KillChainCount = 0;
	}
	
	public IEnumerator CallGoodPanDismissBg()
	{	
		yield return new WaitForSeconds(0.5f);
		
		GoodPanel.Dismiss();
		RewardTextPanel.Dismiss();
		TextPanel.Dismiss();
		StartCoroutine(CallBgDismissBg());
		
	}
	
	public IEnumerator CallLethalPanDismissBg()
	{	
		yield return new WaitForSeconds(0.5f);
		
		LethalPanel.Dismiss();
		RewardTextPanel.Dismiss();
		TextPanel.Dismiss();
		StartCoroutine(CallBgDismissBg());
		
	}
	
	public IEnumerator CallSlayerPanDismissBg()
	{	
		yield return new WaitForSeconds(0.5f);
		
		SlayerPanel.Dismiss();
		RewardTextPanel.Dismiss();
		TextPanel.Dismiss();
		StartCoroutine(CallBgDismissBg());
		
	}
	
	public void DismissKillChainMsg(){
	
		GoodPanel.Dismiss();
		LethalPanel.Dismiss();
		SlayerPanel.Dismiss();
		RewardTextPanel.Dismiss();
		KillChainCount = 0;

		AllPanel.Dismiss();
		Bg.Hide(true);
		TextPanel.Dismiss();
	}
	
	
}
