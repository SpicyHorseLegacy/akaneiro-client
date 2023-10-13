using UnityEngine;
using System.Collections;

public class SpiritBase : MonoBehaviour {
	
	[HideInInspector]	public Player player;
    [SerializeField] protected  GameObject AnimationModel;
	
	Vector3 target = Vector3.zero;
	Vector3 MoveDir = Vector3.zero;
	Vector3 LookDir = Vector3.zero;
	float dis = 0f;
	PlayerMovement pmt = null;
	
	
	[HideInInspector]
	public int level = 1;
	
	public float MoveSpeed = 3f;
	public float RotSpeed = 3f;
	public Vector3 Offset = new Vector3(-1f,1,-1f);
	
	public enum eSpiriteType
	{
		eTypeBird = 3001,
		eTypeCat = 2001,
		eTypeDog = 1001,
		eTypeKoi = 4001,
		eTypeMonkey = 5001,
		eTypeTurtle = 6001,
		eTypeYokai = 7001,
		eTypePengin = 8001,
		eTypeGaben = 9001,
		eTypeCat_Black = 2101,
		eTypeDog_Gold = 1101,
		eTypeWeasel = 10001,
		eTypeSorrow = 11001,
		eTypePhoenix = 12001,
		eTypePanther = 13001,
		eTypeFox = 14001,
		eTypeMax = 99999,
	}
	
	public eSpiriteType mSpiritType;
	
	public bool isOnlyShow = false;
	
	// Use this for initialization
	public virtual void Start () {
        AnimationModel.animation.Play();
	}
	
	void Awake()
	{
		
		if(!isOnlyShow){
		
			player = Player.Instance;
		
			CallOn();
			
			pmt = player.GetComponent<PlayerMovement>();
		}
		
		PlayIdleAnim();
		
		DontDestroyOnLoad(gameObject);
	}

	
	// Update is called once per frame
	public virtual void Update () {
	
		if(!isOnlyShow){
		
			target = player.transform.position + player.AnimationModel.TransformDirection(Offset);
			
			MoveDir = (target - transform.position).normalized;
			
			dis = (target - transform.position).magnitude;
			
			
			if(dis > MoveSpeed * Time.deltaTime)
			{
				transform.position += MoveDir * MoveSpeed * Time.deltaTime;
				
				if(!IsPlayRunAnim())
				   PlayRunAnim();
			}
			else
			{
				transform.position = target;
				
				if( pmt != null && pmt.IsMoving == false)
			    {
				     if(!IsPlayIdleAnim())
				        PlayIdleAnim();
			    }
			    else if(pmt != null && pmt.IsMoving == true)
			    {
				     if(!IsPlayRunAnim())
				        PlayRunAnim();
			    }
				
			}
			
			LookDir = (player.transform.position - transform.position).normalized;
			LookDir.y = 0f;
			if(transform.forward != LookDir)
			{
				transform.forward = Vector3.Lerp(transform.forward, LookDir, RotSpeed);
			}
		
		}
		
	}
	
	public virtual void PlayIdleAnim()
	{
		
	}
	
	public virtual void PlayRunAnim()
	{
		
	}
	
	public virtual bool IsPlayIdleAnim()
	{
		return false;
	}
	
	public virtual bool IsPlayRunAnim()
	{
		return false;
	}
	
	
	
	public virtual void CallOn()
	{
		transform.position = player.transform.position + player.transform.TransformDirection(Offset);
		
	}
	
	public virtual void CallOff()
	{
	}
}
