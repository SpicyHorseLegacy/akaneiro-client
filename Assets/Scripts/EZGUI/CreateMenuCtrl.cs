using UnityEngine;
using System.Collections;

public class CreateMenuCtrl : MonoBehaviour {
	
	public static CreateMenuCtrl Instance;
	
	public Transform [] playerObj;
	public Camera camera;
	public Vector3 Pos;
	public int currentIdx = 4;
	public int aniCount = 0;
    public Transform BGM;
	public Transform [] Idle1_Sound;
	public Transform [] Idle2_Sound;
	public Transform [] Idle3_Sound;
	
	private bool isplayIdel2 = true;

    Transform _bgm;
	
	void Awake(){
		
		Instance = this;
	}
	
	// Use this for initialization
	void Start () {
        EnableBGM();
	}

    void EnableBGM()
    {
        if (BGManager.Instance)
        {
            _bgm = Instantiate(BGM) as Transform;
            _bgm.parent = BGManager.Instance.transform;
            _bgm.audio.volume = 0;

            AudioClipFade fadein = _bgm.gameObject.AddComponent<AudioClipFade>();
            fadein.GO(2f, BGM.audio.volume, true, false);
            _bgm.audio.Play();
        }
    }

    public void DisableBGM()
    {
        if (_bgm)
        {
            AudioClipFade fadein = _bgm.gameObject.AddComponent<AudioClipFade>();
            fadein.GO(3f, 0, true, true);
        }
    }
	
	// Update is called once per frame
    void Update()
    {

        if (_UI_CS_ScreenCtrl.Instance.IsScreenType(_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_CREATE))
        {
            if (!playerObj[currentIdx].GetComponent<Animation>().isPlaying)
            {

                string _ani = "";

                if (playerObj[currentIdx].name.Contains("FS"))
                    _ani = "POW_";
                if (playerObj[currentIdx].name.Contains("NJ"))
                    _ani = "CUN_";
                if (playerObj[currentIdx].name.Contains("SM"))
                    _ani = "FOR_";

                aniCount++;
                if (aniCount > 2)
                {
                    aniCount = 0;

                    if (isplayIdel2)
                    {
                        isplayIdel2 = false;
                        playerObj[currentIdx].GetComponent<Animation>().Play(_ani + "UI_Idle_2");
                        if (null != Idle2_Sound[currentIdx])
                            SoundCue.PlayPrefabAndDestroy(Idle2_Sound[currentIdx]);

                    }
                    else
                    {
                        isplayIdel2 = true;
                        playerObj[currentIdx].GetComponent<Animation>().Play(_ani + "UI_Idle_3");
                        if (null != Idle3_Sound[currentIdx])
                            SoundCue.PlayPrefabAndDestroy(Idle3_Sound[currentIdx]);
                    }
                }
                else
                {

                    playerObj[currentIdx].GetComponent<Animation>().Play(_ani + "UI_Idle_1");
                }
            }
        }
    }
	
	//sex: 1m , 0w
	public void SelectDis(int dis,int sex){
		
		Vector3 srcPos = new  Vector3(999,999,999);
		int disIdx = 0;
		
		for(int i = 0;i<playerObj.Length;i++){
			
			playerObj[i].transform.position = srcPos;
			
		}
		
		if(sex == 1){
			
			disIdx = dis;
			
		}else{
			
			disIdx = (dis+3);
		}
		
		currentIdx = disIdx;
		playerObj[disIdx].position = Pos;
		CloseAniSound();
		
	}
	
	public void CloseAniSound(){
		
		for(int i = 0;i<6;i++){
			
//			if(null != Idle1_Sound[i]){
//				if(Idle1_Sound[i].GetComponent<AudioSource>().isPlaying){
//					Idle1_Sound[i].GetComponent<AudioSource>().Stop();
//				}
//			}
			if(null != Idle2_Sound[i]){
				if(Idle2_Sound[i].GetComponent<AudioSource>().isPlaying){
					Idle2_Sound[i].GetComponent<AudioSource>().Stop();
				}
			}
			if(null != Idle3_Sound[i]){
				if(Idle3_Sound[i].GetComponent<AudioSource>().isPlaying){
					Idle3_Sound[i].GetComponent<AudioSource>().Stop();
				}
			}
		}
	}
}
