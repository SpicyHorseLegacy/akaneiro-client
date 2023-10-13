using UnityEngine;
using System.Collections;

public class BuyKarma : MonoBehaviour {
	public static BuyKarma Instance = null;
	
	void Awake() {
		Instance = this;
	}
	
	// Use this for initialization
	void Start () {
		ReadPayKarmaInfo();
		ReadMoneyVal();
		freeBtn.AddInputDelegate(FreeBtnDelegate);	
		
	}

	// Update is called once per frame
	void Update () {
		
	}

#region Local
	public UIButton freeBtn;
	void FreeBtnDelegate(ref POINTER_INFO ptr) {
		switch(ptr.evt) {
		   case POINTER_INFO.INPUT_EVENT.TAP:
				if(WebLoginCtrl.Instance.IsWebLogin) {
					Application.ExternalCall("get_free_pepper");
				}else {
					SendMsg("free");
				}
				break;
		}	
	}
	
	public BuyKarmaInfo [] objList;
	bool ReadMoneyVal(){
		string fileName = "BuyKarma.Costs";
		string _fileName = LocalizeManage.Instance.GetLangPath(fileName);
		TextAsset item = (TextAsset)Resources.Load(_fileName, typeof(TextAsset));
		string[] itemRowsList = item.text.Split('\n');
		for (int i = 3; i < itemRowsList.Length; ++i){
			string pp = itemRowsList[i];
			string[] vals = pp.Split(new char[] { '	', '	' });
			if(int.Parse(vals[0]) == Platform.Instance.platformType.Get()){
				objList[0].karmaText.Text = vals[1];
				objList[1].karmaText.Text = vals[3];
				objList[2].karmaText.Text = vals[5];
				objList[3].karmaText.Text = vals[7];
				objList[4].karmaText.Text = vals[9];
				objList[5].karmaText.Text = vals[11];
				objList[6].karmaText.Text = vals[13];
				objList[7].karmaText.Text = vals[22];
				objList[8].karmaText.Text = vals[24];
				objList[9].karmaText.Text = vals[26];
				objList[10].karmaText.Text = vals[28];
				objList[11].karmaText.Text = vals[30];
				objList[12].karmaText.Text = vals[32];
				objList[13].karmaText.Text = vals[34];
				if(EUserType.eUserType_Kongregate != Platform.Instance.platformType.Get()){
					objList[0].payMoneyText.Text = vals[15] + "$";
					objList[1].payMoneyText.Text = vals[16] + "$";
					objList[2].payMoneyText.Text = vals[17] + "$";
					objList[3].payMoneyText.Text = vals[18] + "$";
					objList[4].payMoneyText.Text = vals[19] + "$";
					objList[5].payMoneyText.Text = vals[20] + "$";
					objList[6].payMoneyText.Text = vals[21] + "$";
					
					objList[7].payMoneyText.Text = vals[36] + "$";
					objList[8].payMoneyText.Text = vals[37] + "$";
					objList[9].payMoneyText.Text = vals[38] + "$";
					objList[10].payMoneyText.Text = vals[39] + "$";
					objList[11].payMoneyText.Text = vals[40] + "$";
					objList[12].payMoneyText.Text = vals[41] + "$";
					objList[13].payMoneyText.Text = vals[42] + "$";
				}else {
					objList[0].payMoneyText.Text = vals[2];
					objList[1].payMoneyText.Text = vals[4];
					objList[2].payMoneyText.Text = vals[6];
					objList[3].payMoneyText.Text = vals[8];
					objList[4].payMoneyText.Text = vals[10];
					objList[5].payMoneyText.Text = vals[12];
					objList[6].payMoneyText.Text = vals[14];
					
					objList[7].payMoneyText.Text = vals[23];
					objList[8].payMoneyText.Text = vals[25];
					objList[9].payMoneyText.Text = vals[27];
					objList[10].payMoneyText.Text = vals[29];
					objList[11].payMoneyText.Text = vals[31];
					objList[12].payMoneyText.Text = vals[33];
					objList[13].payMoneyText.Text = vals[35];
				}
				return true;
			}	
		}
		return false;
	}
	
	public string shardInfo;
	public string carystalsInfo;
	//1: karma shard 2: karma crystal 
	void ReadPayKarmaInfo(){
		string fileName = "BuyKarma.Info";
		string _fileName = LocalizeManage.Instance.GetLangPath(fileName);
		TextAsset item = (TextAsset)Resources.Load(_fileName, typeof(TextAsset));
		string[] itemRowsList = item.text.Split('\n');

		for (int i = 3; i < itemRowsList.Length; ++i){		
			string pp = itemRowsList[i];		
			string[] vals = pp.Split(new char[] { '	', '	' });	
				BuyKarmaPanel.Instance.InfoText.Text = vals[0];
				if(i == 3){
					shardInfo = vals[0];
				}else if(i == 4){
					carystalsInfo = vals[0];
					return;
				}
		}	
	}
	
	private void SendMsg(string text) {
		if (Steamworks.activeInstance != null)
		{
			Steamworks.activeInstance.StartPayment(text);
		} 
		else if(WebLoginCtrl.Instance.IsWebLogin)
		{
			Application.ExternalCall("select_gold", text);
		}
		else if(ClientLogicCtrl.Instance.isClientVer)
		{
			StartCoroutine(ClientLogicCtrl.Instance.SendMsgToServerForPay(text));
		}
	}
	
#endregion

#region Interface
	/// <summary>
	/// Inits the platform icon. icon from unity scence/gui/_PersistentScene find-->UI manager-->platform<scription> icons
	/// </summary>
	public void InitPlatformIcon(){
		for(int i = 0;i<14;i++){
			objList[i].icon.SetTexture(Platform.Instance.platformIcon[Platform.Instance.platformType.Get()]);
		}
		if(EUserType.eUserType_SpicyHorse != Platform.Instance.platformType.Get()){
			freeBtn.transform.position = new Vector3(999f,999f,999f);
		}
	}
	
	/// <summary>
	/// Sends the buy karma.
	/// if player click item button.it will send msg buy it.
	/// </summary>
	/// <param name='idx'>
	/// Index.
	/// </param>
	public void SendBuyKarma(int idx){
		switch(idx){
		case 1:
			SendMsg("shard1");
			break;
		case 2:
			SendMsg("shard2");
			break;
		case 3:
			SendMsg("shard3");
			break;
		case 4:
			SendMsg("shard4");
			break;
		case 5:
			SendMsg("shard5");
			break;
		case 6:
			SendMsg("shard6");
			break;
		case 7:
			SendMsg("shard7");
			break;
		case 8:
			SendMsg("crystal1");
			break;
		case 9:
			SendMsg("crystal2");
			break;
		case 10:
			SendMsg("crystal3");
			break;
		case 11:
			SendMsg("crystal4");
			break;
		case 12:
			SendMsg("crystal5");
			break;
		case 13:
			SendMsg("crystal6");
			break;
		case 14:
			SendMsg("crystal7");
			break;
		}
	}
#endregion
	
}
