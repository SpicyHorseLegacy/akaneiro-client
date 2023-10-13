using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

public class CS_SceneInfo : MonoBehaviour
{

    public static CS_SceneInfo Instance;

    public class TriggerAnimData
    {
        public int id;
        public string Aniname;
        public bool IsLoopAnim;
        public float AnimDelayTime;
        public bool bCollision = false;
        public bool bDisplay = true;
    }

    public class teleportData
    {
        public bool bCameraFollow;
        public Vector3 teleportPos;
        public float fadeintime;
        public float fadeouttime;
        public bool bStart = false;
        public int step = 1;
        public int triggerID = 0;
        public bool bEnterScene = false;

    }

    //[HideInInspector]
    public Transform[] Monsters;

    public Transform[] EmmitObjects;

    //[HideInInspector]
    public Transform[] BreakActorPrefabs;

    public Transform[] SpiritPrefabs;

    public KarmaGroupManager KarmaGroupPrefab;

    public Dictionary<int, Transform> MonsterList = new Dictionary<int, Transform>();
    public Dictionary<int, Transform> ItemList = new Dictionary<int, Transform>();
    public Dictionary<int, SItemEnter> ItemInfoList = new Dictionary<int, SItemEnter>();
    public Dictionary<int, Transform> MiscThingList = new Dictionary<int, Transform>();
    public Dictionary<int, KarmainfoData> KarmaMap = new Dictionary<int, KarmainfoData>();
	
	List<Transform> uselessObjs = new List<Transform>();

    private Dictionary<string, WWW> sceneBaseWWWs = new Dictionary<string, WWW>();

    [HideInInspector]
    public List<AllyNpc> AllyNpcList = new List<AllyNpc>();

    private Dictionary<int, TriggerAnimData> triggerAnimList = new Dictionary<int, TriggerAnimData>();

    [HideInInspector]
    public teleportData mTeleportData = new teleportData();

    public float NpcRangeFactor = 1.0f;

    public bool bShowFps = false;

    public Rect FPSRect = new Rect(10, 40, 50, 20);

    float mTimeSinceStart = 0;

    int iActualFps = 0;

    string strfps = "";
    [HideInInspector]
    public string pShadow = "";

    Dictionary<int, string> ALLWords = new Dictionary<int, string>();

    public Shader ShadowShader = null;

    public Texture DialogBackGround;

    public int DialogWidth = 300;

    public int DialogHeight = 200;

    public GUIStyle CustomDialogStyle = new GUIStyle();

    public class cSpeechCont
    {
        public bool bLoop = false;
        public int NpcTypeID = 0;
        public List<int> WordList;
        public float SpeekTime;
        public Vector2 minPoint;
        public Vector2 maxPoint;
        public bool bInside = false;
        public bool bRandom = false;
    }

    [HideInInspector]
    List<cSpeechCont> gSpeechContList = new List<cSpeechCont>();

    class cQueueWord
    {
        public Transform npc;
        public List<string> Words;
        public Rect npcRect;
        public float SpeekTime;
        public float fTime;
        public int index;
        public bool bLoop;
        public Vector2 minPoint;
        public Vector2 maxPoint;
        public bool bInside = false;
        public bool bRandom = false;
    }

    class CachedDownloadListData
    {
        public bool bDownloaded = false;
        public UnityEngine.Object[] ObjectList = null;
		public WWW DownLoadedWeb = null;
		
		public CachedDownloadListData()
		{
			bDownloaded = false;
			ObjectList = null;
			DownLoadedWeb = null;
		}
		
		public CachedDownloadListData(WWW _tempWWW)
		{
			bDownloaded = false;
			ObjectList = null;
			DownLoadedWeb = _tempWWW;
		}
    }

    class CachedDownloadData
    {
        public bool bDownloaded = false;
        public Transform DownLoadedTransform = null;
        public WWW DownLoadedWeb = null;
		
		public CachedDownloadData()
		{
			bDownloaded = false;
			DownLoadedTransform = null;
			DownLoadedWeb = null;
		}
		
		public CachedDownloadData(WWW _tempWWW)
		{
			bDownloaded = false;
			DownLoadedTransform = null;
			DownLoadedWeb = _tempWWW;
		}
    }

    [HideInInspector]
    List<cQueueWord> SpeekWordList = new List<cQueueWord>();

    Dictionary<string, CachedDownloadListData> CachedAnimationMap = new Dictionary<string, CachedDownloadListData>();

    Dictionary<string, CachedDownloadData> CachedPerfabMap = new Dictionary<string, CachedDownloadData>();

    Dictionary<string, CachedDownloadData> CachedModelMap = new Dictionary<string, CachedDownloadData>();

    Dictionary<string, CachedDownloadData> CachedSoundMap = new Dictionary<string, CachedDownloadData>();

    Dictionary<string, WWW> CachedRelationMap = new Dictionary<string, WWW>();

    List<string> SoundWWWList = new List<string>();

    List<string> PerfabWWWList = new List<string>();

    List<string> ModelWWWList = new List<string>();

    List<string> AnimationWWWList = new List<string>();

    [HideInInspector]
    public int DownLoadThings = 0;

    [HideInInspector]
    public int TotalDownLoadThings = 1;

    //[HideInInspector]
    public string[] ItemPerfabs = null;

    public AllyNpc AllyNpcPrefab = null;

    bool bEnterNewScene = false;

    string mNewMapName = "";

    [HideInInspector]
    public int gClientMonsterTutial = 0;

    [HideInInspector]
    public int gClientBossTypeID = 0;


    [HideInInspector]
    public List<string> LodingInfoTips = null;

    [HideInInspector]
    public float BundleProgress = 0f;

    [HideInInspector]
    public float fMonsterLoaging = 0.1f;

    [HideInInspector]
    public float totalMonsterLoading = 0.1f;

    [HideInInspector]
    public float fInitialProgress = 0f;

    // Use this for initialization
    void Start()
    {
        mTimeSinceStart = Time.realtimeSinceStartup;

        ReadDialogTxt();
        //		ReadFromWordsCsv("Dialog");

        string _fileName = LocalizeManage.Instance.GetLangPath("LoadingBar.Description");
        TextAsset item = (TextAsset)Resources.Load(_fileName, typeof(TextAsset));

        string[] itemRowsList = item.text.Split('\n');

        LodingInfoTips = itemRowsList.ToList();
    }

    private Transform SearchHierarchyForBone(Transform current, string name)
    {
        // check if the current bone is the bone we're looking for, if so return it
        if (current.name == name)
            return current;

        // search through child bones for the bone we're looking for
        for (int i = 0; i < current.GetChildCount(); ++i)
        {
            // the recursive step; repeat the search one step deeper in the hierarchy
            Transform found = SearchHierarchyForBone(current.GetChild(i), name);

            // a transform was returned by the search above that is not null,
            // it must be the bone we're looking for
            if (found != null)
                return found;
        }

        // bone with name was not found
        return null;
    }

    void OnGUI()
    {
        if (Application.platform == RuntimePlatform.Android)
            bShowFps = true;

        if (bShowFps)
        {
            FPSRect.width = 150;
            GUI.Label(FPSRect, "FPS: " + strfps); //+ pShadow);
        }
#if NGUI
#else
        if (_UI_CS_ScreenCtrl.Instance.currentScreenType != _UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_INGAME_NORMAL)
        {
            return;
        }
#endif
        foreach (cQueueWord it in SpeekWordList)
        {
            if (it.bInside)
            {
                bool bErase = false;

                if (Player.Instance.transform.position.x < it.minPoint.x)
                    bErase = true;
                if (Player.Instance.transform.position.z < it.minPoint.y)
                    bErase = true;
                if (Player.Instance.transform.position.x > it.maxPoint.x)
                    bErase = true;
                if (Player.Instance.transform.position.z > it.maxPoint.y)
                    bErase = true;

                if (bErase)
                {
                    SpeekWordList.Remove(it);
                    break;
                }
            }

            Vector3 posOnScreen = Vector3.zero;
			//-------------------------------------------------->>mm
            if (it.npc != null){
                Vector3 neckNpcPostion = SearchHierarchyForBone(it.npc, "Bip001 Neck").position;
                
				if(Camera.mainCamera != null)
				{
	                if (GameObject.Find("CH_Well(Clone)") != null)
	                    posOnScreen = Camera.mainCamera.WorldToScreenPoint(neckNpcPostion);
					else
	                    posOnScreen = Camera.mainCamera.WorldToScreenPoint(neckNpcPostion);	
				}
			}
			//-------------------------------------------------->>#mm
            // (44, 99) represent coordinate of the bottom of the bubble's tail and 30 for the vertical offset
			float _camerapixelheight = 0;
			if(Camera.mainCamera != null) _camerapixelheight = Camera.mainCamera.pixelHeight;
			it.npcRect.Set(posOnScreen.x - 44 + 5, _camerapixelheight - posOnScreen.y - 99 - 30, DialogWidth, DialogHeight); 


            if (it.index >= 0 && it.index < it.Words.Count)
            {
                if (DialogBackGround != null)
                    GUI.DrawTexture(it.npcRect, DialogBackGround);

                GUI.Label(it.npcRect, it.Words[it.index], CustomDialogStyle);
            }

            if (Time.realtimeSinceStartup - it.fTime > it.SpeekTime)
            {
                it.fTime = Time.realtimeSinceStartup;
                it.index += 1;
                if (it.bLoop)
                {
                    if (it.bRandom)
                        it.index = UnityEngine.Random.Range(0, it.Words.Count);
                    else if (it.index >= it.Words.Count)
                        it.index = 0;
                }
                else
                {
                    if (it.bRandom == false && it.index >= it.Words.Count)
                    {
                        SpeekWordList.Remove(it);
                        break;
                    }
                    else if (it.bRandom)
                    {
                        SpeekWordList.Remove(it);
                        break;
                    }
                }

            }

        }
    }


    // Update is called once per frame
    void Update()
    {
        if (mTeleportData != null && mTeleportData.bStart)
        {
            if (mTeleportData.step == 1)
            {
                //fade out
                FadeInOut fade = Camera.mainCamera.GetComponent<FadeInOut>();
                if (fade)
                {
                    fade.ratio -= mTeleportData.fadeouttime * Time.deltaTime;
                    if (fade.ratio < 0.001f)
                    {
                        mTeleportData.step = 2;
                        fade.ratio = 0f;
                    }
                }
                else
                    mTeleportData.step = 2;


            }
            else if (mTeleportData.step == 2)
            {
                mTeleportData.bEnterScene = true;
                Player.Instance.transform.position = Player.Instance.GetComponent<PlayerMovement>().pointOnTheGround(mTeleportData.teleportPos);
                Player.Instance.ReactivePlayer();
                Player.Instance.GetComponent<PlayerMovement>().StopMoveToNextState(true, Player.Instance.IS);
                if (mTeleportData.bCameraFollow)
                {
                    GameCamera camera = GameCamera.Instance;
                    camera.transform.position = Player.Instance.transform.position + Player.Instance.transform.forward * camera.Current_CS.TargetOffset + camera.Current_CS.OffsetToLookPoint;
                }

                if (Player.Instance.AttachedSpirit != null)
                {
                    Player.Instance.AttachedSpirit.transform.position = Player.Instance.transform.position + Player.Instance.AttachedSpirit.Offset;
                }
                foreach (AllyNpc it in AllyNpcList)
                {
                    it.TeleportSpecifiedPos();
                }

                mTeleportData.step = 3;

                Player.Instance.GetComponent<PlayerMovement>().IsFreezed = false;

                Transform akaModel = Player.Instance.GetAnimationModel();

                if (akaModel != null)
                {
                    CS_Main.Instance.g_commModule.SendMessage(ProtocolBattle_SendRequest.UpdateMoveRequest(mTeleportData.teleportPos, mTeleportData.teleportPos, akaModel.eulerAngles.y, CS_SceneInfo.Instance.SyncTime));
                }

            }
            else if (mTeleportData.step == 3)
            {
                //fade in
                FadeInOut fade = Camera.mainCamera.GetComponent<FadeInOut>();

                if (fade)
                {
                    fade.ratio += mTeleportData.fadeintime * Time.deltaTime;
                    if (fade.ratio >= 0.9999f)
                    {
                        mTeleportData.bStart = false;
                        mTeleportData.step = 1;
                        mTeleportData.bEnterScene = false;
                        fade.ratio = 1f;
                    }
                }
                else
                {
                    mTeleportData.bStart = false;
                    mTeleportData.step = 1;
                    mTeleportData.bEnterScene = false;
                }

            }
        }

        if (Time.realtimeSinceStartup - mTimeSinceStart < 1.0f)
        {
            iActualFps += 1;
        }
        else
        {
            float tmp = (float)iActualFps;
            tmp /= 1.0f;
            iActualFps = (int)(tmp + 0.5);

            strfps = iActualFps.ToString();

            iActualFps = 0;

            mTimeSinceStart = Time.realtimeSinceStartup;

        }

        foreach (KeyValuePair<string, ItemDownLoading.ItemDownLoadData> category in ItemDownLoading.CachedObjectMap)
        {
            if (category.Value.bHasDownloaded)
            {
                category.Value.mClickedButtons.Clear();
            }
            else
            {
                if (category.Value.CachedItemWWW != null && category.Value.CachedItemWWW.assetBundle != null)
                {
                    category.Value.ItemObject = category.Value.CachedItemWWW.assetBundle.mainAsset;

                    if (category.Value.ItemObject != null)
                    {
                        foreach (UIButton btn in category.Value.mClickedButtons)
                        {
                            if (btn == null)
                                continue;
                            btn.SetTexture((Texture2D)category.Value.ItemObject);
                        }
                        category.Value.mClickedButtons.Clear();

                        category.Value.bHasDownloaded = true;
                    }
                }
            }
        }
    }

