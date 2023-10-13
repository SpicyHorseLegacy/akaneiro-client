using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class SummonRewardManager : MonoBehaviour {
	
	public static SummonRewardManager Instance;
	
	void Awake() {
		Instance = this;
		GUIManager.Instance.AddTemplateInitEnd();
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	#region Interface
	[SerializeField]
	private SummonRewardContent templateContent;
	[SerializeField]
	private Transform root;
	private List<Transform> list = new List<Transform>();
	public void AddElement(string iconName,int karmaVal,int xpVal) {
		//create//
		GameObject obj  =(GameObject)Instantiate(templateContent.gameObject);
		obj.transform.parent = root.transform;
        obj.transform.localPosition = new Vector3(0,0,0); 
        obj.transform.localScale= new Vector3(1,1,1);
		//update info//
		obj.GetComponent<SummonRewardContent>().icon.spriteName = iconName;
		obj.GetComponent<SummonRewardContent>().karmaVal.text = karmaVal.ToString();
		obj.GetComponent<SummonRewardContent>().xpVal.text = xpVal.ToString();
		list.Add(obj.transform);
	}
	public void ClearList() {
		for(int i =list.Count -1;i> 0;i--) {
			Destroy(list[i].gameObject);
		}
		list.Clear();
	}
	#endregion
	
	#region Local
	public delegate void Handle_ContinueDelegate();
    public event Handle_ContinueDelegate OnContinueDelegate;
	private void ContinueDelegate() {
		if(OnContinueDelegate != null) {
			OnContinueDelegate();
		}
	}
	#endregion
	
}
