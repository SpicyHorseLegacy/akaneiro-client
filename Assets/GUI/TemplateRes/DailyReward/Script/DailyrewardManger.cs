using UnityEngine;
using System.Collections;

public class DailyrewardManger : MonoBehaviour {
	
	public static DailyrewardManger Instance;
	
	void Awake() {	Instance = this;}
	
	#region Interface
	[SerializeField]
	private DailyRewardDay [] days;
	public UITexture GetItemIconObj(int idx) {
		return days[idx].itemIcon;
	}
	
	public enum DailyRewardDayState {
		Unknow = 0,
		Receive,
		Max
	}
    // if there is a item to show the daily reward, return true.
    // because there is more setting in txt file.
	public bool InitDayState(int idx,DailyRewardDayState state) {
        if (idx < days.Length)
        {
            switch (state)
            {
                case DailyRewardDayState.Unknow:
                    days[idx].unknowIcon.gameObject.SetActive(true);
                    days[idx].itemIcon.gameObject.SetActive(false);
                    days[idx].receiveIcon.gameObject.SetActive(false);
                    break;
                case DailyRewardDayState.Receive:
                    days[idx].unknowIcon.gameObject.SetActive(false);
                    days[idx].itemIcon.gameObject.SetActive(true);
                    days[idx].receiveIcon.gameObject.SetActive(true);
                    break;
            }
            return true;
        }else
        {
            return false;
        }
	}
	
	public delegate void Handle_ThanksDelegate();
    public event Handle_ThanksDelegate OnThanksDelegate;
	private void ThanksDelegate() {
		if(OnThanksDelegate != null) {
			OnThanksDelegate();
		}
	}
	
	[SerializeField]
	private UITexture curItemImg;
	public UITexture GetCurItemTexture() {
		return curItemImg;
	}
	[SerializeField]
	private UILabel itemName;
	public void SetItemName(string name) {
		itemName.text = name;
	}
	
	public delegate void Handle_NextDelegate();
    public event Handle_NextDelegate OnNextDelegate;
	private void NextDelegate() {
		if(OnNextDelegate != null) {
			OnNextDelegate();
		}
	}
	
	[SerializeField]
	private Transform popUpPanel;
	public void HidePopUpPanel(bool hide) {
		popUpPanel.gameObject.SetActive(!hide);
	}
	#endregion
}
