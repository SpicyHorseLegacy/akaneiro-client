using UnityEngine;
using System.Collections;

public class _UI_CS_MissionSummary : MonoBehaviour {
	
	//Instance
	public static _UI_CS_MissionSummary Instance = null;
	
	public UIPanel  BgPanel;
	public UIPanel  Bg2Panel;
	public UIPanel  Bg3Panel;
	public UIButton NextBtn;
	
	public UIPanel  MCInfoPanel;
	public UIPanel  MCLogoTPanel;
	public UIPanel  MCLogoPanel;
	
	public int      RewardKarma;
	public int      MissionEarnings;
	public int      CompleteBonus;
	public int      GhostText;
	public int      TotalExperience;
	public int      RewardKarmaMax;
	public int      MissionEarningsMax;
	public int      CompleteBonusMax;
	public int      GhostTextMax;
	public int      TotalExperienceMax;
	public bool     isCalculate = false;
	public SpriteText    m_RewardKarmaText;
	public SpriteText    m_MissionEarningsText;
	public SpriteText    m_CompleteBonusText;
	public SpriteText    m_GhostText;
	public SpriteText    m_TotalExperienceText;
	public bool m_calcKarma = false;
	public bool m_calcMissionEarnings = false;
	public bool m_calcCompleteBonus = false;
	public bool m_calcGhost = false;
	public bool m_calcTotalExperience = false;
	public int m_increment  = 1;
	public bool isCompleteMission = false;
	
	public Transform PointLoopSoundPrefab;
	public Transform _pointLoopSound;
	public Transform PointEndSoundPrefab;
	
	public bool 		 m_calcLevel = false;
	public bool 		 m_isCalcNextLv = false;
	public UIProgressBar m_LevelBar;
	public UIProgressBar m_LevelEffBar;
	public SpriteText    m_LevelValText;
	public float 		 m_incrementLevel  = 0.01f;
	
	public SpriteText    TitleNameText;
	public SpriteText    DescriptionText;
	
	public UIButton  npc;
	public UIButton  logo;
	
	void Awake(){
		Instance = this;
	}
	
	// Use this for initialization
	void Start () {
		NextBtn.AddInputDelegate(NextBtnDelegate);
	}
	
	// Update is called once per frame
	void Update () {
	
		if(PlayerInfoBar.Instance.isLevelCap){
			m_calcMissionEarnings = false;
			m_MissionEarningsText.Text = "0";
			m_calcCompleteBonus = false;
			m_MissionEarningsText.Text = "0";
			m_MissionEarningsText.Text = "0";
			m_calcGhost = false;
			m_GhostText.Text = "100%";
			m_calcTotalExperience = false;
			m_TotalExperienceText.Text = "0";
			m_calcLevel = false;
			m_LevelBar.Value = m_LevelEffBar.Value;
			m_LevelValText.Text = _PlayerData.Instance.playerLevel.ToString();
		}
		
		if(isCalculate){
			
			if(m_calcKarma){
				RewardKarma += m_increment;
				if(RewardKarma >= RewardKarmaMax){
					m_calcKarma = false;
					RewardKarma = RewardKarmaMax;
				}
				m_RewardKarmaText.Text = RewardKarma.ToString();
			}
			
			if(m_calcMissionEarnings){
				MissionEarnings += m_increment;
				if(MissionEarnings >= MissionEarningsMax){
					m_calcMissionEarnings = false;
					MissionEarnings = MissionEarningsMax;
				}
				m_MissionEarningsText.Text = MissionEarnings.ToString();
			}
			
			if(m_calcCompleteBonus){;
				CompleteBonus += m_increment;
				if(CompleteBonus >= CompleteBonusMax){
					m_calcCompleteBonus = false;
					CompleteBonus = CompleteBonusMax;
				}
				m_CompleteBonusText.Text = CompleteBonus.ToString();
			}
			
			if(m_calcGhost){
				GhostText += m_increment;
				if(GhostText >= GhostTextMax){
					m_calcGhost = false;
					GhostText = GhostTextMax;
				}
				m_GhostText.Text = GhostText.ToString() + "%";
			}
			
			if(m_calcTotalExperience){
				TotalExperience += m_increment;
				if(TotalExperience >= TotalExperienceMax){
					m_calcTotalExperience = false;
					TotalExperience = TotalExperienceMax;
				}
				m_TotalExperienceText.Text = TotalExperience.ToString();
			}
			
			if(m_calcLevel){
				m_LevelBar.Value += m_incrementLevel;
				if(m_LevelBar.Value >= m_LevelEffBar.Value){
					if(m_isCalcNextLv){
						InitLevelBarInfo2();
					}else{
						m_calcLevel = false;
						m_LevelBar.Value = m_LevelEffBar.Value;
					}
				}
				m_LevelValText.Text = _PlayerData.Instance.playerLevel.ToString();
			}

			if(!m_calcKarma && !m_calcMissionEarnings && !m_calcCompleteBonus &&!m_calcGhost && !m_calcTotalExperience && !m_calcLevel){
				isCalculate = false;
				if(_pointLoopSound)
					SoundCue.StopAndDestroyInstance(_pointLoopSound.gameObject);
				if(PointEndSoundPrefab)
					SoundCue.PlayPrefabAndDestroy(PointEndSoundPrefab);
			}
			
		}
		
	}
	
