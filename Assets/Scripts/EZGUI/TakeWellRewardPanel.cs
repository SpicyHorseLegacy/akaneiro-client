using UnityEngine;
using System.Collections;

public class TakeWellRewardPanel : MonoBehaviour {
	
	public static TakeWellRewardPanel Instance = null;
	
	public int	qualityLevel= 0;
	public int	sizeLevel	= 0;
	public int	lidLevel 	= 0;
	
	
	
	void Awake() {
		Instance = this;
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void GetQualityInfo(int level){	
		string _fileName = LocalizeManage.Instance.GetLangPath("Well.Quality.txt");
		TextAsset item = (TextAsset)Resources.Load(_fileName, typeof(TextAsset));
		string[] itemRowsList = item.text.Split('\n');
		for (int i = 3; i < itemRowsList.Length -1; ++i){
			string pp = itemRowsList[i];	
			string[] vals = pp.Split(new char[] { '	', '	' });	
			if(int.Parse(vals[0]) == level){
			}
		}		
	}
	
	public void GetSizeInfo(int level){	
		string _fileName = LocalizeManage.Instance.GetLangPath("Well.Size.txt");
		TextAsset item = (TextAsset)Resources.Load(_fileName, typeof(TextAsset));
		string[] itemRowsList = item.text.Split('\n');
		for (int i = 3; i < itemRowsList.Length -1; ++i){
			string pp = itemRowsList[i];	
			string[] vals = pp.Split(new char[] { '	', '	' });	
			if(int.Parse(vals[0]) == level){
			}
		}		
	}
	
	public void GetLidInfo(int level){	
		string _fileName = LocalizeManage.Instance.GetLangPath("Well.Lid.txt");
		TextAsset item = (TextAsset)Resources.Load(_fileName, typeof(TextAsset));
		string[] itemRowsList = item.text.Split('\n');
		for (int i = 3; i < itemRowsList.Length -1; ++i){
			string pp = itemRowsList[i];	
			string[] vals = pp.Split(new char[] { '	', '	' });	
			if(int.Parse(vals[0]) == level){
			}
		}		
	}
}
