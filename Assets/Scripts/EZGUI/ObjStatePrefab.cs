using UnityEngine;
using System.Collections;

public class ObjStatePrefab : MonoBehaviour {
	
	public UIButton   	icon;
	public UIButton []  buffSlot;
	public SpriteText	level;
	public UIProgressBar 		hpBar;
	public UIProgressBar 		enBar;
	private Rect rect = new Rect(0,0,1,1);
	private int buffCount = 0;
	
	// Use this for initialization
	void Start () {	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void ClearBuff(){
//		Debug.LogError("clean buff");
		buffCount = 0;
		BuffTips.Instance.DismissTip();
		for(int i = 0;i<buffSlot.Length;i++){
			buffSlot[i].SetUVs(rect);
			buffSlot[i].SetTexture(_UI_CS_Resource.Instance.m_EquipmentIcon[0]);
			buffSlot[i].GetComponent<BuffIcon>().isShow = false;
		}
	}
	
	public void UpdateBuffState(BuffManager man){
		int idx = -1;
		ClearBuff();
		for(int i = 0;i<man.Buffs.Count&&i<12;i++){
			//if(man.Buffs[i].BuffType != kTypeBuff.None){
			
            {
                idx =  GetBuffIconIdx(man.Buffs[i].ID,buffSlot[buffCount].GetComponent<BuffIcon>());
				if(-1 != idx){
					buffSlot[buffCount].SetUVs(rect);
					buffSlot[buffCount].SetTexture(BuffIcons.Instance.icons[idx]);
					buffSlot[buffCount].GetComponent<BuffIcon>().isShow = true;
					buffCount++;
				}
			}
		}
	}
	
	public int GetBuffIconIdx(int id,BuffIcon icon){
		string _fileName = LocalizeManage.Instance.GetLangPath("Ability.Status");
		TextAsset item = (TextAsset)Resources.Load(_fileName, typeof(TextAsset));
		string[] itemRowsList = item.text.Split('\n');
		for (int i = 3; i < itemRowsList.Length; ++i){
			string pp = itemRowsList[i];
		   	string[] vals = pp.Split(new char[] { '	', '	' });	
			if(int.Parse(vals[0]) == id){
				icon.name = vals[1];
				icon.description = vals[11];
				return int.Parse(vals[17]);
			}
		}
		return -1;
	}
	
}
