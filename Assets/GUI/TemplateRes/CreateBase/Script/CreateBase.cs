using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CreateBase : MonoBehaviour {
	
	// Use this for initialization
	public static CreateBase Instance;
	void Awake(){
		Instance = this;
//		GUIManager.Instance.AddTemplateInitEnd();
	}
	
	void Start(){
		//Default//
		_Sex.Set(ESex.eSex_Female);
		curType = AbilityDetailInfo.EDisciplineType.EDT_Fortitude;
	}
	
	#region Interface
	[SerializeField]
	private UILabel inputText;
	public string GetInputText() {
		return inputText.text;
	}
	
	[SerializeField]
	private PlayerModel playerMod;
	public PlayerModel GetPlayerModel() {
		return playerMod;
	}
	
	private ESex _Sex = new ESex();
	public ESex GetSex() {
		return _Sex;
	}
	
	private AbilityDetailInfo.EDisciplineType curType;
	public AbilityDetailInfo.EDisciplineType GetCurType() {
		return curType;
	}
	#endregion
	
	#region local
	public delegate void Handle_ProwessDelegate();
    public event Handle_ProwessDelegate OnProwessDelegate;
	private void ProwessDelegate() {
		curType = AbilityDetailInfo.EDisciplineType.EDT_Prowess;
		if(OnProwessDelegate != null) {
			OnProwessDelegate();
		}
	}
	
	public delegate void Handle_FortitudeDelegate();
    public event Handle_FortitudeDelegate OnFortitudeDelegate;
	private void FortitudeDelegate() {
		curType = AbilityDetailInfo.EDisciplineType.EDT_Fortitude; 
		if(OnFortitudeDelegate != null) {
			OnFortitudeDelegate();
		}
	}
	public delegate void Handle_CunningDelegate();
    public event Handle_CunningDelegate OnCunningDelegate;
	private void CunningDelegate() {
		curType = AbilityDetailInfo.EDisciplineType.EDT_Cunning;
		if(OnCunningDelegate != null) {
			OnCunningDelegate();
		}
	}
	
	public delegate void Handle_FemaleDelegate();
    public event Handle_FemaleDelegate OnFemaleDelegate;
	private void FemaleDelegate() {
		_Sex.Set(ESex.eSex_Female);
		if(OnFemaleDelegate != null) {
			OnFemaleDelegate();
		}
	}
	
	public delegate void Handle_MaleDelegate();
    public event Handle_MaleDelegate OnMaleDelegate;
	private void MaleDelegate() {
		_Sex.Set(ESex.eSex_Male);
		if(OnMaleDelegate != null) {
			OnMaleDelegate();
		}
	}

	public delegate void Handle_BackDelegate();
    public event Handle_BackDelegate OnBackDelegate;
	private void BackDelegate() {
		if(OnBackDelegate != null) {
			OnBackDelegate();
		}
	}
	
	public delegate void Handle_CreateDelegate();
    public event Handle_CreateDelegate OnCreateDelegate;
	private void CreateDelegate() {
		if(OnCreateDelegate != null) {
			OnCreateDelegate();
		}
	}
	#endregion
	
	
}
