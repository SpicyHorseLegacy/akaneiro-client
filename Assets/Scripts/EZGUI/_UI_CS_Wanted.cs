using UnityEngine;
using System.Collections;

public class _UI_CS_Wanted : MonoBehaviour {
	
	public static _UI_CS_Wanted Instance;
	
	public UIPanel 				rewardSelectPanel;
	public SpriteText			NameText;
	public SpriteText			ExpText;
	public SpriteText			SkText;
	public int					objectID;
	
	private static LocalizeManage localizeMgr_ = null;
	void Awake(){
		Instance = this;
		if (localizeMgr_ == null)
        {
            localizeMgr_ = LocalizeFontManager.ManagerInstance;
        }
        localizeMgr_.OnLangChanged += this.UpdateLocalize;
	}
	
	// Use this for initialization
	void Start () {
		rewardSelectPanel.Dismiss();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

#region Local
	private void UpdateLocalize(LocalizeManage.Language _lang) {
      StartCoroutine(RestRewardPanel());
    }
	
	private IEnumerator Endit(){
		yield return new WaitForSeconds(1.5f);
		rewardSelectPanel.Dismiss();
	}
	
	private IEnumerator RestRewardPanel() {
		yield return new WaitForSeconds(0.1f);
		rewardSelectPanel.Dismiss();
	}
#endregion	
	
#region Interface	
	public void AwakeRewardWanted(int objID,int exp,int sk,string name){
		objectID 		= objID;
		ExpText.Text 	= exp.ToString();
		SkText.Text 	= sk.ToString();
		NameText.Text 	= name;	
		rewardSelectPanel.BringIn();
		StartCoroutine(Endit());
	}
#endregion	
	
}