	public void CheckIsMissionComplete(){
		//查看剩余点数分配 任务完成不请求 1.任务完成完结会请求 2.会冲突对于任务完成界面.
		if(isCompleteMission){
			isCompleteMission = false;
			AwakeCompleteSummary();
		}else{
			CS_Main.Instance.g_commModule.SendMessage(
		   		ProtocolGame_SendRequest.hasAssignedTalentPointReq()
			);
		}
	}
	
	public void SetMissionCompleteTitle(string name){
		TitleNameText.Text = name;
	}
	public void SetMissionCompleteDescription(string info){
		DescriptionText.Text = info;
	}
	
	void NextBtnDelegate(ref POINTER_INFO ptr)
	{
		switch(ptr.evt)
		{
		   case POINTER_INFO.INPUT_EVENT.TAP:
				_UI_CS_MissionLogic.Instance.RsetMissionScore();	
				MCInfoPanel.Dismiss();
				CloseSummary();
				if(_UI_CS_LevelUp.Instance.IsLevelUp){
					_UI_CS_LevelUp.Instance.IsLevelUp = false;
//					_UI_CS_LevelUp.Instance.AwakeLevelUp();
					CS_Main.Instance.g_commModule.SendMessage(
				   		ProtocolGame_SendRequest.hasAssignedTalentPointReq()
					);
					//if (BGManager.Instance)
					//	BGManager.Instance.ExitOutsideAudio(2);
				}else{
                    if (BGManager.Instance)
                    {
                        BGMInfo.AutoPlayBGM = true;
                        BGManager.Instance.PlayOriginalBG(2);
                        BGMInfo.AutoPlayBGM = false;
                    }
					CloseSummary();
					Time.timeScale = 1;	
					_UI_CS_ScreenCtrl.Instance.SetNextScreenType(_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_INGAME_NORMAL);
					_UI_CS_FightScreen.Instance.m_CS_Ingame_NormalPanel.BringIn();
//					_UI_CS_FightScreen.Instance.m_fightPanel.BringIn();
					_UI_MiniMap.Instance.isShowSmallMap = true;

                    //Player.Instance.ReactivePlayer();
				}
				break;
		   default:
				break;
		}	
	}
	
	public void CloseSummary(){
		BgPanel.Dismiss();
		Bg2Panel.Dismiss();
		Bg3Panel.Dismiss();
		MoneyBadgeInfo.Instance.Hide(false);
	}
	
	public void MissionSummaryPanelDelegate(UIPanelBase panel, EZTransition trans){

		isCalculate = true;
		m_calcKarma = true;
		m_calcMissionEarnings = true;     		
		m_calcCompleteBonus = true;
		m_calcGhost = true;
		m_calcTotalExperience = true;
		m_calcLevel = true;
		RewardKarma = 0;
		MissionEarnings = 0;
		CompleteBonus = 0;
		GhostText = 0;
		TotalExperience = 0;

//		if(0 == string.Compare(trans.name,"Bring In Forward")){
			Bg2Panel.BringIn();
			Bg3Panel.BringIn();
//		}
		
		if(PointLoopSoundPrefab)
			_pointLoopSound = SoundCue.PlayInstance(PointLoopSoundPrefab.gameObject);
	}
	
	private IEnumerator StartCalcCB()
	{
		yield return new WaitForSeconds(0.5f);
		
		isCalculate = true;
		m_calcKarma = true;
		m_calcMissionEarnings = true;     		
		m_calcCompleteBonus = true;
		m_calcGhost = true;
		m_calcTotalExperience = true;
		m_calcLevel = true;
		RewardKarma = 0;
		MissionEarnings = 0;
		CompleteBonus = 0;
		GhostText = 0;
		TotalExperience = 0;
		Bg2Panel.BringIn();
		Bg3Panel.BringIn();
		if(PointLoopSoundPrefab)
			_pointLoopSound = SoundCue.PlayInstance(PointLoopSoundPrefab.gameObject);
	}
	
	public Transform MissionCompleteSoundPrefab;
	
