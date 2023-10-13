using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.IO;

public class _UI_MiniMap : MonoBehaviour {
	
	//Instance
	public static _UI_MiniMap Instance 	= null;

	public Texture2D  	BgTexture;
	public Material 	materialMesh;
	public Dictionary<int,Transform> monsterList = new Dictionary<int,Transform>();  
	public UIButton 	MBtn;
	
	public Texture2D [] monsterIcons;
	
	void Awake() {
		Instance = this;
	}

	// Use this for initialization
	void Start () {
		Init();
		InitMiniMapInfo();
	}
	
	// Update is called once per frame
	void Update () {
		UpdateMiniMap();
	}
	
	public void OnGUI(){
		DrawIcons();
	}
	
	#region Local
	public Rect	BigViewRect;
	[SerializeField]
	private int [] 	triangles;
	[SerializeField]
	private Vector3 [] vectorArray;
	[SerializeField]
	private Vector3 [] vectorBigArray; 
	private Mesh smallMapMesh;
	private Mesh BigMapMesh;
	public Transform 	ObjMesh;
	public Transform 	ObjMeshBig;
	private void Init() {
		BigViewRect.x = 0;
		BigViewRect.y = 0;
		BigViewRect.width = 1;
		BigViewRect.height = 1;
		smallMapMesh = new  Mesh();
		smallMapMesh.vertices = vectorArray;	
		smallMapMesh.triangles = triangles;
		BigMapMesh = new  Mesh();
		BigMapMesh.vertices = vectorBigArray;	
		BigMapMesh.triangles = triangles;
		UpdateSmUV(1,1);
		UpdateBigUV(1,1);
		ObjMesh.GetComponent<MeshFilter>().mesh = smallMapMesh;
		ObjMeshBig.GetComponent<MeshFilter>().mesh = BigMapMesh;
		MBtn.AddInputDelegate(MDelegate);
	}
	[SerializeField]
	private float 	[]	MapLT_X;
	[SerializeField]
	private float 	[]	MapLT_Z;
	[SerializeField]
	private float 	[]	MapRB_X;
	[SerializeField]
	private float 	[]	MapRB_Z;
	[SerializeField]
	private float 	[]	MapWidth;
	[SerializeField]
	private float 	[]	MapHeight;
	[SerializeField]
	private string 	[]	MapImgName;
	//congfig file /res<pos,mapName ...>
	private void InitMiniMapInfo() {
		string _fileName = LocalizeManage.Instance.GetLangPath("MiniMapInfo.Info");
		TextAsset item = (TextAsset)Resources.Load(_fileName, typeof(TextAsset));
		string[] itemRowsList = item.text.Split('\n');
		for (int i = 3; i < itemRowsList.Length - 1; ++i){
			string pp = itemRowsList[i];
			string[] vals = pp.Split(new char[] { '	', '	' });
			if(i-3 < MapLT_X.Length) {
				MapLT_X[i-3] = float.Parse(vals[1]);
				MapLT_Z[i-3] = float.Parse(vals[2]);
				MapRB_X[i-3] = float.Parse(vals[3]);
				MapRB_Z[i-3] = float.Parse(vals[4]);
				MapWidth[i-3] = float.Parse(vals[5]);
				MapHeight[i-3] = float.Parse(vals[6]);
				MapImgName[i-3] = vals[7];
			}
		}
	}
	private int CurrentIdx;
	private bool bIShow = false;
	public bool isShowSmallMap 	= false;
	public bool isShowBigMap   	= false;
	public float scaleVal = 0.2f;
	public float scaleBigVal = 0.5f;
	private void UpdateMiniMap() {
		//need or on need.//
		if(isShowSmallMap && null != Player.Instance && _UI_CS_ScreenCtrl.Instance.IsScreenType(_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_INGAME_NORMAL)){
			float scaleW = (Player.Instance.transform.position.x - MapLT_X[CurrentIdx])/MapWidth[CurrentIdx];
			float scaleH = (Player.Instance.transform.position.z - MapRB_Z[CurrentIdx])/MapHeight[CurrentIdx];
			UpdateSmUV(scaleW - scaleVal/2,scaleH - scaleVal/2);
			UpdateBigUV(scaleW - scaleBigVal/2,scaleH - scaleBigVal/2);
			if(isShowBigMap){
				ObjMeshBig.GetComponent<MiniMapHide>().Hide(false);
			}else{
				ObjMeshBig.GetComponent<MiniMapHide>().Hide(true);
			}	
		}
	}
	
