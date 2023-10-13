	using UnityEngine;
using System.Collections;

public class TipManager : MonoBehaviour {

	public static TipManager Instance;
	
	void Awake() {
		Instance = this;
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public Tip curTip;
	public Tip equpTip;

	public delegate void Handle_SellDelegate();
    public event Handle_SellDelegate OnSellDelegate;
	private void _SellDelegate() {
//		GUILogManager.LogErr("_SellDelegate");
		if(OnSellDelegate != null) {
			OnSellDelegate();
		}
	}
	
	public delegate void Handle_UseDelegate();
    public event Handle_UseDelegate OnUseDelegate;
	private void _UseDelegate() {
//		GUILogManager.LogErr("_UseDelegate");
		if(OnUseDelegate != null) {
			OnUseDelegate();
		}
	}
	
	public delegate void Handle_EquipDelegate();
	public event Handle_EquipDelegate OnEquipDelegate;
	private void _EquipDelegate()
	{
		if(OnEquipDelegate != null)
			OnEquipDelegate();
	}
	
	[SerializeField]
	private Transform UseBtn;
	public void HideUseBtn(bool _hide) {
		NGUITools.SetActive(UseBtn.gameObject,!_hide);
		if(_hide == false)
			HideEquipBtn(true);
	}
	
	[SerializeField]
	private Transform EquipBtn;
	public void HideEquipBtn(bool _hide)
	{
		NGUITools.SetActive(EquipBtn.gameObject, !_hide);
	}
	
	[SerializeField]
	private Transform SellBtn;
	public void HideSellBtn(bool _hide) {
		NGUITools.SetActive(SellBtn.gameObject,!_hide);
	}
}