	public void AwakeCompleteSummary(){
		_UI_CS_ScreenCtrl.Instance.SetNextScreenType(_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_MISSION_SUMMARY);
		_UI_CS_FightScreen.Instance.m_CS_Ingame_NormalPanel.Dismiss();
		_UI_CS_IngameMenu.Instance.m_CS_Ingame_MenuPanel.Dismiss();
		_UI_CS_FightScreen.Instance.m_fightPanel.Dismiss();
		_UI_MiniMap.Instance.isShowBigMap = false;
		_UI_MiniMap.Instance.isShowSmallMap = false;
		MoneyBadgeInfo.Instance.Hide(true);
		RewardKarmaMax =  (RewardKarmaMax + _UI_CS_MissionLogic.Instance.MissionKarma);
		MissionEarningsMax = _UI_CS_MissionLogic.Instance.MissionScore;
		GhostTextMax = 100;
		TotalExperienceMax = ((CompleteBonusMax + MissionEarningsMax) * GhostTextMax / 100);
		_UI_CS_KillChain.Instance.DismissKillChainMsg();
		InitLevelBarInfo();
        if (MissionCompleteSoundPrefab && BGManager.Instance)
			BGManager.Instance.PlayOutsideBGM(MissionCompleteSoundPrefab,0,2);
		BgPanel.BringIn();
		MCLogoTPanel.BringIn();
		MCLogoPanel.BringIn();
		StartCoroutine(StartLogoCB());
		npc.SetUVs(new Rect(0,0,1,1));
		logo.SetUVs(new Rect(0,0,1,1));
		//downloading image
		TextureDownLoadingMan.Instance.DownLoadingTexture("Figure_use6",npc.transform);
		TextureDownLoadingMan.Instance.DownLoadingTexture("Mission_P_Complete",logo.transform);
		
	}
	
	private IEnumerator StartLogoCB()
	{
		yield return new WaitForSeconds(2f);
		MissionCompleteLogoEnd();
	}
	
	public void MissionCompleteLogoEnd(){
		MCLogoPanel.Dismiss();
		MCLogoTPanel.Dismiss();
		MCInfoPanel.BringIn();
		StartCoroutine(StartCalcCB());
	}
	
	public void InitLevelBarInfo(){
		long tcurexp = _PlayerData.Instance.readCurExpVal(_PlayerData.Instance.playerLevel);
		if(TotalExperienceMax >= (_PlayerData.Instance.playerCurrentExp-tcurexp)){
			m_isCalcNextLv = true;
			// alrd lv up so
			long curExp = 0;
			long maxExp = _PlayerData.Instance.ReadMaxExpVal(_PlayerData.Instance.playerLevel-1);
			curExp = maxExp - (TotalExperienceMax - (_PlayerData.Instance.playerCurrentExp- tcurexp)) ;
			if(0 > curExp){
				curExp = 0;
			}
			tcurexp = _PlayerData.Instance.readCurExpVal(_PlayerData.Instance.playerLevel-1);
			curExp = (curExp-tcurexp);
			if(curExp < 0) {
				curExp = 0;
			}
			m_LevelBar.Value = (float)curExp/(float)(maxExp-tcurexp);
			m_LevelEffBar.Value = 1;
			m_LevelValText.Text = (_PlayerData.Instance.playerLevel-1).ToString();
		}else{
			m_isCalcNextLv = false;
			long curExp = _PlayerData.Instance.playerCurrentExp - TotalExperienceMax;
			if(curExp < 0) {
				curExp = 0;
			}
			long maxExp = _PlayerData.Instance.ReadMaxExpVal(_PlayerData.Instance.playerLevel);
			m_LevelBar.Value = (float)curExp/(float)(maxExp-tcurexp);
			m_LevelEffBar.Value = (float)(_PlayerData.Instance.playerCurrentExp-tcurexp)/(float)(maxExp-tcurexp);
			m_LevelValText.Text = _PlayerData.Instance.playerLevel.ToString();
		}
	}
	
	public void InitLevelBarInfo2(){
			m_isCalcNextLv = false;
			long curExp = 0;
			long maxExp = _PlayerData.Instance.ReadMaxExpVal(_PlayerData.Instance.playerLevel);
			long tcurexp = _PlayerData.Instance.readCurExpVal(_PlayerData.Instance.playerLevel);
			m_LevelBar.Value = (float)curExp/(float)maxExp;
			m_LevelEffBar.Value = (float)(_PlayerData.Instance.playerCurrentExp-tcurexp)/(float)(maxExp-tcurexp);
			m_LevelValText.Text = _PlayerData.Instance.playerLevel.ToString();
	}
}