	void MDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.TAP:
				if(!ChatBox.Instance.isInputState) {
					if(_UI_MiniMap.Instance.isShowBigMap) {
						_UI_MiniMap.Instance.isShowBigMap = false;
					}else{
						_UI_MiniMap.Instance.isShowBigMap = true;
					}
					MapPicCopy();
				}		
				break;
		   default:
				break;
		}	
	}

	//set mini map info and downLoadMini Texture. //
	public void SetMiniMapIdx(string minimapName){
		if(0 == string.Compare(minimapName,"Aka_TestBox")){
			CurrentIdx = 0;
//			TextureDownLoadingMan.Instance.DownLoadingTexture("Aka_TestBox",ObjMesh);
		}else if(0 == string.Compare(minimapName,"Hub_Village")){
			CurrentIdx = 1;
//			TextureDownLoadingMan.Instance.DownLoadingTexture("Vilalge",ObjMesh);
			_UI_CS_DebugInfo.Instance.SetMissionInfo(false,0,0,0);
		}else if(0 == string.Compare(minimapName,"Hub_Village_Tutorial")){
			CurrentIdx = 1;
//			TextureDownLoadingMan.Instance.DownLoadingTexture("Vilalge",ObjMesh);
			_UI_CS_DebugInfo.Instance.SetMissionInfo(false,0,0,0);
		}else if(0 == string.Compare(minimapName,"A1_M1")){
			CurrentIdx = 2;
//			TextureDownLoadingMan.Instance.DownLoadingTexture("A1M1",ObjMesh);
		}else if(0 == string.Compare(minimapName,"A1_M2")){
			CurrentIdx = 3;
//			TextureDownLoadingMan.Instance.DownLoadingTexture("A1M2",ObjMesh);
		}else if(0 == string.Compare(minimapName,"A1_M3")){
			CurrentIdx = 4;
//			TextureDownLoadingMan.Instance.DownLoadingTexture("A1M3",ObjMesh);
		}else if(0 == string.Compare(minimapName,"A2_M1")){
			CurrentIdx = 5;
//			TextureDownLoadingMan.Instance.DownLoadingTexture("A2M1",ObjMesh);
		}else if(0 == string.Compare(minimapName,"A2_M2")){
			CurrentIdx = 6;
//			TextureDownLoadingMan.Instance.DownLoadingTexture("A2M2",ObjMesh);
		}else if(0 == string.Compare(minimapName,"A2_M3")){
			CurrentIdx = 7;
//			TextureDownLoadingMan.Instance.DownLoadingTexture("A2M3",ObjMesh);
		}else if(0 == string.Compare(minimapName,"A3_M1")){
			CurrentIdx = 8;
//			TextureDownLoadingMan.Instance.DownLoadingTexture("A3M1",ObjMesh);
		}else if(0 == string.Compare(minimapName,"A3_M2")){
			CurrentIdx = 9;
//			TextureDownLoadingMan.Instance.DownLoadingTexture("A3M2",ObjMesh);
		}else if(0 == string.Compare(minimapName,"A3_M3")){
			CurrentIdx = 10;
//			TextureDownLoadingMan.Instance.DownLoadingTexture("A3M3",ObjMesh);
		}else if(0 == string.Compare(minimapName,"A4_M1")){
			CurrentIdx = 11;
//			TextureDownLoadingMan.Instance.DownLoadingTexture("A4M1",ObjMesh);
		}else if(0 == string.Compare(minimapName,"A4_M2")){
			CurrentIdx = 12;
//			TextureDownLoadingMan.Instance.DownLoadingTexture("A4M2",ObjMesh);
		}else if(0 == string.Compare(minimapName,"A4_M3")){
			CurrentIdx = 13;
//			TextureDownLoadingMan.Instance.DownLoadingTexture("A4M3",ObjMesh);
		}else if(0 == string.Compare(minimapName,"A5_M1")){
			CurrentIdx = 14;
//			TextureDownLoadingMan.Instance.DownLoadingTexture("A5M1",ObjMesh);
		}else if(0 == string.Compare(minimapName,"A5_M2")){
			CurrentIdx = 15;
//			TextureDownLoadingMan.Instance.DownLoadingTexture("A5M2",ObjMesh);
		}else if(0 == string.Compare(minimapName,"A5_M3")){
			CurrentIdx = 16;
//			TextureDownLoadingMan.Instance.DownLoadingTexture("A5M3",ObjMesh);
		}else if(0 == string.Compare(minimapName,"A6_M1")){
			CurrentIdx = 17;
//			TextureDownLoadingMan.Instance.DownLoadingTexture("A6M1",ObjMesh);
		}else if(0 == string.Compare(minimapName,"A6_M2")){
			CurrentIdx = 18;
//			TextureDownLoadingMan.Instance.DownLoadingTexture("A6M2",ObjMesh);
		}else if(0 == string.Compare(minimapName,"A6_M3")){
			CurrentIdx = 19;
//			TextureDownLoadingMan.Instance.DownLoadingTexture("A6M3",ObjMesh);
		}else if(0 == string.Compare(minimapName,"A7_M1")){
			CurrentIdx = 20;
//			TextureDownLoadingMan.Instance.DownLoadingTexture("A7M1",ObjMesh);
		}else if(0 == string.Compare(minimapName,"A7_M2")){
			CurrentIdx = 21;
//			TextureDownLoadingMan.Instance.DownLoadingTexture("A7M2",ObjMesh);
		}else if(0 == string.Compare(minimapName,"A7_M3")){
			CurrentIdx = 22;
//			TextureDownLoadingMan.Instance.DownLoadingTexture("A7M3",ObjMesh);
		}else if(0 == string.Compare(minimapName,"A1_Pit1")){
			CurrentIdx = 99;
//			TextureDownLoadingMan.Instance.DownLoadingTexture("A1M1",ObjMesh);
		}else if(0 == string.Compare(minimapName,"2012E3Demo")){
			CurrentIdx = 3;
//			TextureDownLoadingMan.Instance.DownLoadingTexture("A1M2",ObjMesh);
		}else if(0 == string.Compare(minimapName,"TWILIGHT")){
			CurrentIdx = 3;
//			TextureDownLoadingMan.Instance.DownLoadingTexture("A1M2",ObjMesh);
		}else if(0 == string.Compare(minimapName,"A1_M4")){
			CurrentIdx = 23;
		}else if(0 == string.Compare(minimapName,"A2_M4")){
			CurrentIdx = 24;
		}else if(0 == string.Compare(minimapName,"A3_M4")){
			CurrentIdx = 25;
		}else if(0 == string.Compare(minimapName,"A4_M4")){
			CurrentIdx = 26;
		}
		if(CurrentIdx < MapImgName.Length) {
			TextureDownLoadingMan.Instance.DownLoadingTexture(MapImgName[CurrentIdx],ObjMesh);
		}else {
			TextureDownLoadingMan.Instance.DownLoadingTexture(MapImgName[0],ObjMesh);
		}
	}
	
	#region Draw Icons
	//player and enemy i use ongui.//
	private void DrawIcons() {
		//need or on nedd draw.//
		if(isShowSmallMap && null != Player.Instance&& _UI_CS_ScreenCtrl.Instance.IsScreenType(_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_INGAME_NORMAL)){
			DrawPlayerPoint();
			DrawEnemy(true);
		}else{
			DrawEnemy(false);
		}
	}
	public UIButton 	playerIcon;
	public UIButton 	playerIconBig;
	private void DrawPlayerPoint(){
		playerIcon.transform.localEulerAngles = new Vector3(0,0,-Player.Instance.GetEulerAngles().y + 45);
		playerIconBig.transform.localEulerAngles = new Vector3(0,0,-Player.Instance.GetEulerAngles().y + 45);
	}
	
	private void DrawEnemy(bool isShow){
		float smallMapW = 3.7f;
		float smallMapH = 3.7f;
		float ScaleMapW = MapWidth[CurrentIdx] / (1 / scaleVal);
		float ScaleMapH = MapHeight[CurrentIdx] / (1 / scaleVal);
		foreach(KeyValuePair<int,Transform> monster in CS_SceneInfo.Instance.MonsterList){
			if(null == monster.Value){	
				continue;
			}
			Transform m;
			
			monsterList.TryGetValue(monster.Key, out m);
			if(m == null){
				LogManager.Log_Warn("miniMap don't find monster.Key:"+monster.Key);
				continue;
				
			}
			if(!isShow){
				m.transform.position = new Vector3(999,999,999);
			}else{
				float offestX = Player.Instance.transform.position.x - monster.Value.transform.position.x;
				float offestZ = Player.Instance.transform.position.z - monster.Value.transform.position.z;
				float realW = offestX * smallMapW / ScaleMapW;
				float realH = offestZ * smallMapH / ScaleMapH;
				
				Matrix4x4 m1 = new Matrix4x4();
				Matrix4x4 m2 = new Matrix4x4();
				Matrix4x4 m3 = new Matrix4x4();
				Matrix4x4 m4 = new Matrix4x4();
				Vector3 sourceVector = new Vector3(0f,0f,0f);
				m1.SetTRS( new Vector3( - realW, - realH,0),Quaternion.Euler(0,0,0),new Vector3(1,1,1));
				m2.SetTRS( Vector3.zero,Quaternion.Euler(0,0,45),new Vector3(1,1,1));
				m3.SetTRS( new Vector3(playerIcon.transform.position.x ,playerIcon.transform.position.y,0),Quaternion.Euler(0,0,0),new Vector3(1,1,1));
				m4 = m3 * (m2 * m1);
				if(realW > 1.4f || realW <-1.4f || realH > 1.4f || realH < -1.4f){
					if(null != m)	{	
						m.transform.position = new Vector3(m4.MultiplyPoint3x4(sourceVector).x ,m4.MultiplyPoint3x4(sourceVector).y,999);
					}	
				}else{
					if(null != m){
						m.transform.position = new Vector3(m4.MultiplyPoint3x4(sourceVector).x ,m4.MultiplyPoint3x4(sourceVector).y,-1f);
					}	
				}
			}
		}
	}
	#endregion
	#endregion
	
	#region InterFace
	#region monster icon
	public Transform 	MonsterIcon;
	public void AddMonsterIcon(int key,Transform monster){
		Transform monsterBtn = UnityEngine.Object.Instantiate(MonsterIcon)as Transform;
		if(monsterList.ContainsKey(key)){
			monsterList.Remove(key);
		}
		//change icon for minimap
		if(monster.GetComponent<NpcBase>()){
			monsterBtn.GetComponent<UIButton>().SetUVs(new Rect(0,0,1,1));
			if(monster.GetComponent<NpcBase>().IsBoss){
				monsterBtn.GetComponent<UIButton>().SetTexture(monsterIcons[0]);
			}else if(monster.GetComponent<NpcBase>().IsWanted){
				monsterBtn.GetComponent<UIButton>().SetTexture(monsterIcons[1]);
			}else if(monster.GetComponent<NpcBase>().IsObjective){
				monsterBtn.GetComponent<UIButton>().SetTexture(monsterIcons[3]);
			}else{
				monsterBtn.GetComponent<UIButton>().SetTexture(monsterIcons[2]);
			}
		}
		monsterList.Add(key,monsterBtn);	
	}
	public void DelMonsterIcon(int key){
		Transform t;
	    monsterList.TryGetValue(key, out t);
		if(null != t){
			Destroy(t.gameObject);
		}
		monsterList.Remove(key);
	}
	public void ClearMonsterIcon(){
		monsterList.Clear();
	}
	#endregion
	#region UV
	private Vector2 [] 	vectorUVs = new Vector2[6]; 
	private Vector2 [] 	vectorBigUVs = new Vector2[6]; 
	public Matrix4x4 MatrixUv;
	public void UpdateSmUV(float uvX,float uvY){
		float val = scaleVal;
		vectorUVs[0].x = uvX ;
		vectorUVs[0].y = uvY + val ;
		vectorUVs[3].x = uvX ;
		vectorUVs[3].y = uvY + val ;
		vectorUVs[2].x = uvX + val ;
		vectorUVs[2].y = uvY ;
		vectorUVs[4].x = uvX + val ;
		vectorUVs[4].y = uvY ;
		vectorUVs[1].x = uvX + val ;
		vectorUVs[1].y = uvY + val ;
		vectorUVs[5].x = uvX ;
		vectorUVs[5].y = uvY ;
		Matrix4x4 m1 = new Matrix4x4();
		Matrix4x4 m2 = new Matrix4x4();
		Matrix4x4 m3 = new Matrix4x4();
		m1.SetTRS( new Vector3(-(uvX + val * 0.5f /*+ xxx*/),-(uvY + val * 0.5f /*+ xxx*/),0),Quaternion.Euler(0,0,0),new Vector3(1,1,1));
		m2.SetTRS( Vector3.zero,Quaternion.Euler(0,0,-45),new Vector3(1,1,1));
		m3.SetTRS( new Vector3((uvX + val * 0.5f /*+ xxx*/),(uvY + val * 0.5f /*+ xxx*/),0),Quaternion.Euler(0,0,0),new Vector3(1,1,1));
		MatrixUv = m3 * (m2 * m1);
		for(int i =0;i<6;i++){
			vectorUVs[i]= MatrixUv.MultiplyPoint3x4(vectorUVs[i]); 
		}
		smallMapMesh.uv = vectorUVs;
	}
	public void UpdateBigUV(float uvX,float uvY){
		float valBig = scaleBigVal;
		vectorBigUVs[0].x = uvX ;
		vectorBigUVs[0].y = uvY + valBig ;
		vectorBigUVs[3].x = uvX ;
		vectorBigUVs[3].y = uvY + valBig ;
		vectorBigUVs[2].x = uvX + valBig ;
		vectorBigUVs[2].y = uvY ;
		vectorBigUVs[4].x = uvX + valBig ;
		vectorBigUVs[4].y = uvY ;
		vectorBigUVs[1].x = uvX + valBig ;
		vectorBigUVs[1].y = uvY + valBig ;
		vectorBigUVs[5].x = uvX ;
		vectorBigUVs[5].y = uvY ;
		Matrix4x4 m4 = new Matrix4x4();
		Matrix4x4 m5 = new Matrix4x4();
		Matrix4x4 m6 = new Matrix4x4();	
		m4.SetTRS( new Vector3(-(uvX + valBig * 0.5f /*+ xxx*/),-(uvY + valBig * 0.5f /*+ xxx*/),0),Quaternion.Euler(0,0,0),new Vector3(1,1,1));
		m5.SetTRS( Vector3.zero,Quaternion.Euler(0,0,-45),new Vector3(1,1,1));
		m6.SetTRS( new Vector3((uvX + valBig * 0.5f /*+ xxx*/),(uvY + valBig * 0.5f /*+ xxx*/),0),Quaternion.Euler(0,0,0),new Vector3(1,1,1));
		MatrixUv = m6 * (m5 * m4);
		for(int i =0;i<6;i++){
			vectorBigUVs[i]= MatrixUv.MultiplyPoint3x4(vectorBigUVs[i]); 
		}
		BigMapMesh.uv = vectorBigUVs;
	}
	#endregion
	public void MapPicCopy(){
		ObjMeshBig.GetComponent<MeshRenderer>().materials[0].mainTexture = ObjMesh.GetComponent<MeshRenderer>().materials[0].mainTexture;
	}
	#endregion
	
	

	
	
	
	
	
	
	
	
	

	
	
	
	
	
	
	
	
	
	
}
