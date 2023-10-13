using UnityEngine;
using System.Collections;

public class Tutorial : MonoBehaviour {
	
	public static Tutorial Instance = null;
	
	public UIPanel  BGpanel;
	
	public UIButton 	backBtn;
	public SpriteText 	backText;
	public UIButton 	nextBtn;
	public SpriteText 	nextText;
	public UIButton 	okayBtn;
	public SpriteText 	okayText;
	public UIButton 	exitBtn;
	
	public SpriteText 	TitleText;
	public SpriteText 	Msg1Text;
	public SpriteText 	Msg2Text;
	
	public UIPanel  	BGpanel_OldLady;
	public UIButton 	okayBtn_OldLady;
	public SpriteText 	OldLadyNameText;
	public SpriteText 	OldLadyMsgText;
	public bool 		isTutorial = false;
	
	public int stepOldLady = 0;
	
	public int stepIdx = 0;

	public int Idx = 0;
	
	public Transform objPos;
	public Transform objPos_OladLady;
	public Transform objPos_OladMan;
	public Transform popUpSound;
	
	
	void Awake(){
		Instance = this;
	}
	
	// Use this for initialization
	void Start () {
		backBtn.AddInputDelegate(backBtnDelegate);
		nextBtn.AddInputDelegate(nextBtnDelegate);
		okayBtn.AddInputDelegate(okayBtnDelegate);
		exitBtn.AddInputDelegate(exitBtnDelegate);
		okayBtn_OldLady.AddInputDelegate(okayBtn_OldLadyDelegate);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void backBtnDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.TAP:
				Idx--;
				TutorialLogic.Instance.ShowUIImg(stepIdx,Idx,objPos.transform.position);
				UpdateBtnStats();
				break;
		   default:
				break;
		}	
	}	
	
	void nextBtnDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.TAP:
				Idx++;
				TutorialLogic.Instance.ShowUIImg(stepIdx,Idx,objPos.transform.position);
				UpdateBtnStats();
				break;
		   default:
				break;
		}	
	}	
	
	void okayBtnDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.TAP:
				stepIdx = 0;
				Idx = 0;
				TutorialLogic.Instance.DismissUIImg();
				BGpanel.Dismiss();
				UpdateBtnStats();
				break;
		   default:
				break;
		}	
	}	
	
	void exitBtnDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.TAP:
				stepIdx = 0;
				Idx = 0;
				TutorialLogic.Instance.DismissUIImg();
				BGpanel.Dismiss();
				UpdateBtnStats();
				break;
		   default:
				break;
		}	
	}	
		
	void okayBtn_OldLadyDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.TAP:
				CheckOldLadyLogic();
				break;
		   default:
				break;
		}	
	}
	
	public void AwakeOldLadyUI(int idx){
		if(null != TutorialLogic.Instance){
			TutorialLogic.Instance.ShowUI_OldLady(objPos_OladLady.transform.position);
			UpdateOldLadyText(idx);
			BGpanel_OldLady.BringIn();
		}
	}
	
	public void DismissOldLadyUI(){
		if(null != TutorialLogic.Instance){
			TutorialLogic.Instance.DismissOldLady();
			BGpanel_OldLady.Dismiss();
		}
	}
	
	public void AwakeOldManUI(int idx){
		if(null != TutorialLogic.Instance){
			TutorialLogic.Instance.ShowUI_OldMan(objPos_OladMan.transform.position);
			UpdateOldLadyText(idx);
			BGpanel_OldLady.BringIn();
		}
	}
	
	public void DismissOldManUI(){
		if(null != TutorialLogic.Instance){
			TutorialLogic.Instance.DismissOldMan();
			BGpanel_OldLady.Dismiss();
		}
	}
	
	public void AwakeTutrialUI(int triggerId){
		if(null != TutorialLogic.Instance){
			SoundCue.PlayPrefabAndDestroy(popUpSound);
			stepIdx = triggerId;
			Idx		= 0;
			UpdateBtnStats();
			TutorialLogic.Instance.ShowUIImg(stepIdx,Idx,objPos.transform.position);
			UpdateTitle();
			BGpanel.BringIn();
		}
	}
	
	void UpdateBtnStats(){
		if(null != TutorialLogic.Instance){
			int count = TutorialLogic.Instance.GetArrayLength(stepIdx);
			if(0 == Idx){
				if(1 == count){
					backBtn.gameObject.layer  = LayerMask.NameToLayer("Default");
					backText.gameObject.layer = LayerMask.NameToLayer("Default");
					nextBtn.gameObject.layer  = LayerMask.NameToLayer("Default");
					nextText.gameObject.layer = LayerMask.NameToLayer("Default");
					okayBtn.gameObject.layer  = LayerMask.NameToLayer("EZGUI");
					okayText.gameObject.layer = LayerMask.NameToLayer("EZGUI");
					return;
				}
				backBtn.gameObject.layer  = LayerMask.NameToLayer("Default");
				backText.gameObject.layer = LayerMask.NameToLayer("Default");
				nextBtn.gameObject.layer  = LayerMask.NameToLayer("EZGUI");
				nextText.gameObject.layer = LayerMask.NameToLayer("EZGUI");
				okayBtn.gameObject.layer  = LayerMask.NameToLayer("Default");
				okayText.gameObject.layer = LayerMask.NameToLayer("Default");
			}else if((count-1) == Idx){
				backBtn.gameObject.layer  = LayerMask.NameToLayer("EZGUI");
				backText.gameObject.layer = LayerMask.NameToLayer("EZGUI");
				nextBtn.gameObject.layer  = LayerMask.NameToLayer("Default");
				nextText.gameObject.layer = LayerMask.NameToLayer("Default");
				okayBtn.gameObject.layer  = LayerMask.NameToLayer("EZGUI");
				okayText.gameObject.layer = LayerMask.NameToLayer("EZGUI");
			}else{
				backBtn.gameObject.layer  = LayerMask.NameToLayer("EZGUI");
				backText.gameObject.layer = LayerMask.NameToLayer("EZGUI");
				nextBtn.gameObject.layer  = LayerMask.NameToLayer("EZGUI");
				nextText.gameObject.layer = LayerMask.NameToLayer("EZGUI");
				okayBtn.gameObject.layer  = LayerMask.NameToLayer("Default");
				okayText.gameObject.layer = LayerMask.NameToLayer("Default");
			}
			UpdateTitle();
		}
	}
	
	void UpdateTitle(){
		int imgIdx = TutorialLogic.Instance.GetImgIdx(stepIdx,Idx);
		string fileName = "Tutorial.Info";
		string _fileName = LocalizeManage.Instance.GetLangPath(fileName);
		TextAsset item = (TextAsset)Resources.Load(_fileName, typeof(TextAsset));
		string[] itemRowsList = item.text.Split('\n');
		for (int i = 3; i < itemRowsList.Length; ++i){		
			string pp = itemRowsList[i];		
			string[] vals = pp.Split(new char[] { '	', '	' });	
			if(int.Parse(vals[0]) == imgIdx){	
				TitleText.Text = vals[1];
				Msg1Text.Text = vals[2];
				Msg2Text.Text = vals[3];
				return;
			}	
		}
	}
	
	void UpdateOldLadyText(int id){
		string fileName = "Tutorial.OldLady";
		string _fileName = LocalizeManage.Instance.GetLangPath(fileName);
		TextAsset item = (TextAsset)Resources.Load(_fileName, typeof(TextAsset));
		string[] itemRowsList = item.text.Split('\n');
		for (int i = 3; i < itemRowsList.Length; ++i){		
			string pp = itemRowsList[i];		
			string[] vals = pp.Split(new char[] { '	', '	' });	
			if(int.Parse(vals[0]) == id){	
				OldLadyNameText.Text = vals[1];
				OldLadyMsgText.Text  = vals[2];
				return;
			}	
		}
	}
	
	void CheckOldLadyLogic(){
		switch(stepOldLady){
		case 0:
			DismissOldManUI();
			AwakeOldLadyUI(1);
			break;
		case 1:
			DismissOldLadyUI();
			AwakeOldManUI(2);
			break;
		case 2:
			DismissOldManUI();
			AwakeTutrialUI(0);
			break;
		case 3:
			DismissOldLadyUI();
			AwakeOldLadyUI(4);
			break;
		case 4:
			DismissOldLadyUI();
			AwakeOldLadyUI(5);
			break;
		case 5:
			DismissOldLadyUI();
			break;
		default:
			DismissOldLadyUI();
			break;
		}
		stepOldLady++;
	}
}
