using UnityEngine;
using System.Collections;

public class TutorialLogic : MonoBehaviour {
	
	public static TutorialLogic Instance = null;
	
	public Texture2D [] triggers1Img;
	public Texture2D [] triggers2Img;
	public Texture2D [] triggers3Img;
	
	public Material  tutrialMat;
	public Transform obj;
	public Transform obj_OldLady;
	public Transform obj_OldMan;
	
	public UIClientTrigger trifferUI;
	
	void Awake(){
		Instance = this;
	}
	
	// Use this for initialization
	void Start () {
		DismissUIImg();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void ShowUI_OldLady(Vector3 pos){
		obj_OldLady.transform.position = pos;
	}
	
	public void DismissOldLady(){
		obj_OldLady.transform.position = new Vector3(999f,999f,999f);
	}
	
	public void ShowUI_OldMan(Vector3 pos){
		obj_OldMan.transform.position = pos;
	}
	
	public void DismissOldMan(){
		obj_OldMan.transform.position = new Vector3(999f,999f,999f);
	}
	
	public void ShowUIImg(int triggerId,int idx,Vector3 pos){
		tutrialMat.mainTexture = GetTutorialImg(triggerId,idx);
		obj.transform.position = pos;
	}
	
	public void DismissUIImg(){
		obj.transform.position = new Vector3(999f,999f,999f);
	}
	
	Texture2D GetTutorialImg(int trifferId,int idx){
		switch(trifferId){
		case 0:
			return triggers1Img[idx];
		case 1:
			return triggers2Img[idx];
		case 2:
			return triggers3Img[idx];
		}
		return null;
	}
	
	public int GetArrayLength(int triggerId){
		switch(triggerId){
		case 0:
			return triggers1Img.Length;
		case 1:
			return triggers2Img.Length;
		case 2:
			return triggers3Img.Length;
		}
		return 0;
	}
	
	public int GetImgIdx(int triggerId,int idx){
		int imgIdx = 0;
		for(int i = 0;i<triggerId;i++){
			imgIdx += GetArrayLength(i);
		}
		imgIdx += idx;
		return imgIdx;
	}
	
	public void UpdateTutrialLogicByHunt(){
		switch(CS_SceneInfo.Instance.gClientMonsterTutial){
		case 2:
			StartCoroutine(Wait1sec());
			break;
		case 11:
			TutorialGate.Instance.door2.CloseDoor();
			break;
		case 22:
			SoundCue.PlayPrefabAndDestroy(Tutorial.Instance.popUpSound);
			TutorialNpc.Instance.HideObj(TutorialNpc.Instance.oldLady1);
			TutorialNpc.Instance.ShowObj(TutorialNpc.Instance.oldLady2,TutorialNpc.Instance.oldLady2Pos.position);
			trifferUI.bTouch = false;
			TutorialGate.Instance.door1.CloseDoor();
			TutorialGate.Instance.door2.CloseDoor();
			break;
		}
	}
	
	private IEnumerator Wait1sec(){
		yield return new WaitForSeconds(1f);
		SoundCue.PlayPrefabAndDestroy(Tutorial.Instance.popUpSound);
		Tutorial.Instance.AwakeTutrialUI(1);
		TutorialGate.Instance.door1.CloseDoor();
	}
}
