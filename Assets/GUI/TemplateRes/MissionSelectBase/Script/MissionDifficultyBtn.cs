using UnityEngine;
using System.Collections;

public class MissionDifficultyBtn : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	#region Interface
	public void SetSelectBar(MissionDifficultyData data){
		NGUITools.SetActive(star.gameObject,data.isPass);
		NGUITools.SetActive(lockSprite.gameObject,data.isLock);
		if(data.isLock){
			background.color = new Color(0.196f,0.196f,0.196f,1);
			lvIcon.color = new Color(0.196f,0.196f,0.196f,1);
			NGUITools.SetActive(info.gameObject,false);
		}
		else{
			background.color = new Color(0.196f,0.196f,0.117f,1);
			lvIcon.color = data.color;
			NGUITools.SetActive(info.gameObject,true);
		}
		
		exp.text = data.exp.ToString();
		karma.text = data.karma.ToString();
	}
	#endregion
	
	#region Local
	[SerializeField]
	private UISprite star;
	
	[SerializeField]
	private UISprite lvIcon;

	[SerializeField]
	private UISprite lockSprite;
	
	[SerializeField]
	private UILabel exp;
	
	[SerializeField]
	private UILabel karma;
	
	[SerializeField]
	private UISlicedSprite background;
	
	[SerializeField]
	private Transform info;
	#endregion
}
