using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class LoginInputManager : MonoBehaviour {
	
	public static LoginInputManager Instance;
	
	
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
	public void InitInputState(bool isRemember,string account,string password,string ip,string port) {
		UpdateAccount(isRemember,account,password,ip,port);
	}
	
	[SerializeField]
	private UIInput accountInput;
	public string GetAccountFromInput() {
		return accountInput.text;
	}
	[SerializeField]
	private UIInput passwordInput;
	public string GetPassWordFromInput() {
		return passwordInput.text;
	}
	[SerializeField]
	private UIInput portInput;
	public string GetPortFromInput() {
		return portInput.text;
	}
	[SerializeField]
	private UICheckbox rememberBox;
	public bool GetRememberFlag() {
		return rememberBox.isChecked;
	}
	[SerializeField]
	private UIInput ipInput;
	public void UpdateIP(string ip) {
		ipInput.text = ip;
	}
	public string GetIPFromInput() {
		return ipInput.text;
	}
	
	//Check the format is email.//
	public bool GetEmailValid(){
		string sEmail = "";
		sEmail = accountInput.text;
		Regex r = new Regex(@"^\s*([A-Za-z0-9_+-]+(\.\w+)*@([\w-]+\.)+\w{2,10})\s*$");
		if(r.IsMatch(sEmail)){
			return true;
		}else{
			return false;
		}
	}
	[SerializeField]
	private Transform ipPortRoot;
	public void HideIpAndPort() {
		NGUITools.SetActive(ipPortRoot.gameObject,false);
	}
	[SerializeField]
	private Transform allInputRoot;
	public void HideAllInput() {
		NGUITools.SetActive(allInputRoot.gameObject,false);
	}
	#endregion
	
	#region Local
	private void UpdateAccount(bool isRemember,string account,string password,string ip,string port) {
		rememberBox.isChecked = isRemember;
		if(isRemember) {
			accountInput.text = account;
			passwordInput.text = password;
			ipInput.text = ip;
			portInput.text = port;
		}else {
			accountInput.text = "";
			passwordInput.text = "";
			ipInput.text = "";
			portInput.text = "";
		}
	}
	
	public delegate void Handle_LoginDelegate();
    public event Handle_LoginDelegate OnLoginDelegate;
	private void LoginDelegate() {
		if(OnLoginDelegate != null) {
			OnLoginDelegate();
		}
	}
	
	public delegate void Handle_RegistrationDelegate();
    public event Handle_RegistrationDelegate OnRegistrationDelegate;
	private void RegistrationDelegate() {
		if(OnRegistrationDelegate != null) {
			OnRegistrationDelegate();
		}
	}
	#endregion
}