    public void BundleDownloadDone()
    {
        if (mNewMapName.Length > 0)
            StartCoroutine(DownloadScense(mNewMapName));
    }
	
	
	public IEnumerator DownloadScense(string mapName) {
		
		WWW database = null;
		
        if (mapName.ToLower() != "emptyscenes") {
			string url = BundlePath.AssetbundleBaseURL + "Scenes/";
			url += (mapName + ".unity3d");
			database = new WWW(url);
            yield return database;
            AssetBundle temp = database.assetBundle;
        }
        AsyncOperation async = Application.LoadLevelAsync(mapName);
        yield return async;
        if(database != null) {
#if NGUI
			LoadingScreenCtrl.Instance.SetDownLoadProgress("Loading...",1);
#else
			_UI_CS_LoadProgressCtrl.Instance.SetMainCameraTag(mapName, 2);		
#endif
            database.assetBundle.Unload(false);
            database.Dispose();
            database = null;
        }
#if NGUI
		if(mapName.ToLower() == "emptyscenes")
		{
			GUIManager.Instance.ChangeUIScreenState("SelectScreen");
		}
#endif
    }

    public void ClearSceneProgress()
    {
        TotalDownLoadThings = 1;
        BundleProgress = 0f;
        fMonsterLoaging = 1f;
        fInitialProgress = 0f;
    }

    private static LocalizeManage localizeMgr_ = null;
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Instance = this;

