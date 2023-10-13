using UnityEngine;
using System.Collections;

public class CreateDisciplineInfo{
	public string disciplineIcon;
	public string disciplineName;
	public string disciplineInfo;
	public string bounesInfo;
	public string masteryInfo;
	public Texture2D abilityIcon;
	public Texture2D abilityIcon2;
	public Texture2D abilityIcon3;
	public string abilityName;
	public string abilityName2;
	public string abilityName3;
	public string abilityInfo;
	public string abilityInfo2;
	public string abilityInfo3;
	public string abilityLevel;
	public string abilityForDisciplineName;
	public Color color;
	
}

public class CreateInfo : MonoBehaviour {
	
	public static CreateInfo Instance;

	void Awake() {
		Instance = this;
	}
	
	void Start () {
		abilityIndex = 1;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	#region Interface
	private CreateDisciplineInfo curObject;
	public void SetCurObject(CreateDisciplineInfo info) {
		curObject = info;
		UpdateInfo();
	}
	public CreateDisciplineInfo GetCurObject() {
		return curObject;
	} 
	#endregion
	private int abilityIndex ;
	#region local
	[SerializeField]
	private UISlicedSprite icon;
	void SetIconSprite(string sSpriteNmae) {
		if(icon != null) {
			icon.spriteName = sSpriteNmae;
		}
	}
	[SerializeField]
	private UILabel discipline;
	void SetDiscipline(string text,Color cColor){
		if(discipline!=null){
			discipline.text = text;
			discipline.effectColor = cColor;
		}
	}
	[SerializeField]
	private UILabel disciplineInfo;
	void SetDisciplineInfo(string text){
		if(disciplineInfo!=null){disciplineInfo.text = text;}
	}
	[SerializeField]
	private UILabel bounesInfo;
	void SetBounesInfo(string text){
		if(bounesInfo!=null){bounesInfo.text = text;}
	}
	[SerializeField]
	private UILabel masteryInfo;
	void SetMasteryInfo(string text){
		if(masteryInfo!=null){masteryInfo.text = text;}
	}
	[SerializeField]
	private UITexture abilityIcon;
	void SetAbilityIconSprite1(Texture2D sSpriteImg){
		if(abilityIcon!=null){abilityIcon.mainTexture = sSpriteImg;}
	}
	[SerializeField]
	private UITexture abilityIcon2;
	void SetAbilityIconSprite2(Texture2D sSpriteImg){
		if(abilityIcon2!=null){abilityIcon2.mainTexture = sSpriteImg;}
	}
	[SerializeField]
	private UITexture abilityIcon3;
	void SetAbilityIconSprite3(Texture2D sSpriteImg){
		if(abilityIcon3!=null){abilityIcon3.mainTexture = sSpriteImg;}
	}
	[SerializeField]
	private UILabel abilityName;
	void SetAbilityName(string text){
		if(abilityName!=null){abilityName.text = text;}
	}
	[SerializeField]
	private UILabel abilityName2;
	void SetAbilityName2(string text){
		if(abilityName2!=null){abilityName2.text = text;}
	}
	[SerializeField]
	private UILabel abilityName3;
	void SetAbilityName3(string text){
		if(abilityName3!=null){abilityName3.text = text;}
	}
	[SerializeField]
	private UILabel abilityLevel;
	void SetAbilityLevel(string text){
		if(abilityLevel!=null){
			abilityLevel.text = text;
			abilityLevel.color = Color.yellow;
		}
	}
	[SerializeField]
	private UILabel abilityAbilityInfo;
	void SetAbilityDisciplineInfo(string text,Color cColor){
		if(abilityAbilityInfo!=null){
			abilityAbilityInfo.text = text;
			abilityAbilityInfo.color = cColor;
		}
	}
	[SerializeField]
	private UILabel abilityInfo;
	void SetAbilityInfo(string text){
		if(abilityInfo!=null){abilityInfo.text = text;}
	}
	[SerializeField]
	private UILabel abilityInfo2;
	void SetAbilityInfo2(string text){
		if(abilityInfo2!=null){abilityInfo2.text = text;}
	}
	[SerializeField]
	private UILabel abilityInfo3;
	void SetAbilityInfo3(string text){
		if(abilityInfo3!=null){abilityInfo3.text = text;}
	}
	
	void setIndex1(){
		abilityIndex = 1 ;
		UpdateInfo();
	}
	void setIndex2(){
		abilityIndex = 2 ;
		UpdateInfo();
	}
	void setIndex3(){
		abilityIndex = 3 ;
		UpdateInfo();
	}
	
	void UpdateInfo(){
		SetIconSprite(curObject.disciplineIcon);
		SetDiscipline(curObject.disciplineName,curObject.color);
		SetDisciplineInfo(curObject.disciplineInfo);
		SetBounesInfo(curObject.bounesInfo);
		SetMasteryInfo(curObject.masteryInfo);
		if (abilityIndex == 1){
			SetAbilityName(curObject.abilityName);
			SetAbilityInfo(curObject.abilityInfo);
		}else if (abilityIndex == 2){
			SetAbilityName(curObject.abilityName2);
			SetAbilityInfo(curObject.abilityInfo2);
		}else if (abilityIndex == 3){
			SetAbilityInfo(curObject.abilityInfo3);
			SetAbilityName(curObject.abilityName3);
		}else{
			SetAbilityName(curObject.abilityName);
			SetAbilityInfo(curObject.abilityInfo);
		}
		SetAbilityIconSprite1(curObject.abilityIcon);
		SetAbilityIconSprite2(curObject.abilityIcon2);
		SetAbilityIconSprite3(curObject.abilityIcon3);
		SetAbilityLevel(curObject.abilityLevel);
		SetAbilityDisciplineInfo(curObject.abilityForDisciplineName,curObject.color	);
		//Debug.Log("OOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO");
	}
	
	#endregion
	
}
