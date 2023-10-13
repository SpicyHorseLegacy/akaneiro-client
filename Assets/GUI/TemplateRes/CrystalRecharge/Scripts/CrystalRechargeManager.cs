using UnityEngine;
using System.Collections;

public class CrystalRechargeManager : MonoBehaviour {
	
	public static CrystalRechargeManager Instance;
	
	void Awake() {
		Instance = this;
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	#region Interface 
	public delegate void Handle_AddKarmaDelegate(string content);
    public event Handle_AddKarmaDelegate OnAddKarmaDelegate;
	private void _AddKarmaDelegate(string content) {
		if(OnAddKarmaDelegate != null) {
			OnAddKarmaDelegate(content);
		}
	}
	private void _AddKarma1Delegate() {
		_AddKarmaDelegate("crystal1");
	}
	private void _AddKarma2Delegate() {
		_AddKarmaDelegate("crystal2");
	}
	private void _AddKarma3Delegate() {
		_AddKarmaDelegate("crystal3");
	}
	private void _AddKarma4Delegate() {
		_AddKarmaDelegate("crystal4");
	}
	private void _AddKarma5Delegate() {
		_AddKarmaDelegate("crystal5");
	}
	private void _AddKarma6Delegate() {
		_AddKarmaDelegate("crystal6");
	}
	private void _AddKarma7Delegate() {
		_AddKarmaDelegate("crystal7");
	}
	
	[SerializeField]
	private UILabel karmaInfoContent;
	public void SetKarmaInfo(string content) {
		karmaInfoContent.text = content;
	}
	
	public delegate void Handle_ExitDelegate();
    public event Handle_ExitDelegate OnExitDelegate;
	private void _ExitDelegate() {
		if(OnExitDelegate != null) {
			OnExitDelegate();
		}
	}
	
	[SerializeField]
	private UILabel [] karmaVal;
	[SerializeField]
	private UILabel [] payVal;
	public void SetKarmaVal(int idx,string val) {
		karmaVal[idx].text = val.ToString();
	}
	public void SetPayVal(int idx,string val) {
		payVal[idx].text = val.ToString();
	}
	#endregion
}