        if (localizeMgr_ == null)
        {
            localizeMgr_ = LocalizeFontManager.ManagerInstance;
        }
        localizeMgr_.OnLangChanged += this.setLocalizeText;
    }

    public void PushWWW(string url, WWW web)
    {
        if (!sceneBaseWWWs.ContainsKey(url))
        {
            sceneBaseWWWs.Add(url, web);
        }
    }

    public void PopWWW(string url)
    {
        if (sceneBaseWWWs.ContainsKey(url))
        {
            sceneBaseWWWs.Remove(url);
        }
    }

    public bool DoExist(string url)
    {
        return sceneBaseWWWs.ContainsKey(url);
    }

    public static Vector3 pointOnTheGround(Vector3 pos)
    {
        RaycastHit hitInfo;
        int layer = 1 << LayerMask.NameToLayer("Walkable");
        if (Physics.Raycast(pos + Vector3.up * 20, Vector3.down, out hitInfo, 100f, layer))
            pos.y = hitInfo.point.y;
        return pos;
    }

    #region SYNCTIME
    uint _syncServerTime;
    float _syncClientTime;

    public uint SyncTime
    {
        get
        {
            //Debug.LogError("time : " + Time.time + " || synctime : " + _syncClientTime + " || diff : " + (Time.time - _syncClientTime));
            return (uint)(_syncServerTime + (int)((Time.realtimeSinceStartup - _syncClientTime) * 1000.0f));
        }
    }

    public void On_SyncCurTime(uint curTime)
    {
        _syncServerTime = curTime;
        _syncClientTime = Time.realtimeSinceStartup;
    }
    #endregion

    public void On_EnterMonster(SMonsterEnter npcInfo)
    {
        Transform MonsterPerfab = null;
		if(_bundleTool != null)
		{
	        foreach (NpcBase it in _bundleTool.MonsterPrefabs)
	        {
	            if (it != null)
	            {
	                if (it && it.TypeID == npcInfo.npcID)
	                {
	                    MonsterPerfab = it.transform;
	                    break;
	                }
	            }
	        }
	
	        if (MonsterPerfab)
	        {
				//æ›´æ–°tutorialå¯¹è±¡è¿›å…¥é€»è¾‘//
				if(TutorialMan.Instance.GetTutorialFlag()) {
					TutorialMan.Instance.UpdateObjArrow(npcInfo.npcID,MonsterPerfab,false);
		        }
				
	            if (!MonsterList.ContainsKey(npcInfo.objectID))
	            {
	                Quaternion myRotation = Quaternion.identity;
					Vector3 desPosition = pointOnTheGround(npcInfo.pos);
	                if (MonsterPerfab.GetComponent<Fruit02_NPC>() != null)
	                    desPosition = npcInfo.pos;
	
	                Vector3 WalkDir = Vector3.zero;
	
	                Transform NewMonster = Instantiate(MonsterPerfab) as Transform;
					MonsterList.Add(npcInfo.objectID, NewMonster);
	                NewMonster.position = desPosition;
	                if (MonsterPerfab.GetComponent<NpcBase>() && MonsterPerfab.GetComponent<NpcBase>().IsRandomRotationSpawn)
	                {
	                    float ftemp = UnityEngine.Random.Range(0f, 1f);
	                    WalkDir.x = Mathf.Cos(ftemp * Mathf.PI * 2);
	                    WalkDir.z = Mathf.Sin(ftemp * Mathf.PI * 2);
	                    myRotation = Quaternion.LookRotation(WalkDir);
	                    NewMonster.rotation = myRotation;
	                }
#if NGUI
#else
	                _UI_MiniMap.Instance.AddMonsterIcon(npcInfo.objectID, NewMonster);
#endif	
	                if (NewMonster.GetComponent<NpcBase>())
	                {
	                    NewMonster.GetComponent<NpcBase>().ObjID = npcInfo.objectID;
	                    NewMonster.GetComponent<NpcBase>().Enable(true);
	                    NewMonster.GetComponent<NpcBase>().AttrMan.Attrs[EAttributeType.ATTR_CurHP] = npcInfo.hp;
	                    NewMonster.GetComponent<NpcBase>().AttrMan.Attrs[EAttributeType.ATTR_MaxHP] = npcInfo.hp;
	                    NewMonster.GetComponent<NpcBase>().AttrMan.Attrs[EAttributeType.ATTR_MoveSpeed] = npcInfo.moveSpeed;
	                    NewMonster.GetComponent<NpcBase>().fixPoint = npcInfo.pos;
	                }
	
	                if (!npcInfo.bRayCast)
	                {
	                    Renderer[] NpcRenderers = NewMonster.GetComponentsInChildren<Renderer>();
	                    foreach (Renderer NpcRenderer in NpcRenderers)
	                    {
	                        NpcRenderer.enabled = false;
	                    }
	                    if (NewMonster.GetComponent<NpcCreateModel>())
	                    {
	                        foreach (Transform it in NewMonster.GetComponent<NpcBase>().Weapons)
	                        {
	                            if (it != null)
	                            {
	                                Renderer[] WeaponRenderers = it.GetComponentsInChildren<Renderer>();
	                                foreach (Renderer WeaponRenderer in WeaponRenderers)
	                                {
	                                    WeaponRenderer.enabled = false;
	                                }
	                            }
	                        }
	                    }
	                }
	            }
	        }
	        else
	        {
	            Debug.LogError("[Monster Spawn] Don't have MonsterPerfab " + npcInfo.npcID + " ObjID: " + npcInfo.objectID);
	        }
		}
    }
	
    public void On_BreakableActorEnter(SBreakableActorEnter BreakableActorInfo)
    {
        Transform BreakableActorPerfab = null;
		
		if(_bundleTool != null)
		{
	        foreach (InteractiveObj theActor in _bundleTool.InteractiveObjPrefabs)
	        {
	            if (theActor && theActor.TypeID == BreakableActorInfo.ActorID)
	            {
	                BreakableActorPerfab = theActor.transform;
	                break;
	            }
	        }
	
	        if (BreakableActorPerfab)
	        {
//				//æ›´æ–°tutorialå¯¹è±¡è¿›å…¥é€»è¾‘//
//				if(TutorialMan.Instance.GetTutorialFlag()) {
////					LogManager.Log_Error("Breakable id: "+BreakableActorInfo.ActorID);
//					TutorialMan.Instance.UpdateObjArrow(BreakableActorInfo.ActorID,BreakableActorPerfab);
//		        }
				
	            if (!MiscThingList.ContainsKey(BreakableActorInfo.objectID))
	            {
	                Transform NewActor = (Transform)Instantiate(BreakableActorPerfab);
	                NewActor.position = BreakableActorInfo.pos;
	                NewActor.localScale = BreakableActorInfo.scale;
	                NewActor.eulerAngles = BreakableActorInfo.rotation;
	
	                if (NewActor.GetComponent<InteractiveObj>())
	                {
	                    NewActor.GetComponent<InteractiveObj>().ObjID = BreakableActorInfo.objectID;
	                }
	                NewActor.gameObject.SetActive(true);
	                DontDestroyOnLoad(NewActor.gameObject);
	                MiscThingList.Add(BreakableActorInfo.objectID, NewActor);
	            }
	        }
		}
    }

    public void On_EnterItem(SItemEnter mapIteminfo)
    {
        Transform ItemType = null;
        ItemInfoList.Add(mapIteminfo.objectID, mapIteminfo);
        ItemDropStruct tempItem = ItemDeployInfo.Instance.GetItemObject(mapIteminfo.iteminfo.ID, mapIteminfo.iteminfo.perfrab, (int)mapIteminfo.iteminfo.gem, (int)mapIteminfo.iteminfo.enchant, (int)mapIteminfo.iteminfo.element, (int)mapIteminfo.iteminfo.level);
        if (tempItem == null)
            return;
        ItemType = ItemPrefabs.Instance.GetItemPrefab(tempItem._ItemID, tempItem._TypeID, tempItem._PrefabID);
        string ItemName = "";
        float Val = (tempItem.info_gemVal + tempItem.info_encVal + tempItem.info_eleVal);
#if NGUI		
		ItemName = PlayerDataManager.Instance.GetItemName(tempItem);
#else
        ItemName = _UI_CS_ItemVendor.Instance.GetItemName(tempItem);
#endif
        StartCoroutine(ShowItem(ItemType, mapIteminfo, ItemName, Val, tempItem));
		
		
    }

    public IEnumerator ShowItem(Transform ItemType, SItemEnter mapIteminfo, string name, float val, ItemDropStruct iteminfo)
    {
        yield return new WaitForSeconds(0.2f);

        if (ItemType != null && !ItemList.ContainsKey(mapIteminfo.objectID))
        {
            Vector3 desPosition = pointOnTheGround(mapIteminfo.pos);
            Transform ItemInstance = (Transform)Instantiate(ItemType, desPosition, ItemType.rotation);
            if (ItemInstance.GetComponent<LootDropMoving>())
            {
                ItemInstance.GetComponent<LootDropMoving>()._iteminfo = iteminfo;
                ItemInstance.GetComponent<LootDropMoving>().JumpToGround(false);

            }
            if (ItemInstance.collider)
                ItemInstance.collider.isTrigger = true;
            ItemInstance.GetComponent<Item>().ObjectID = mapIteminfo.objectID;
            ItemInstance.GetComponent<Item>().ItemInfo = mapIteminfo.iteminfo;

            ItemList.Add(mapIteminfo.objectID, ItemInstance);
            ItemInstance.gameObject.SetActive(true);

            if (null != iteminfo)
            {
#if NGUI
                int nbTips = InGameLootTooltipManager._lootsTooltipList.Count;

                GUIManager.Instance.AddTemplate("InGameLootTooltip");
                
                while (InGameLootTooltipManager.LastInstance == null || InGameLootTooltipManager._lootsTooltipList.Count != nbTips+1)
                    yield return new WaitForFixedUpdate();

                Color lootColor = new Color();
                if (val < 1)
                    lootColor = PlayerDataManager.Instance.levelTextColor[0];
                
                else if ((1 - 0.01) < val && val < 6)
                    lootColor = PlayerDataManager.Instance.levelTextColor[1];

                else if ((6 - 0.01) < val && val < 11)
                    lootColor = PlayerDataManager.Instance.levelTextColor[2];
                
                else if ((11 - 0.01) < val && val < 15)
                    lootColor = PlayerDataManager.Instance.levelTextColor[3];
                
                else if ((15 - 0.01) < val)
                    lootColor = PlayerDataManager.Instance.levelTextColor[4];

                InGameLootTooltipManager.LastInstance.Dropped(name, lootColor, ItemInstance);
#else
                _UI_CS_IngameToolTipMan.Instance.AddTip(ItemInstance, name, val, iteminfo);
#endif
            }
			
			//æ›´æ–°tutorialå¯¹è±¡è¿›å…¥é€»è¾‘//
			if(TutorialMan.Instance.GetTutorialFlag()) {
				TutorialMan.Instance.UpdateObjArrow(mapIteminfo.iteminfo.ID,ItemInstance,true);
	        }
        }
    }

    public void On_SetObjectPosition(int objectID, Vector3 position)
    {
        if (MonsterList.ContainsKey(objectID))
        {
            NpcBase theMonster = MonsterList[objectID].GetComponent<NpcBase>();

            if (theMonster != null)
            {
                if (theMonster.FSM.IsInState(theMonster.KnockbackState))
                {
                    //theMonster.KnockbackState.LastMovePosition = position;

                    Vector3 SerVerPos = position;

                    SerVerPos.y = 0f;

                    Vector3 ClientPos = theMonster.transform.position;

                    ClientPos.y = 0f;

                    float dis = Vector3.Distance(SerVerPos, ClientPos);

                    if (dis >= 1f)
                    {
                        //						Debug.Log("Monster " + theMonster.ObjID + "  KnockbackState Foreced to move right position " + dis.ToString());

                        RaycastHit hit;

                        int layer = 1 << LayerMask.NameToLayer("Walkable");

                        Vector3 temp = position;

                        temp.y = 100f;

                        if (Physics.Raycast(temp, Vector3.down, out hit, 100f, layer))
                        {
                            temp.y = hit.point.y;
                        }

                        theMonster.transform.position = temp;

                    }
                }
                else if (theMonster.FSM.IsInState(theMonster.FleeState))
                {
                    Vector3 SerVerPos = position;

                    SerVerPos.y = 0f;

                    Vector3 ClientPos = theMonster.transform.position;

                    ClientPos.y = 0f;

                    float dis = Vector3.Distance(SerVerPos, ClientPos);

                    if (dis >= 1f)
                    {
                        //						Debug.Log("Monster " + theMonster.ObjID + "  Flee Foreced to move right position " + dis.ToString());

                        RaycastHit hit;

                        int layer = 1 << LayerMask.NameToLayer("Walkable");

                        Vector3 temp = position;

                        temp.y = 100f;

                        if (Physics.Raycast(temp, Vector3.down, out hit, 100f, layer))
                        {
                            temp.y = hit.point.y;
                        }

                        theMonster.transform.position = temp;

                    }
                }
                else if (theMonster.LastAnimationState == NpcBase.EAnimationStateType.AniSpawn)
                {
                    RaycastHit hit;

                    int layer = 1 << LayerMask.NameToLayer("Walkable");

                    Vector3 temp = position;

                    temp.y = 100f;

                    if (Physics.Raycast(temp, Vector3.down, out hit, 100f, layer))
                    {
                        temp.y = hit.point.y;
                    }

                    theMonster.SpawnState.LastMovePosition = temp;

                    theMonster.SpawnState.bMove = true;

                    theMonster.bReachTargetPoint = false;

                    Vector3 temp1 = theMonster.transform.position;
                    temp1.y = 0f;
                    Vector3 temp2 = position;
                    temp2.y = 0f;
                    float totalDis = Vector3.Distance(temp1, temp2);

                    theMonster.SpawnState.SetSpeed(totalDis * 1.5f);

                }
                else if (theMonster.LastAnimationState == NpcBase.EAnimationStateType.AniAttack)
                {
                    Vector3 SerVerPos = position;

                    SerVerPos.y = 0f;

                    Vector3 ClientPos = theMonster.transform.position;

                    ClientPos.y = 0f;

                    float dis = Vector3.Distance(SerVerPos, ClientPos);

                    RaycastHit hit;

                    int layer = 1 << LayerMask.NameToLayer("Walkable");

                    Vector3 temp = position;

                    if (Physics.Raycast(temp + Vector3.up * 4, Vector3.down, out hit, 20f, layer))
                    {
                        temp.y = hit.point.y;

                        Vector3 NowPos = theMonster.transform.position;

                        NowPos.y = hit.point.y;

                        theMonster.transform.position = NowPos;

                    }

                    if (dis >= 1f)
                    {
                        //						Debug.Log("Monster " + theMonster.ObjID + " Attack Foreced to move right position " + dis.ToString());

                        theMonster.transform.position = temp;

                    }


                }
            }
        }
    }

    public void On_SetObjectVecPosition(float ftime, int objectID, vectorServerPosition vecpos)
    {
        //Debug.Log("set object pos");
        if (MonsterList.ContainsKey(objectID))
        {
            NpcBase theMonster = MonsterList[objectID].GetComponent<NpcBase>();

            if (theMonster != null)
            {
                theMonster.SetNextMoveTarget(ftime, vecpos);
            }
        }
    }

    public void OnBeginSkill(SUseSkillResult useSkillResult)
    {
        //Debug.Log("Begin Skill : ID : " + useSkillResult.skillID);

        BaseObject sourceObj = GetTargetByID(useSkillResult.sourceObjectID);

        if (!sourceObj)
        {
            Debug.LogError("Begin Skill : ID : " + useSkillResult.sourceObjectID + " is not in map! But he is using Skill ID : " + useSkillResult.skillID);
            return;
        }

        if (useSkillResult.sourceObjectID == 1)
        {
            //it is AKA attack NPC
            if (Player.Instance != null)
                Player.Instance.abilityManager.on_BeginAbility(useSkillResult);
            else { Debug.LogError("Can not find player instance!"); }
        }
        else
        {
            if (MonsterList.ContainsKey(useSkillResult.sourceObjectID))
            {
                int objectID = useSkillResult.sourceObjectID;
                NpcBase theMonster = MonsterList[objectID].GetComponent<NpcBase>();
                theMonster.abilityManager.on_BeginAbility(useSkillResult);
                return;
            }

            foreach (AllyNpc it in AllyNpcList)
            {
                if (useSkillResult.sourceObjectID == it.ObjID)
                {
                    it.abilityManager.on_BeginAbility(useSkillResult);
                    break;
                }
            }
        }
    }

    public void On_UseFightSkillResult(SUseSkillResult useSkillResult)
    {
        //Debug.Log("Sever send back Use skill result!");

        if (useSkillResult.sourceObjectID == 1)
        {
            //it is AKA attack NPC
            if (Player.Instance != null)
                Player.Instance.abilityManager.On_UseAbilityResult(useSkillResult);
            //else{Debug.Log("Can not find player instance!");}
        }
        else
        {
            if (MonsterList.ContainsKey(useSkillResult.sourceObjectID))
            {
                int objectID = useSkillResult.sourceObjectID;
                NpcBase theMonster = MonsterList[objectID].GetComponent<NpcBase>();
                theMonster.TargetObjectID = useSkillResult.destObejctID;
                theMonster.abilityManager.On_UseAbilityResult(useSkillResult);
                return;
            }

            foreach (AllyNpc it in AllyNpcList)
            {
                if (useSkillResult.sourceObjectID == it.ObjID)
                {
                    it.abilityManager.On_UseAbilityResult(useSkillResult);
                    break;
                }
            }

            // it is NPC attack AKA
            //CS_SceneInfo.Instance.AttackPlayer(useSkillResult);
        }
    }

    public void AttackPlayer(SUseSkillResult result)
    {
        //Debug.LogError("attackplayer");
        //BaseObject sourceObj = GetTargetByID(result.sourceObjectID);

        //if (!sourceObj)
        //{
        //    Debug.LogError("ID : " + result.sourceObjectID + " is not in map!");
        //    return;
        //}

        //if (sourceObj.ObjType == ObjectType.Enermy)
        //{
        //    if (MonsterList.ContainsKey(result.sourceObjectID))
        //    {
        //        int objectID = result.sourceObjectID;
        //        NpcBase theMonster = MonsterList[objectID].GetComponent<NpcBase>();

        //        theMonster.TargetObjectID = result.destObejctID;

        //        if (theMonster.bNotifyDead)
        //            return;

        //        if (result.skillID != (int)AbilityIDs.NormalAttack_1H_ID)
        //        {
        //            if (!theMonster.FSM.IsInState(theMonster.abilityManager.GetAbilityByID((uint)result.skillID)))
        //                theMonster.FSM.ChangeState(theMonster.abilityManager.GetAbilityByID((uint)result.skillID));
        //            theMonster.abilityManager.On_UseAbilityResult(result);
        //            return;
        //        }

        //        if (theMonster != null)
        //        {
        //            theMonster.ReceiveAttackResult(result);
        //        }
        //    }
        //    else
        //    {
        //        Debug.LogError(result.sourceObjectID.ToString() + " is not in map, but he is attacking! ");
        //    }
        //}

        //if (sourceObj.ObjType == ObjectType.Ally)
        //{
        //    foreach (AllyNpc it in AllyNpcList)
        //    {
        //        if (result.sourceObjectID == it.ObjID)
        //        {
        //            //it.GetComponent<AllyNpcAbility>().On_UseAbilityResult(result);

        //            it.abilityManager.On_UseAbilityResult(result);

        //            break;
        //        }
        //    }
        //}
    }

    public void OnObjectLeave(int objID)
    {
        if (MonsterList.ContainsKey(objID))
        {
            NpcBase theMonster = GetMonsterByID(objID);
			
#if NGUI
			GUILogManager.LogInfo("On_SpawnnerKilled");
			PlayerDataManager.Instance.CheckMissionProgress((int)_UI_CS_RamusTask.MISSION_TYPE.HUNT,MonsterList[objID].GetComponent<NpcBase>().TypeID,0);
			if (theMonster.IsWanted) {
				if(MissionObjectiveManager.Instance) {
					MissionObjectiveManager.Instance.PlayWantedPanel(theMonster.NpcName,theMonster.RewardWantedExp,theMonster.RewardWantedSk);
				}
			}
            if (Hud_KillChain_Manager.Instance)
                Hud_KillChain_Manager.Instance.UpdateKillChain();
#else			
			 //æ›´æ–°tutorialå¯¹è±¡ç®­å¤´//
			if(TutorialMan.Instance.GetTutorialFlag()) {
				TutorialMan.Instance.UpdateObjArrowRemove(theMonster.TypeID);
			}
            _UI_CS_KillChain.Instance.CalcKillChainTime();
            _UI_CS_MissionLogic.Instance.CheckMissionProgress(_UI_CS_MissionLogic.MissionType.HUNT, MonsterList[objID].GetComponent<NpcBase>().TypeID, 0);
            if (theMonster.IsWanted) {
                _UI_CS_Wanted.Instance.AwakeRewardWanted(objID, theMonster.RewardWantedExp, theMonster.RewardWantedSk, theMonster.NpcName);
            }
#endif
            if (theMonster != null)
            {
                theMonster.NotifyDie();

                //if (theMonster.IsBoss)
                {
                    gClientBossTypeID = theMonster.TypeID;

                    foreach (KeyValuePair<int, Transform> category in MonsterList)
                    {
                        if (category.Value != null && category.Value.gameObject != null)
                        {
                            NpcBase ym = category.Value.GetComponent<NpcBase>();

                            if (ym != null && ym.ObjID != objID && ym.TypeID == theMonster.TypeID)
                            {
                                gClientBossTypeID = -1;
                                break;
                            }
                        }
                    }
                }
            }
#if NGUI
#else
            //æ›´æ–°tutorialé€»è¾‘æ€ªç‰©çŒŽæ€éƒ¨åˆ†//
			if(TutorialMan.Instance.GetTutorialFlag()) {
                gClientMonsterTutial++;
				TutorialMan.Instance.UpdateTutorialHuntState(theMonster.TypeID);
            }
            _UI_MiniMap.Instance.DelMonsterIcon(objID);
#endif
            return;
        }

        if (ItemList.ContainsKey(objID))
        {
            Transform tempItem = ItemList[objID];
			//æ›´æ–°tutorialæ”¶é›†ç‰©å“é€»è¾‘.//
			if(TutorialMan.Instance.GetTutorialFlag()) {
				if(tempItem.GetComponent<LootDropMoving>()) {
					TutorialMan.Instance.UpdateTutorialCollectState(tempItem.GetComponent<LootDropMoving>()._iteminfo._ItemID);
				}
            }
			
			//æ›´æ–°tutorialå¯¹è±¡ç®­å¤´//
			if(TutorialMan.Instance.GetTutorialFlag()) {
				if(tempItem.GetComponent<LootDropMoving>()) {
					TutorialMan.Instance.UpdateObjArrowRemove(tempItem.GetComponent<LootDropMoving>()._iteminfo._ItemID);
				}
			}
			
            Player.Instance.NotifyPickUpItem(tempItem);
            ItemList.Remove(objID);
            ItemInfoList.Remove(objID);
            Destroy(tempItem.gameObject);
            return;

        }

        if (MiscThingList.ContainsKey(objID))
        {
            Transform misc = MiscThingList[objID];
			MiscThingList.Remove(objID);
			
            if (misc)
            {
                if (misc.GetComponent<InteractiveHandler>() != null)
                {
					 //æ›´æ–°tutorialå¯¹è±¡ç®­å¤´//
					if(TutorialMan.Instance.GetTutorialFlag()) {
						TutorialMan.Instance.UpdateObjArrowRemove(misc.GetComponent<InteractiveHandler>().TypeID);
					}
#if NGUI
					PlayerDataManager.Instance.CheckMissionProgress((int)_UI_CS_RamusTask.MISSION_TYPE.DESTORY, misc.GetComponent<InteractiveHandler>().TypeID,0);
#else
                    _UI_CS_MissionLogic.Instance.CheckMissionProgress(_UI_CS_MissionLogic.MissionType.DESTORY, misc.GetComponent<InteractiveHandler>().TypeID, 0);
#endif
                    misc.GetComponent<InteractiveHandler>().GoToHell();
					if(!misc.GetComponent<InteractiveHandler>().DestroyOnDeath)
					{
						uselessObjs.Add(misc);
					}
                    return;
                }


                if (misc.GetComponent<Trap>())
                {
                    misc.GetComponent<Trap>().GoToHell();
                    return;
                }

                DestructAfterTime.DestructGameObjectNow(misc.gameObject);
            }
            return;
        }


        //Player.Instance.FSM.ChangeState(Player.Instance.DS);
        for (int i = 0; i < AllyNpcList.Count; i++)
        {
            if (AllyNpcList[i].ObjID == objID)
            {
                AllyNpcList[i].AttrMan.Attrs[EAttributeType.ATTR_CurHP] = 0;
                if (AllyNpcList[i].FSM != null && !AllyNpcList[i].FSM.IsInState(AllyNpcList[i].DS))
                    AllyNpcList[i].FSM.ChangeState(AllyNpcList[i].DS);

                //Destroy(AllyNpcList[i].gameObject);

                PlayerInfoBar.Instance.RemoveNpc(AllyNpcList[i].stateObj);
                AllyNpcList.RemoveAt(i);

                break;
            }
        }
    }

    /// <summary>
    /// Checks if interactive object and calculate now.
    /// </summary>
    /// <returns>
    /// The if interactive object and calculate now.
    /// </returns>
    /// <param name='useSkillResult'>
    /// If set to <c>true</c> use skill result.
    /// </param>
    public bool CheckIfInteractiveObjAndCalculateNow(SUseSkillResult useSkillResult)
    {
        // check if the target obj is interactive object which doesn't need attack animation to use. Calculate result at once.
        if (MiscThingList.ContainsKey(useSkillResult.destObejctID))
        {
            Transform misc = MiscThingList[useSkillResult.destObejctID];
            if (misc.GetComponent<InteractiveHandler>() && !misc.GetComponent<InteractiveHandler>().NeedsAttackAnimationToUSE)
            {
                return true;
            }
        }
        return false;
    }

    public void ClearALLObjects()
    {
		if(uselessObjs.Count > 0)
		{
			foreach(Transform _obj in uselessObjs)
			{
				if(_obj != null)
					Destroy(_obj.gameObject);
			}
		}
		uselessObjs.Clear();
		
        foreach (KeyValuePair<int, Transform> category in MonsterList)
        {
            if (category.Value != null && category.Value.gameObject != null)
                Destroy(category.Value.gameObject);
        }

        MonsterList.Clear();

        foreach (KeyValuePair<int, Transform> category in ItemList)
        {
            if (category.Value != null && category.Value.gameObject != null)
                Destroy(category.Value.gameObject);
        }

        ItemList.Clear();

        //MiscThingList

        foreach (KeyValuePair<int, Transform> category in MiscThingList)
        {
            if (category.Value != null && category.Value.gameObject != null)
                Destroy(category.Value.gameObject);
        }

        MiscThingList.Clear();

        foreach (KeyValuePair<int, KarmainfoData> category in KarmaMap)
        {
            foreach (Transform it in category.Value.KarmaInstanceList)
            {
                if (it != null && it != null)
                    Destroy(it.gameObject);
            }
        }

        foreach (AllyNpc it in AllyNpcList)
        {
            if (it != null && it.gameObject != null)
                Destroy(it.gameObject);
        }
#if NGUI
#else
        PlayerInfoBar.Instance.ClearAllAllyState();
#endif
        AllyNpcList.Clear();

        KarmaMap.Clear();

        ItemInfoList.Clear();

        if (Player.Instance)
            Player.Instance.AllEnemys.Clear();

        //		//MiniMap
        //		foreach( KeyValuePair<int,Transform> mon in _UI_MiniMap.Instance.monsterList)
        //		{
        ////			if(null != mon.Value){
        ////				Destroy(mon.Value.gameObject);
        ////			}
        ////			
        ////			if(null != mon.Key){
        ////				_UI_MiniMap.Instance.monsterList.Remove(mon.Key);
        ////			}	
        //			
        //			if(null != mon.Value.gameObject){
        //				
        //				Destroy(mon.Value.gameObject);
        //				
        //			}
        //			
        //		}
        //		_UI_MiniMap.Instance.monsterList.Clear();
		
		foreach(CachedDownloadData _data in CachedModelMap.Values)
		{
			if(_data != null && _data.DownLoadedWeb != null)
			{
				_data.DownLoadedWeb.assetBundle.Unload(true);
			}
		}
		
		foreach(CachedDownloadData _data in CachedSoundMap.Values)
		{
			if(_data != null && _data.DownLoadedWeb != null)
			{
				_data.DownLoadedWeb.assetBundle.Unload(true);
			}
		}
		
		foreach(CachedDownloadListData _data in CachedAnimationMap.Values)
		{
			if(_data != null && _data.DownLoadedWeb != null)
			{
				_data.DownLoadedWeb.assetBundle.Unload(true);
			}
		}
		
		foreach(CachedDownloadData _data in CachedPerfabMap.Values)
		{
			if(_data != null && _data.DownLoadedWeb != null)
			{
				
				_data.DownLoadedWeb.assetBundle.Unload(true);
			}
		}
		
		CachedModelMap.Clear();
		CachedSoundMap.Clear();
		CachedAnimationMap.Clear();
		CachedPerfabMap.Clear();
		
		List<Transform> _tempArray = new List<Transform>();
		
		for(int i = 0; i < Monsters.Length; i++)
		{
			if(Monsters[i] != null)
				Destroy(Monsters[i].gameObject);
		}
		Monsters = _tempArray.ToArray();
		
		_tempArray.Clear();
		for(int i = 0; i < BreakActorPrefabs.Length; i++)
		{
			if(BreakActorPrefabs[i] != null)
				_tempArray.Add(BreakActorPrefabs[i]);
		}
		BreakActorPrefabs = _tempArray.ToArray();

		if(_bundleTool)
		{
			_bundleTool.HideSceneThings();
			//_bundleTool.ClearALL();
			//_bundleTool = null;
		}
    }

    public void On_PickupItemFailed(int objectID, EServerErrorType error)
    {
        //if(ItemList.ContainsKey(objID))

        //Debug.LogError("ItemID " + objectID + " Can not Pickup");

        if (ItemList.ContainsKey(objectID))
        {
            Transform item = ItemList[objectID];
            if (item.GetComponent<LootDropMoving>())
            {
                item.GetComponent<LootDropMoving>().JumpToGround(true);
            }
        }
        else
        {
            Debug.LogError("[ItemPick] Pick item : " + objectID + ". But it's gone!");
        }

        LogManager.Log_Debug("ItemID " + objectID + " Can not Pickup");

    }

    public void On_UpdateAnimationNotify(int objectID, int TargetID, short animationID)
    {
        if (MonsterList.ContainsKey(objectID))
        {
            NpcBase theMonster = MonsterList[objectID].GetComponent<NpcBase>();
            if (theMonster != null)
            {
                if (animationID >= (short)NpcBase.EAnimationStateType.AniIdle && animationID < (short)NpcBase.EAnimationStateType.AniMax)
                {
                    theMonster.LastAnimationState = (NpcBase.EAnimationStateType)animationID;

                    if (theMonster.LastAnimationState == NpcBase.EAnimationStateType.AniIdle)
                        theMonster.FSM.ChangeState(theMonster.IS);
                    else if (theMonster.LastAnimationState == NpcBase.EAnimationStateType.AniChase)
                        theMonster.TargetObjectID = TargetID;
                    else if (theMonster.LastAnimationState == NpcBase.EAnimationStateType.AniSleep)
                        theMonster.FSM.ChangeState(theMonster.SleepState);
                    else if (theMonster.LastAnimationState == NpcBase.EAnimationStateType.AniWakeUp)
                        theMonster.FSM.ChangeState(theMonster.WakeupState);
                    else if (theMonster.LastAnimationState == NpcBase.EAnimationStateType.AniSpawn)
                    {
                        theMonster.SpawnAnimIndex = TargetID;
                        theMonster.FSM.ChangeState(theMonster.SpawnState);
                    }
                    else if (theMonster.LastAnimationState == NpcBase.EAnimationStateType.AniFlee)
                    {
                        theMonster.FSM.ChangeState(theMonster.FleeState);
                    }
                    else if (theMonster.LastAnimationState == NpcBase.EAnimationStateType.AniAttack)
                    {
                        theMonster.CurAttackPropertyIndex = TargetID;

                        if (theMonster.CurAttackPropertyIndex >= theMonster.AttackState.AttackArray.Length)
                            theMonster.CurAttackPropertyIndex = theMonster.AttackState.AttackArray.Length - 1;

                        if (theMonster.CurAttackPropertyIndex < 0)
                            theMonster.CurAttackPropertyIndex = 0;
                    }
                }
            }
        }
    }

    public void On_SkillObjectEnter(SSkillObjectEnter skillObjectInfo)
    {
        //Debug.Log("skilobject enter : " + skillObjectInfo.objectTypeID + " || " + skillObjectInfo.skillID);
        BaseObject _caster = GetTargetByID(skillObjectInfo.ownerObjectID);
        BaseAttackableObject _casterObj = _caster.GetComponent<BaseAttackableObject>();
        if (_casterObj)
        {
            if (_casterObj.abilityManager.GetAbilityByID((uint)skillObjectInfo.skillID))
            {
                AbilityObject _obj = _casterObj.abilityManager.GetAbilityByID((uint)skillObjectInfo.skillID).On_SkillObjectEnter(skillObjectInfo);
                if (_obj)
                {
                    _obj.Prepare();
                    MiscThingList.Add(skillObjectInfo.objectID, _obj.transform);
                }
            }
        }
    }

    public void On_SkillObjectActive(int _objID)
    {
        BaseObject _baseobj = GetTargetByID(_objID);
        AbilityObject _obj = _baseobj.GetComponent<AbilityObject>();

        if (_obj)
        {
            _obj.Active();
        }
        else
        {
            Debug.LogWarning("[ABI] Server call back to active object : " + _objID + ". But there isn't object match the ID.");
        }
    }

    // NPC hit player by rock / fireball
    public void On_BulletHit(int objectID, SSkillEffect bulletEffect)
    {
        BaseObject bulletObj = GetTargetByID(objectID);

        DamageSource bulletDamageSource = null;
        if (bulletObj)
        {
            AbilityObject bullet = (AbilityObject)bulletObj;

            bulletDamageSource = bullet.gameObject.AddComponent<DamageSource>() as DamageSource;
            bulletDamageSource.Owner = bullet.DestAbility.Owner;
            bulletDamageSource.SourceObj = bullet.DestAbility.Owner;

            if (bulletObj.GetComponent<OniThrowObj>())
            {
                OniThrowObj _throwObj = bulletObj.GetComponent<OniThrowObj>();
                bulletDamageSource.IsPlayImpactSound = _throwObj.IsPlayImpactSound;
                bulletDamageSource.ImpactSoundPrefab = _throwObj.ImpactSoundPrefab;
                bulletDamageSource.IsPlayImpactVFX = _throwObj.IsPlayImpactVFX;
                bulletDamageSource.ImpactVFXPrefab = _throwObj.ImpactVFXPrefab;
            }
        }

        On_UpdateSkillEffect(bulletDamageSource, bulletEffect);

        if (bulletObj)
        {
            if (MiscThingList.ContainsKey(objectID))
            {
                Destroy(MiscThingList[objectID].gameObject);
                MiscThingList.Remove(objectID);
            }
        }
    }

    // useless
    public void On_RushHit(int objectID, SSkillEffect hitEffect)
    {
        On_UpdateSkillEffect(null, hitEffect);
    }

    public void On_TrapHit(int objectID, SSkillEffect hitEffect)
    {
        BaseObject trapObj = GetTargetByID(objectID);
        On_UpdateSkillEffect(null, hitEffect);
        if (trapObj)
        {
            if (MiscThingList.ContainsKey(objectID))
            {
                Transform misc = MiscThingList[objectID];

                if (misc && misc.GetComponent<Trap>())
                {
                    misc.GetComponent<Trap>().Explorsion();
                }
                return;
            }
        }
    }

    public void On_UpdateAttribution(Transform target, DamageSource source, vectorAttrChange attrVec, bool isCrit)
    {
        on_UpdateAttribution(target, source, attrVec, isCrit, new EStatusElementType(), true);
    }

    private void on_UpdateAttribution(Transform target, DamageSource source, vectorAttrChange attrVec, bool isCrit, EStatusElementType elementType, bool isShowDamageText)
    {
        BaseHitableObject targetObj = target.GetComponent<BaseHitableObject>();
        if (targetObj)
        {
            foreach (SAttributeChange attrChange in attrVec)
            {
                // if target is Player, update all attributions in PlayerData
                if (attrChange.attributeType.Get() == EAttributeType.ATTR_CurHP)
                {
                    targetObj.TakeDamage(attrChange.value, source, isCrit, elementType, isShowDamageText);

                    // show battle info in panel
                    if (source && source.Owner && source.Owner.GetComponent<BaseObject>())
                    {
                        string critString = "";
                        if (isCrit)
                            critString = "(Crit)";
                        string attackString = "attacks";
                        if (attrChange.value > 0)
                            attackString = "heals";
                        string battleInfo = "    \"" + source.Owner.transform.name + "\"(ID : " + source.Owner.GetComponent<BaseObject>().ObjID + ") " + attackString + " \"" + target.transform.name + "\"(ID : " + target.GetComponent<BaseObject>().ObjID + ")";
                        attackString = "takes damage : ";
                        if (attrChange.value > 0)
                            attackString = "heals : ";
                        battleInfo += attackString + attrChange.value + critString + "(" + elementType.GetString() + ")";
                        if (BattleInfoPanel.Instance)
                            BattleInfoPanel.Instance.ADD_Info(battleInfo);
                    }
                }
                else
                {
                    targetObj.AttrMan.Attrs[attrChange.attributeType.Get()] += attrChange.value;
                    if (attrChange.attributeType.Get() == EAttributeType.ATTR_MoveSpeed)
                    {
                        if (target.GetComponent<NpcBase>())
                        {
                            float toss = targetObj.AttrMan.Attrs[attrChange.attributeType.Get()] / 100f;
                            if (target.GetComponent<NpcBase>().CS != null)
                                target.GetComponent<NpcBase>().CS.ChangSpeed(toss, false);
                        }
                    }
                }
            }
        }
        else
        {
            Debug.LogError("No base object script in transform : " + target.name);
        }
    }

    public void On_UpdateStatusEffect(vectorStatusEffects statusEffectVec)
    {
        foreach (SStatusEffect statusEffect in statusEffectVec)
        {
//            Debug.Log("Status ID : " + statusEffect.statustID + " || Type : " + statusEffect.statusOperateType.GetString() + "|| to id : " + statusEffect.destObjectID);
            BaseObject targetBaseObj = GetTargetByID(statusEffect.destObjectID);
            if (statusEffect.destObjectID == -1) targetBaseObj = Player.Instance;
            if (targetBaseObj)
            {
                BaseHitableObject targetObj = targetBaseObj.GetComponent<BaseHitableObject>();
                Transform target = targetObj.transform;

                BaseBuff buff = null;

                if (targetObj.BuffMan && (targetObj.ObjType == ObjectType.Ally || targetObj.ObjType == ObjectType.NPC || targetObj.ObjType == ObjectType.Enermy || targetObj.ObjType == ObjectType.Player))
                {
                    switch (statusEffect.statusOperateType.Get())
                    {
                        case EStatusOperateType.eStatusType_Add:
                            BaseObject sourceObj = GetTargetByID(statusEffect.soureObjectID);
                            if (statusEffect.soureObjectID == -1) sourceObj = Player.Instance;
                            Transform source = null;
                            if (sourceObj) source = sourceObj.transform;
                            buff = targetObj.AddBuff(statusEffect.statustID, source);
                            break;

                        case EStatusOperateType.eStatusType_Update:
                            buff = targetObj.BuffMan.GetBuffByID(statusEffect.statustID);
                            break;

                        case EStatusOperateType.eStatusType_Delete:
                            buff = targetObj.BuffMan.GetBuffByID(statusEffect.statustID);
                            break;

                        case EStatusOperateType.eStatusType_Once:
                            buff = targetObj.BuffMan.GetBuffByID(statusEffect.statustID);
                            break;
                    }
                }

                on_UpdateAttribution(target, buff, statusEffect.attribueVec, false, new EStatusElementType(), true);

                // if delete buff, calculate damage first.
                if (statusEffect.statusOperateType.Get() == EStatusOperateType.eStatusType_Delete && targetObj.BuffMan)
                    targetObj.BuffMan.DeleteBuffByID(statusEffect.statustID);
            }
        }
    }

    public void On_UpdateSkillEffect(DamageSource source, SSkillEffect skillEffect)
    {
        on_UpdateSkillEffect(source, skillEffect, true);
    }

    private void on_UpdateSkillEffect(DamageSource source, SSkillEffect skillEffect, bool isShowDamageText)
    {
        BaseObject targetObj = GetTargetByID(skillEffect.targetID);

        if (targetObj)
        {
            Transform target = targetObj.transform;
            on_UpdateAttribution(target, source, skillEffect.attributeChangeVec, skillEffect.isCritical, skillEffect.elementType, isShowDamageText);
            On_UpdateStatusEffect(skillEffect.statusEffectVec);
        }
    }

    private void on_updateSkillEffectVec(DamageSource source, vectorSkillEffects skillEffectVec, bool isShowDamageText)
    {
        foreach (SSkillEffect skillEffect in skillEffectVec)
        {
            on_UpdateSkillEffect(source, skillEffect, isShowDamageText);
        }
    }

    public void On_UpdateResult(DamageSource source, SUseSkillResult result)
    {
        On_UpdateAttribution(source.Owner, source, result.attributeChangeVec, false);
        On_UpdateStatusEffect(result.statusEffectVec);
        bool isShowDamageText = true;
        on_updateSkillEffectVec(source, result.skillEffectVec, isShowDamageText);
    }

    public BaseObject GetTargetByID(int id)
    {
        BaseObject target = null;
        if (id == 1)
            target = Player.Instance;
        else if (GetMonsterByID(id))
        {
            target = GetMonsterByID(id);
        }
        else if (GetMiscObj(id))
        {
            target = GetMiscObj(id);
        }
        else if (GetAllyObj(id))
        {
            target = GetAllyObj(id);
        }
        return target;
    }

    public NpcBase GetMonsterByID(int id)
    {
        if (MonsterList.ContainsKey(id))
            return MonsterList[id].GetComponent<NpcBase>();

        return null;
    }

    public BaseObject GetMiscObj(int id)
    {
        if (MiscThingList.ContainsKey(id))
        {
            return MiscThingList[id].GetComponent<BaseObject>();
        }

        return null;
    }

    public AllyNpc GetAllyObj(int id)
    {
        foreach (AllyNpc ally in AllyNpcList)
        {
            if (ally.ObjID == id)
            {
                return ally;
            }
        }
        return null;
    }

    public void RemoveMonsterByID(int id)
    {
        if (MonsterList.ContainsKey(id))
            MonsterList.Remove(id);
    }

    public void RemoveMiscThingByID(int id)
    {
        if (MiscThingList.ContainsKey(id))
            MiscThingList.Remove(id);
    }

    public void On_AllyNpcEnter(int objectID)
    {
        foreach (AllyNpc it in AllyNpcList)
        {
            if (it.ObjID <= 0)
            {
                it.ObjID = objectID;
                //				_UI_CS_AllyManager.Instance.AllyAdd();
                it.ObjID = objectID;
                break;
            }
        }
    }

    public void AddMiscToList(int objectID, Transform misc)
    {
        if (!MiscThingList.ContainsKey(objectID))
            MiscThingList.Add(objectID, misc);
    }

    public void AddTriggerAnimation(TriggerAnimData ads)
    {
        if (!triggerAnimList.ContainsKey(ads.id))
            triggerAnimList.Add(ads.id, ads);

    }

    public void RemoveTriggerAnimation(int id)
    {
        if (triggerAnimList.ContainsKey(id))
            triggerAnimList.Remove(id);
    }

    public bool IsTriggerAnimation(int id)
    {
        return triggerAnimList.ContainsKey(id);
    }

    public TriggerAnimData GetTriggerAnimation(int id)
    {
        return triggerAnimList[id];
    }

    public void On_moneyEnter(SMoneyEnter moneyInfo)
    {
        //if( KarmaModelPerfabs == null && moneyInfo.money <= 0)
        //	return;

        StartCoroutine(ShowKarma(moneyInfo));

    }

    public IEnumerator ShowKarma(SMoneyEnter moneyInfo)
    {
        yield return new WaitForSeconds(0.1f);

        KarmaGroupManager _group = Instantiate(KarmaGroupPrefab, moneyInfo.pos, Quaternion.identity) as KarmaGroupManager;
        _group.CreateKarmaWithMoneyInfo(moneyInfo);

        List<Transform> tempList = new List<Transform>();
        tempList.Add(_group.transform);
        if (!KarmaMap.ContainsKey(moneyInfo.objectID))
        {
            KarmainfoData temp = new KarmainfoData();
            temp.KarmaInfo = moneyInfo;
            temp.KarmaInstanceList = tempList;
            KarmaMap.Add(moneyInfo.objectID, temp);
        }
        else
        {
            KarmaMap[moneyInfo.objectID].KarmaInfo = moneyInfo;
            KarmaMap[moneyInfo.objectID].KarmaInstanceList = tempList;
        }

        //List<Transform> tempList = new List<Transform>();

        //Transform destiKarma = null;

        //for(int i = 0; i < moneyInfo.Distribution.Count;i++)
        //{
        //    SServerMapMoney temp = (SServerMapMoney)moneyInfo.Distribution[i];

        //    foreach( Transform it in KarmaModelPerfabs)
        //    {

        //        if(it.GetComponent<KarmaController>() != null && (int)it.GetComponent<KarmaController>().KarmaType == temp.ID)
        //        {
        //            destiKarma = it;
        //            break;
        //        }
        //    }
        //    if( destiKarma != null)
        //    {
        //        for(int j = 0; j < temp.Value;j++)
        //        {
        //            Transform theObj  = Instantiate(destiKarma, moneyInfo.pos,Quaternion.identity) as Transform;
        //           //theObj.GetComponent<KarmaDrop>().id = moneyInfo.objectID;

        //           //tempList.Add(theObj);
        //        }

        //    }

        //}
    }

    public void KillAllEnemy()
    {
        foreach (KeyValuePair<int, Transform> category in MonsterList)
        {
            if (category.Value != null && category.Value.gameObject != null)
            {
                category.Value.GetComponent<NpcBase>().NotifyDie();
                category.Value.GetComponent<NpcBase>().FSM.ChangeState(category.Value.GetComponent<NpcBase>().DS);
            }
        }

        foreach (KeyValuePair<int, Transform> category in MiscThingList)
        {
            if (category.Value != null && category.Value.gameObject != null)
            {
                Destroy(category.Value.gameObject);
            }


        }

        MiscThingList.Clear();

        //MiniMap
        foreach (KeyValuePair<int, Transform> mon in _UI_MiniMap.Instance.monsterList)
        {
            if (null != mon.Value)
                Destroy(mon.Value.gameObject);

            //			_UI_MiniMap.Instance.monsterList.Remove(mon.Key);

        }

        _UI_MiniMap.Instance.monsterList.Clear();

    }

    public void PushNewContent(cSpeechCont cont)
    {
        gSpeechContList.Add(cont);
    }

    public void PopNewContent(Transform npc)
    {
        if (bExistWord(npc))
            return;

        for (int i = 0; i < gSpeechContList.Count; i++)
        {
            BaseObject pObject = npc.GetComponent<BaseObject>();

            if (pObject != null && pObject.ObjType == ObjectType.NPC)
            {
                ShopNpc pShopNpc = pObject.GetComponent<ShopNpc>();

                if (pShopNpc != null)
                {
                    if ((int)pShopNpc.npcType == gSpeechContList[i].NpcTypeID)
                    {
                        ShowSpeechDialog(gSpeechContList[i], pObject.transform);
                        gSpeechContList.RemoveAt(i);
                        break;
                    }
                }

            }
            else if (pObject != null && pObject.ObjType == ObjectType.Enermy)
            {
                NpcBase pEnemy = pObject.GetComponent<NpcBase>();

                if (pEnemy != null)
                {
                    if (pEnemy.TypeID == gSpeechContList[i].NpcTypeID)
                    {
                        ShowSpeechDialog(gSpeechContList[i], pObject.transform);
                        gSpeechContList.RemoveAt(i);
                        break;
                    }
                }
            }
            else if (pObject != null && pObject.ObjType == ObjectType.Ally)
            {
                AllyNpc pAllyNpc = pObject.GetComponent<AllyNpc>();

                if (pAllyNpc != null)
                {
                    if (pAllyNpc.typeID == gSpeechContList[i].NpcTypeID)
                    {
                        ShowSpeechDialog(gSpeechContList[i], pObject.transform);
                        gSpeechContList.RemoveAt(i);
                        break;
                    }
                }
            }


            if (gSpeechContList[i].NpcTypeID == 0 && pObject.ObjType == ObjectType.Player)
            {
                ShowSpeechDialog(gSpeechContList[i], pObject.transform);
                gSpeechContList.RemoveAt(i);
                break;
            }
        }
    }

    void ShowSpeechDialog(cSpeechCont pSpeechCont, Transform npc)
    {
        cQueueWord newWord = new cQueueWord();

        newWord.npc = npc;

        newWord.npcRect = new Rect(0, 0, 0, 0);

        newWord.Words = new List<string>();

        newWord.bRandom = pSpeechCont.bRandom;

        foreach (int idx in pSpeechCont.WordList)
        {
            ///if(idx >= ALLWords.Count )
            //continue;
            if (!ALLWords.ContainsKey(idx))
                continue;

            newWord.Words.Add(ALLWords[idx]);
        }
        newWord.fTime = Time.realtimeSinceStartup;

        newWord.SpeekTime = pSpeechCont.SpeekTime;

        newWord.index = 0;

        if (newWord.bRandom)
            newWord.index = UnityEngine.Random.Range(0, newWord.Words.Count);


        newWord.bLoop = pSpeechCont.bLoop;

        newWord.minPoint = pSpeechCont.minPoint;

        newWord.maxPoint = pSpeechCont.maxPoint;

        newWord.bInside = pSpeechCont.bInside;

        SpeekWordList.Add(newWord);

    }

    bool bExistWord(Transform npc)
    {
        bool bExist = false;

        foreach (cQueueWord it in SpeekWordList)
        {
            if (it.npc == npc)
            {
                bExist = true;
                break;
            }
        }

        return bExist;
    }

    void ReadDialogTxt()
    {
        int iNum = 0;
        string sInf = "";
		ALLWords.Clear();
        string _fileName = LocalizeManage.Instance.GetLangPath("Dialog.Dialog");
        TextAsset item = (TextAsset)Resources.Load(_fileName, typeof(TextAsset));
        string[] itemRowsList = item.text.Split('\n');
        for (int i = 3; i < itemRowsList.Length - 1; ++i)
        {
            string pp = itemRowsList[i];
            string[] vals = pp.Split(new char[] { '	', '	' });
            iNum = int.Parse(vals[0]);
            sInf = vals[1];
            if (iNum >= 0)
                ALLWords.Add(iNum, sInf);
        }
    }

    private void setLocalizeText(LocalizeManage.Language _lang)
    {
        ALLWords.Clear();
        ReadDialogTxt();
    }

    void ReadFromWordsCsv(string strFile)
    {
        List<string[]> ls = CSVHelper.ReadCSVAssert(strFile);//ReadCSV(strFile);

        foreach (string[] it in ls)
        {
            if (it == null)
                continue;

            if (it.Length <= 1)
                continue;

            string temp = "";

            int iNum = -1;
            for (int i = 0; i < it.Length; i++)
            {
                if (i == 0)
                {
                    iNum = int.Parse(it[0]);
                    //					Convert.ToInt32((string)(it[0]));
                }
                else
                {
                    temp += it[i];
                }
            }

            if (iNum >= 0)
                ALLWords.Add(iNum, temp);

        }
    }

    #region Download Bundles
	
	public LoadBundleTool BundleToolPrefab;
	LoadBundleTool _bundleTool = null;

    public IEnumerator DownloadMonsterBundle(string mapName)
    {
//        Debug.LogError("DownloadMonsterBundle : " + mapName);

        mNewMapName = mapName;
		
		string _sceneXMLPath = BundlePath.AssetbundleBaseURL + "XML/SCENE/" + mapName + ".xml";
    //    string _sceneXMLPath = BundlePath.AssetbundleBaseURL + "XML/SCENE/SCENEALLINFO/" + mapName + "_AllBundleInfo.xml";

        WWW _bundleInfo = new WWW(_sceneXMLPath);

        yield return _bundleInfo;
		
		if(BundleToolPrefab != null)
		{
			if(_bundleTool == null)
				_bundleTool = UnityEngine.Object.Instantiate(BundleToolPrefab, transform.position, Quaternion.identity) as LoadBundleTool;
			_bundleTool.transform.parent = transform;
			_bundleTool.LoadBundleBySceneXML(_bundleInfo.text, mapName);
			//_bundleTool.LoadBundleByScene(_bundleInfo.text, mapName);
		}
		else
		{
			Debug.LogError("[Bundle Loader] There isn't a bundle loader prefab, can't create a new loader!");
		}

        #region useless
        /*

        List<string> monsterList = new List<string>();
        List<string> BreakableList = new List<string>();
        List<Transform> BreakablePrefabList = null;

        SoundWWWList.Clear();
        PerfabWWWList.Clear();
        ModelWWWList.Clear();
        AnimationWWWList.Clear();

        BundleProgress = 0f;
        fMonsterLoaging = totalMonsterLoading;
        fInitialProgress = 0f;

        string keyElementStr = "";

        if (BreakActorPrefabs != null && BreakActorPrefabs.Length > 0)
        {
            BreakablePrefabList = BreakActorPrefabs.ToList();
        }
        else
        {
            BreakablePrefabList = new List<Transform>();
        }

        DownLoadThings = 0;
        mNewMapName = mapName;

        string strRelation = mapName + "Relation.assetbundle";

        WWW RelationWWW = null;

        AssetBundle Relationbundle = null;

        if (!CachedRelationMap.ContainsKey(mapName))
        {
            RelationWWW = new WWW(BundlePath.AssetbundleBaseURL + strRelation);
            yield return RelationWWW;
            Relationbundle = RelationWWW.assetBundle;
            CachedRelationMap.Add(mapName, RelationWWW);
        }
        else
        {
            RelationWWW = CachedRelationMap[mapName];

            if (RelationWWW.isDone == false)
                yield return null;

            if (RelationWWW != null)
                Relationbundle = RelationWWW.assetBundle;
        }

        if (Relationbundle != null && Relationbundle.mainAsset != null)
        {
            StringHolder relation = (StringHolder)Relationbundle.mainAsset;

            if (relation.content != null && relation.content.Length > 1)
            {
                DownLoadThings = Convert.ToInt32(relation.content[relation.content.Length - 1]);

                TotalDownLoadThings = DownLoadThings;
                //Debug.LogError("downloading thing : " + TotalDownLoadThings);

                bEnterNewScene = true;

                for (int i = 0; i < relation.content.Length; i++)
                {
                    string infostr = relation.content[i];

                    if (infostr.ToLower() == "monstertype")
                    {
                        keyElementStr = "monstertype";
                        continue;
                    }
                    else if (infostr.ToLower() == "breakabletype")
                    {
                        keyElementStr = "breakabletype";
                        continue;
                    }

                    string[] parts = infostr.Split(':');

                    if (keyElementStr == "monstertype")
                    {
                        if (parts.Length > 1)
                        {
                            monsterList.Add(parts[1]);
                        }
                    }
                    else if (keyElementStr == "breakabletype")
                    {
                        if (parts.Length > 1)
                        {
                            BreakableList.Add(parts[1]);
                        }
                    }
                }
            }
        }

        foreach (string Perfabstr in BreakableList)
        {
            string mapKaystr = Perfabstr.Replace(".assetbundle", "");

            Transform BreakableBase = null;

            if (!CachedPerfabMap.ContainsKey(mapKaystr))
            {
                PerfabWWWList.Add(mapKaystr);

                string strBundle = BundlePath.AssetbundleBaseURL + Perfabstr;
                WWW PerfabWWW = new WWW(strBundle);

                CachedDownloadData newData = new CachedDownloadData(PerfabWWW);
                CachedPerfabMap.Add(mapKaystr, newData);

                _UI_CS_LoadProgressCtrl.Instance.database = PerfabWWW;
                _UI_CS_LoadProgressCtrl.Instance.IsLoad = true;

                if (LodingInfoTips != null && LodingInfoTips.Count > 0)
                    _UI_CS_LoadProgressCtrl.Instance.LoadObjNameText.Text = LodingInfoTips[5];

                float startTime = Time.realtimeSinceStartup;
                yield return PerfabWWW;

                _UI_CS_LoadProgressCtrl.Instance.IsLoad = false;
                DownLoadThings -= 1;
                //Debug.LogError("-1 : " + DownLoadThings + " || " + mapKaystr);

                BundleProgress += (1f / TotalDownLoadThings);

                BreakableBase = ((GameObject)PerfabWWW.assetBundle.mainAsset).transform;

                if (CachedPerfabMap.ContainsKey(mapKaystr))
                {
                    CachedPerfabMap[mapKaystr].bDownloaded = true;
                    CachedPerfabMap[mapKaystr].DownLoadedTransform = BreakableBase;
                }

                BreakablePrefabList.Add(BreakableBase);
            }
            else
            {
                if (CachedPerfabMap[mapKaystr].bDownloaded == false)
                    yield return null;

                if (CachedPerfabMap[mapKaystr].bDownloaded)
                {
                    bool bRepeat = false;
                    foreach (string git in PerfabWWWList)
                    {
                        if (git == mapKaystr)
                        {
                            bRepeat = true;
                            break;
                        }
                    }

                    if (!bRepeat)
                    {
                        DownLoadThings -= 1;
                        //Debug.LogError("-1 : " + DownLoadThings + " || " + mapKaystr);

                        PerfabWWWList.Add(mapKaystr);
                        BundleProgress += (1f / TotalDownLoadThings);
                    }
                }

            }

        }
        if (BreakablePrefabList != null)
            BreakActorPrefabs = BreakablePrefabList.ToArray();

        foreach (string Perfabstr in monsterList)
        {
            string mapKaystr = Perfabstr.Replace(".assetbundle", "");

            bool _isRepeatDownloading = false;
            foreach (string git in PerfabWWWList)
            {
                if (git == mapKaystr)
                {
                    _isRepeatDownloading = true;
                    break;
                }
            }

            if (!_isRepeatDownloading)
            {
                PerfabWWWList.Add(mapKaystr);

                if (!CachedPerfabMap.ContainsKey(mapKaystr))
                {
                    string strBundle = BundlePath.AssetbundleBaseURL + Perfabstr;
                    WWW PerfabWWW = new WWW(strBundle);

                    float startTime = Time.realtimeSinceStartup;

                    CachedDownloadData newData = new CachedDownloadData(PerfabWWW);
                    newData.bDownloaded = false;
                    newData.DownLoadedTransform = null;
                    newData.DownLoadedWeb = PerfabWWW;
                    CachedPerfabMap.Add(mapKaystr, newData);

                    _UI_CS_LoadProgressCtrl.Instance.database = PerfabWWW;
                    _UI_CS_LoadProgressCtrl.Instance.IsLoad = true;

                    if (LodingInfoTips != null && LodingInfoTips.Count > 0)
                        _UI_CS_LoadProgressCtrl.Instance.LoadObjNameText.Text = LodingInfoTips[4];

                    yield return PerfabWWW;

                    _UI_CS_LoadProgressCtrl.Instance.IsLoad = false;
                    DownLoadThings -= 1;
                    //Debug.LogError("-1 : " + DownLoadThings + " || " + mapKaystr);

                    BundleProgress += (1f / TotalDownLoadThings);

                    if (CachedPerfabMap.ContainsKey(mapKaystr))
                    {
                        CachedPerfabMap[mapKaystr].bDownloaded = true;
                        CachedPerfabMap[mapKaystr].DownLoadedTransform = ((GameObject)PerfabWWW.assetBundle.mainAsset).transform;
                    }
                    else
                    {
                        LogManager.Log_Error("CachedPerfabMap.ContainsKey ! fail,name:" + mapKaystr);
                    }

                    StringHolder mainHolder = PerfabWWW.assetBundle.Load("temps", typeof(StringHolder)) as StringHolder;

                    if (mainHolder != null && mainHolder.content != null)
                    {
                        foreach (string it in mainHolder.content)
                        {
                            string[] keystrList = it.Split(';');
                            DownLoadMonsterKey(keystrList);
                        }
                    }
                    else
                    {
                        LogManager.Log_Error("mainHolder.contentis null ! fail,name:");
                    }
                }
                else
                {
                    if (CachedPerfabMap[mapKaystr].bDownloaded == false)
                        yield return null;

                    if (CachedPerfabMap[mapKaystr].bDownloaded)
                    {
                        
                        DownLoadThings -= 1;
                        //Debug.LogError("-1 : " + DownLoadThings + " || " + mapKaystr);

                        BundleProgress += (1f / TotalDownLoadThings);

                        if (CachedPerfabMap[mapKaystr].DownLoadedWeb != null && CachedPerfabMap[mapKaystr].DownLoadedWeb.assetBundle != null)
                        {
                            StringHolder mainHolder = CachedPerfabMap[mapKaystr].DownLoadedWeb.assetBundle.Load("temps", typeof(StringHolder)) as StringHolder;

                            if (mainHolder != null && mainHolder.content != null)
                            {
                                foreach (string it in mainHolder.content)
                                {
                                    string[] keystrList = it.Split(';');

                                    DownLoadMonsterKey(keystrList);
                                }
                            }
                        }
                        else
                        {
                            LogManager.Log_Error("Loaded fail 2:" + mapKaystr);
                        }
                    }
                    else
                    {
                        LogManager.Log_Error("Loaded fail 1:" + mapKaystr);
                    }
                }
            }
        }
         */
        #endregion
    }

    void DownLoadMonsterKey(string[] keyStrs)
    {
        string[] temStrList = keyStrs[0].Split(':');

        if (temStrList.Length > 1)
        {
            string sreKey = temStrList[0].ToLower();

            if (sreKey == "model")
            {
                StartCoroutine(DistribModel(keyStrs));
            }
            else if (sreKey == "animation")
            {
                StartCoroutine(DistribAnimation(keyStrs));
            }
            else
            {
                StartCoroutine(DistribSound(keyStrs));
            }
        }
    }

    IEnumerator DistribModel(string[] stringlst)
    {
        foreach (string it in stringlst)
        {
            string[] temStrList = it.Split(':');

            if (temStrList.Length <= 1)
                continue;

            string bundle = temStrList[1];
            string mapKaystr = bundle.Replace(".assetbundle", "");

            bool _isRepeatDownloading = false;
            foreach (string git in ModelWWWList)
            {
                if (git == mapKaystr)
                {
                    _isRepeatDownloading = true;
                    break;
                }
            }

            if (!_isRepeatDownloading)
            {
                ModelWWWList.Add(mapKaystr);

                if (!CachedModelMap.ContainsKey(mapKaystr))
                {
                    string strBundle = BundlePath.AssetbundleBaseURL + temStrList[1];
                    WWW tempWWW = new WWW(strBundle);

                    CachedDownloadData newData = new CachedDownloadData(tempWWW);
                    CachedModelMap.Add(mapKaystr, newData);

                    _UI_CS_LoadProgressCtrl.Instance.database = tempWWW;
                    _UI_CS_LoadProgressCtrl.Instance.IsLoad = true;

                    if (LodingInfoTips != null && LodingInfoTips.Count > 0)
                        _UI_CS_LoadProgressCtrl.Instance.LoadObjNameText.Text = LodingInfoTips[4];

                    float startTime = Time.realtimeSinceStartup;
                    yield return tempWWW;

                    _UI_CS_LoadProgressCtrl.Instance.IsLoad = false;

                    DownLoadThings -= 1;
                    //Debug.LogError("-1 : " + DownLoadThings + " || " + mapKaystr);

                    BundleProgress += (1f / TotalDownLoadThings);

                    if (CachedModelMap.ContainsKey(mapKaystr))
                    {
                        CachedModelMap[mapKaystr].bDownloaded = true;
                        CachedModelMap[mapKaystr].DownLoadedTransform = (Transform)tempWWW.assetBundle.mainAsset;
                    }
                }
                else
                {
                    if (CachedModelMap[mapKaystr].bDownloaded == false)
                        yield return null;

                    if (CachedModelMap[mapKaystr].bDownloaded)
                    {
                        DownLoadThings -= 1;
                        //Debug.LogError("-1 : " + DownLoadThings + " || " + mapKaystr);
                        BundleProgress += (1f / TotalDownLoadThings);
                    }
                }
            }
        }
    }

    IEnumerator DistribAnimation(string[] stringlst)//,Transform monster)
    {
		string[] temStrList = stringlst[0].Split(':');
		string bundle = temStrList[1];
		
	    if( temStrList[0].ToLower() == "animation")
	    {
            string mapKaystr = bundle.Replace(".assetbundle", "");

            bool _isRepeatDownloading = false;
            foreach (string git in AnimationWWWList)
            {
                if (git == mapKaystr)
                {
                    _isRepeatDownloading = true;
                    break;
                }
            }

            if (!_isRepeatDownloading)
            {
                AnimationWWWList.Add(mapKaystr);

                if (!CachedAnimationMap.ContainsKey(mapKaystr))
                {
                    string strBundle = BundlePath.AssetbundleBaseURL + bundle;
                    WWW tempWWW = new WWW(strBundle);

                    CachedDownloadListData newData = new CachedDownloadListData(tempWWW);
                    CachedAnimationMap.Add(mapKaystr, newData);

                    _UI_CS_LoadProgressCtrl.Instance.database = tempWWW;

                    _UI_CS_LoadProgressCtrl.Instance.IsLoad = true;
                    if (LodingInfoTips != null && LodingInfoTips.Count > 0)
                        _UI_CS_LoadProgressCtrl.Instance.LoadObjNameText.Text = LodingInfoTips[4];

                    float startTime = Time.realtimeSinceStartup;
                    yield return tempWWW;

                    _UI_CS_LoadProgressCtrl.Instance.IsLoad = false;

                    DownLoadThings -= 1;
                    //Debug.LogError("-1 : " + DownLoadThings + " || " + mapKaystr);
                    BundleProgress += (1f / TotalDownLoadThings);

                    UnityEngine.Object[] AnimationClips = null;
                    try
                    {
                        AnimationClips = tempWWW.assetBundle.LoadAll();
                    }
                    catch (NullReferenceException e)
                    {
                        string message = "BUNDLE LOAD FAILURE\n\n";
                        message += "Catched NullReferenceException assetBundle.LoadAll(): " + e.ToString() + "\n\n\n\n";
                        message += "If you think that this message is not your fault,\n";
                        message += "please contact our technical support at http://support.spicyhorse.com/\n\n\n\n";
                        message += "PRESS SPACE TO TERMINATE APPLICATION";
                        BSOD.Error(message);
                        throw e;
                    }

                    if (CachedAnimationMap.ContainsKey(mapKaystr))
                    {
                        CachedAnimationMap[mapKaystr].bDownloaded = true;
                        CachedAnimationMap[mapKaystr].ObjectList = AnimationClips;
                    }
                }
                else
                {
                    if (CachedAnimationMap[mapKaystr].bDownloaded == false)
                        yield return null;

                    if (CachedAnimationMap[mapKaystr].bDownloaded)
                    {
                        DownLoadThings -= 1;
                        //Debug.LogError("-1 : " + DownLoadThings + " || " + mapKaystr);
                        BundleProgress += (1f / TotalDownLoadThings);
                    }
                }
            }
        }
    }

    IEnumerator DistribSound(string[] stringlst)//,Transform monster)
    {
        foreach (string it in stringlst)
        {
            string[] temStrList = it.Split(':');

            if (temStrList.Length <= 1)
                continue;

            string bundle = temStrList[1];
            string mapKaystr = bundle.Replace(".assetbundle", "");

            bool _isRepeatDownloading = false;
            foreach (string git in SoundWWWList)
            {
                if (git == mapKaystr)
                {
                    _isRepeatDownloading = true;
                    break;
                }
            }

            if (!_isRepeatDownloading)
            {
                SoundWWWList.Add(mapKaystr);

                if (!CachedSoundMap.ContainsKey(mapKaystr))
                {
                    string strBundle = BundlePath.AssetbundleBaseURL + bundle;
                    WWW tempWWW = new WWW(strBundle);

                    CachedDownloadData newData = new CachedDownloadData(tempWWW);
                    newData.bDownloaded = false;
                    newData.DownLoadedTransform = null;
                    CachedSoundMap.Add(mapKaystr, newData);

                    _UI_CS_LoadProgressCtrl.Instance.database = tempWWW;

                    _UI_CS_LoadProgressCtrl.Instance.IsLoad = true;
                    if (LodingInfoTips != null && LodingInfoTips.Count > 0)
                        _UI_CS_LoadProgressCtrl.Instance.LoadObjNameText.Text = LodingInfoTips[4];

                    float startTime = Time.realtimeSinceStartup;
                    yield return tempWWW;

                    _UI_CS_LoadProgressCtrl.Instance.IsLoad = false;

                    DownLoadThings -= 1;
                    //Debug.LogError("-1 : " + DownLoadThings + " || " + mapKaystr);

                    BundleProgress += (1f / TotalDownLoadThings);

                    if (CachedSoundMap.ContainsKey(mapKaystr))
                    {
                        CachedSoundMap[mapKaystr].bDownloaded = true;
                        CachedSoundMap[mapKaystr].DownLoadedTransform = (Transform)tempWWW.assetBundle.mainAsset;
                    }
                }
                else
                {
                    if (CachedSoundMap[mapKaystr].bDownloaded == false)
                        yield return null;

                    if (CachedSoundMap[mapKaystr].bDownloaded)
                    {
                        DownLoadThings -= 1;
                        //Debug.LogError("-1 : " + DownLoadThings + " || " + mapKaystr);
                        BundleProgress += (1f / TotalDownLoadThings);
                    }
                }
            }
        }
    }

    #endregion

    #region Equip Models Animations and Sounds to monsters

    void EquipPerfab(Transform theMonster, string[] keyStrs)
    {
        string[] temStrList = keyStrs[0].Split(':');

        if (temStrList.Length > 1)
        {
            string sreKey = temStrList[0].ToLower();

            if (sreKey == "model")
            {
                EquipModle(theMonster, keyStrs);
            }
            else if (sreKey == "animation")
            {
                EquipAnimation(theMonster, keyStrs);
            }
            else
            {
                EquipSound(theMonster, keyStrs);
            }

        }
    }

    void EquipModle(Transform theMonster, string[] keyStrs)
    {
        string[] temStrList = keyStrs[0].Split(':');

        if (temStrList.Length > 1)
        {
            string sreKey = temStrList[0].ToLower();

            if (sreKey == "model")
            {
                string mapKaystr = temStrList[1].Replace(".assetbundle", "");

                Transform modelPerfab = null;

                if (CachedModelMap.ContainsKey(mapKaystr))
                {
                    modelPerfab = CachedModelMap[mapKaystr].DownLoadedTransform;

                    if (modelPerfab != null)
                    {
						FieldInfo _tempF = theMonster.GetComponent("NpcCreateModel").GetType().GetField("ModelPrefab");
						_tempF.SetValue(theMonster.GetComponent("NpcCreateModel"), (object)modelPerfab);
						//Transform temp = theMonster.GetComponent("NpcCreateModel").GetType().GetProperty("ModelPrefab").GetValue(temp,null) as Transform;
//                        if (theMonster.GetComponent<NpcCreateModel>())
//                            theMonster.GetComponent<NpcCreateModel>().ModelPrefab = modelPerfab;
                    }
                }
            }
        }
    }

    void EquipAnimation(Transform theMonster, string[] stringlst)
    {
        string[] temStrList = stringlst[0].Split(':');

        string sreKey = temStrList[0].ToLower();

        string bundle = temStrList[1];

        if (sreKey == "animation")
        {
            string mapKaystr = bundle.Replace(".assetbundle", "");

            UnityEngine.Object[] AnimationClips = null;

            if (CachedAnimationMap.ContainsKey(mapKaystr))
            {
                AnimationClips = CachedAnimationMap[mapKaystr].ObjectList;

                if (theMonster.GetComponent<Animation>() == null)
                {
                    theMonster.gameObject.AddComponent<Animation>();
                }

                if (theMonster.GetComponent<Animation>() != null && AnimationClips != null)
                {
                    foreach (UnityEngine.Object it in AnimationClips)
                    {
                        AnimationClip newClip = it as AnimationClip;

                        if (newClip != null)
                        {
                            theMonster.GetComponent<Animation>().AddClip(newClip, newClip.name);

                        }
                    }
                }

                List<AnimationClip> spawnAnimations = new List<AnimationClip>();

                List<AnimationClip> attackAnimations = new List<AnimationClip>();

                List<AnimationClip> deathAnimations = new List<AnimationClip>();

                for (int i = 1; i < stringlst.Length; i++)
                {
                    temStrList = stringlst[i].Split(':');

                    sreKey = temStrList[0].ToLower();

                    if (temStrList.Length <= 1)
                    {
                        continue;
                    }

                    bundle = temStrList[1];

                    if (sreKey == "idle")
                    {
                        if (theMonster.GetComponent<NpcBase>())
                        {
                            if (theMonster.animation[bundle] != null)
                                theMonster.GetComponent<NpcBase>().IdleAnim = theMonster.animation[bundle].clip;
                        }
                    }
                    else if (sreKey == "spawn")
                    {
                        if (theMonster.GetComponent<NpcBase>())
                        {
                            if (theMonster.animation[bundle] != null)
                                spawnAnimations.Add(theMonster.animation[bundle].clip);

                            theMonster.GetComponent<NpcBase>().SpawnAnims = spawnAnimations.ToArray();

                        }
                    }
                    else if (sreKey == "sleep")
                    {
                        if (theMonster.GetComponent<NpcBase>())
                        {
                            if (theMonster.animation[bundle] != null)
                                theMonster.GetComponent<NpcBase>().SleepAnim = theMonster.animation[bundle].clip;
                        }
                    }
                    else if (sreKey == "wakeup")
                    {
                        if (theMonster.GetComponent<NpcBase>())
                        {
                            if (theMonster.animation[bundle] != null)
                                theMonster.GetComponent<NpcBase>().WakeupAnim = theMonster.animation[bundle].clip;
                        }
                    }
                    else if (sreKey == "alert")
                    {
                        if (theMonster.GetComponent<NpcBase>())
                        {
                            if (theMonster.animation[bundle] != null)
                                theMonster.GetComponent<NpcBase>().AlertAnim = theMonster.animation[bundle].clip;
                        }
                    }
                    else if (sreKey == "attackidle")
                    {
                        if (theMonster.GetComponent<NpcBase>())
                        {
                            if (theMonster.animation[bundle] != null)
                                theMonster.GetComponent<NpcBase>().AttackIdleAnim = theMonster.animation[bundle].clip;
                        }
                    }
                    else if (sreKey == "run")
                    {
                        if (theMonster.GetComponent<NpcBase>())
                        {
                            if (theMonster.animation[bundle] != null)
                                theMonster.GetComponent<NpcBase>().RunAnim = theMonster.animation[bundle].clip;
                        }
                    }
                    else if (sreKey == "walk")
                    {
                        if (theMonster.GetComponent<NpcBase>())
                        {
                            if (theMonster.animation[bundle] != null)
                                theMonster.GetComponent<NpcBase>().WalkAnim = theMonster.animation[bundle].clip;
                        }
                    }
                    else if (sreKey == "turnleft")
                    {
                        if (theMonster.GetComponent<NpcBase>())
                        {
                            if (theMonster.animation[bundle] != null)
                                theMonster.GetComponent<NpcBase>().TurnLeftAnim = theMonster.animation[bundle].clip;
                        }
                    }
                    else if (sreKey == "turnright")
                    {
                        if (theMonster.GetComponent<NpcBase>())
                        {
                            if (theMonster.animation[bundle] != null)
                                theMonster.GetComponent<NpcBase>().TurnRightAnim = theMonster.animation[bundle].clip;
                        }
                    }
                    else if (sreKey == "damage")
                    {
                        if (theMonster.GetComponent<NpcBase>())
                        {
                            if (theMonster.animation[bundle] != null)
                                theMonster.GetComponent<NpcBase>().DamageAnim = theMonster.animation[bundle].clip;
                        }
                    }
                    else if (sreKey == "death")
                    {
                        if (theMonster.GetComponent<NpcBase>())
                        {
                            if (theMonster.animation[bundle] != null)
                                deathAnimations.Add(theMonster.animation[bundle].clip);

                            theMonster.GetComponent<NpcBase>().DeathAnims = deathAnimations.ToArray();
                        }
                    }
                    else if (sreKey == "stun")
                    {
                        if (theMonster.GetComponent<NpcBase>())
                        {
                            if (theMonster.animation[bundle] != null)
                                theMonster.GetComponent<NpcBase>().StunAnim = theMonster.animation[bundle].clip;
                        }
                    }
                    else if (sreKey == "knockback")
                    {
                        if (theMonster.GetComponent<NpcBase>())
                        {
                            if (theMonster.animation[bundle] != null)
                                theMonster.GetComponent<NpcBase>().KnockBackAnim = theMonster.animation[bundle].clip;
                        }

                    }
                    else if (sreKey == "attack")
                    {
                        if (theMonster.GetComponent<NpcBase>())
                        {
                            if (theMonster.animation[bundle] != null)
                                attackAnimations.Add(theMonster.animation[bundle].clip);

                            theMonster.GetComponent<NpcBase>().AttackAnims = attackAnimations.ToArray();

                        }
                    }
                }
            }
        }
    }

    void EquipSound(Transform theMonster, string[] stringlst)
    {
        List<Transform> attackSounds = new List<Transform>();

        List<Transform> miscSounds = new List<Transform>();

        foreach (string it in stringlst)
        {
            string[] temStrList = it.Split(':');

            if (temStrList.Length <= 1)
                continue;

            string bundle = temStrList[1];

            string sreKey = temStrList[0].ToLower();

            string mapKaystr = bundle.Replace(".assetbundle", "");

            Transform tSoundPerfab = null;

            if (CachedSoundMap.ContainsKey(mapKaystr))
            {
                tSoundPerfab = CachedSoundMap[mapKaystr].DownLoadedTransform;
            }

            if (sreKey == "attackidlesound")
            {
                if (theMonster.GetComponent<NpcSoundEffect>())
                    theMonster.GetComponent<NpcSoundEffect>().AttackIdeSoundPrefab = tSoundPerfab;
            }
            else if (sreKey == "damagelightSound")
            {
                if (theMonster.GetComponent<NpcSoundEffect>())
                    theMonster.GetComponent<NpcSoundEffect>().DamageLightSoundPrefab = tSoundPerfab; ;
            }
            else if (sreKey == "damageheavySound")
            {
                if (theMonster.GetComponent<NpcSoundEffect>())
                    theMonster.GetComponent<NpcSoundEffect>().DamageHeavySoundPrefab = tSoundPerfab; ;
            }
            else if (sreKey == "sleepsound")
            {
                if (theMonster.GetComponent<NpcSoundEffect>())
                    theMonster.GetComponent<NpcSoundEffect>().SleepSoundPrefab = tSoundPerfab; ;
            }
            else if (sreKey == "sleepwakeupsound")
            {
                if (theMonster.GetComponent<NpcSoundEffect>())
                    theMonster.GetComponent<NpcSoundEffect>().SleepWakeUpSoundPrefab = tSoundPerfab; ;
            }
            else if (sreKey == "spawnfromabovesound")
            {
                if (theMonster.GetComponent<NpcSoundEffect>())
                    theMonster.GetComponent<NpcSoundEffect>().SpawnFromAboveSoundPrefab = tSoundPerfab; ;
            }
            else if (sreKey == "entrysound")
            {
                if (theMonster.GetComponent<NpcSoundEffect>())
                    theMonster.GetComponent<NpcSoundEffect>().EntrySoundPrefab = tSoundPerfab; ;
            }
            else if (sreKey == "entryidlesound")
            {
                if (theMonster.GetComponent<NpcSoundEffect>())
                    theMonster.GetComponent<NpcSoundEffect>().EntryIdleSoundPrefab = tSoundPerfab; ;
            }
            else if (sreKey == "jumpsound")
            {
                if (theMonster.GetComponent<NpcSoundEffect>())
                    theMonster.GetComponent<NpcSoundEffect>().JumpSoundPrefab = tSoundPerfab; ;
            }
            else if (sreKey == "stepsound")
            {
                if (theMonster.GetComponent<NpcSoundEffect>())
                    theMonster.GetComponent<NpcSoundEffect>().StepSoundPrefab = tSoundPerfab; ;
            }
            else if (sreKey == "attacksound")
            {
                if (theMonster.GetComponent<NpcSoundEffect>())
                {
                    attackSounds.Add(tSoundPerfab);

                    theMonster.GetComponent<NpcSoundEffect>().AttackSoundPrefab = attackSounds.ToArray();
                }

            }
            else if (sreKey == "deathsound")
            {
                if (theMonster.GetComponent<NpcSoundEffect>())
                {
                    theMonster.GetComponent<NpcSoundEffect>().DeathSoundPrefab = tSoundPerfab; ;
                }

            }
            else if (sreKey == "painsound")
            {
                if (theMonster.GetComponent<NpcSoundEffect>())
                {
                    theMonster.GetComponent<NpcSoundEffect>().PainSoundPrefab = tSoundPerfab; ;
                }
            }
            else if (sreKey == "impactsound")
            {
                if (theMonster.GetComponent<NpcSoundEffect>())
                {
                    theMonster.GetComponent<NpcSoundEffect>().ImpactSoundPrefab = tSoundPerfab; ;
                }
            }
            else if (sreKey == "idlesound")
            {
                if (theMonster.GetComponent<NpcSoundEffect>())
                {
                    theMonster.GetComponent<NpcSoundEffect>().IdleSoundPrefab = tSoundPerfab; ;
                }
            }
            else if (sreKey == "bodyfallsound")
            {
                if (theMonster.GetComponent<NpcSoundEffect>())
                {
                    theMonster.GetComponent<NpcSoundEffect>().BodyFallSoundPrefab = tSoundPerfab; ;
                }
            }
            else if (sreKey == "commonkilledsound")
            {
                if (theMonster.GetComponent<NpcSoundEffect>())
                {
                    theMonster.GetComponent<NpcSoundEffect>().CommonKilledSoundPrefab = tSoundPerfab; ;
                }
            }
            else if (sreKey == "BossSpawnnSound")
            {
                if (theMonster.GetComponent<NpcSoundEffect>())
                {
                    theMonster.GetComponent<NpcSoundEffect>().BossSpawnnSoundPrefab = tSoundPerfab; ;
                }
            }
            else if (sreKey == "bossstartsound")
            {
                if (theMonster.GetComponent<NpcSoundEffect>())
                {
                    theMonster.GetComponent<NpcSoundEffect>().BossStartSoundPrefab = tSoundPerfab; ;
                }
            }
            else if (sreKey == "misc")
            {
                if (theMonster.GetComponent<NpcSoundEffect>())
                {
                    miscSounds.Add(tSoundPerfab);

                    theMonster.GetComponent<NpcSoundEffect>().MiscSoundPrefabs = miscSounds.ToArray();
                }
            }
        }
    }

    #endregion
}

public class KarmainfoData
{
		public SMoneyEnter KarmaInfo;
		public List<Transform> KarmaInstanceList;
}
