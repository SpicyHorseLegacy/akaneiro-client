using UnityEngine;
using System.Collections;

//脚本目的: 为了避免webside 重定义目标对象导致就客户端冲突.以这个脚本作为嫁接. 使版本相对统一//
public class Ezgui2Ngui : MonoBehaviour {
	
	public void APIUrl(string _url) {
		VersionManager.Instance.APIUrl(_url);
    }
	public void API1(string _url) {
		VersionManager.Instance.API1(_url);
	}
	public void User(string _user) {
		VersionManager.Instance.User(_user);
	}            
    public void Token(string _token) {
		VersionManager.Instance.Token(_token);
    }
	public void _MessageType(string _msg) {
		VersionManager.Instance._MessageType(_msg);
    }
    public void Auth(string _codes) {
		VersionManager.Instance.Auth(_codes);
    }
    public void SetType(int _type) {
		VersionManager.Instance.SetType(_type);
    }
	public void Version(string _ver) {
		VersionManager.Instance.Version(_ver);
    }
    public void SetSSL(int _type) {
		VersionManager.Instance.SetSSL(_type);
    }           
    public void Links(string a_ipandprot) {
		VersionManager.Instance.Links(a_ipandprot);
    }
    public void Links2(string ip, string p) {
        VersionManager.Instance.Links2(ip,p);
    }
	public void NeedLogin(string needLogin)
	{
		VersionManager.Instance.NeedLogin(needLogin);
	}
}
