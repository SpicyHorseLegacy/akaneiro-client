using UnityEngine;
using System.Collections;

public enum kBuffContinueType
{
	kBuffTypeContinue = 0,				// 持续性BUFF，一直其作用，比如减速效果 
	kBuffTypeCircle = 1,				// 周期性起作用BUFF，类似于腐蚀术 
}

public class BaseBuff : DamageSource {

	public kTypeBuff BuffType;
	public int ID;
	public string Name;				                // NAME
	public string Description;		                // DESCRIPTION
	public Texture2D Icon;			                // ICON
    public int Priority;                            // Show buff which has a higher priority first
    public kBuffContinueType ContinueType;			// TYPE
    public float LifeTime;
    public int Tick;
    public bool isActiveAtBeginning;

    public int MaxStackNum;
    public int StackNum;

	public Transform Target;

    [HideInInspector]public float CurLife;
	float cycleTime;
    int curTick;
	
	public Transform VFXPrefab;
	
	public override void Enter()
	{
		base.Enter();
		// if bufftype is circle type, set new time to active 
		if(ContinueType == kBuffContinueType.kBuffTypeCircle)
		{
			cycleTime = LifeTime / Tick;
			curTick = Tick;
			
			//  if buff needs avtive at the beginning, active immediatelly
			if(isActiveAtBeginning)
				TickExecute();
			
		}else{
			cycleTime = LifeTime;
			curTick = 1;
		}
        CurLife = LifeTime;
	}
	
	public override void Execute()
	{
		base.Execute();
        CurLife -= Time.deltaTime;
        if (CurLife < 0) CurLife = 0;
		/*
		cycleTime -= Time.deltaTime;
		if(cycleTime < 0)
		{
			// if tick is out, ends Buff. But if has tick, that means it's circle type and needs to active at once.
			curTick --;
			if(curTick <= 0)
				Exit();
			else
			{
				TickExecute();
				cycleTime = LifeTime / Tick;
			}
		}
		*/
	}
	
	public override void Exit()
	{
		base.Exit();
		Owner.GetComponent<BaseHitableObject>().BuffMan.RemoveBuffFromList(this);
        if (Buff_Exit_Event != null)
            Buff_Exit_Event(this);
		Destroy(gameObject);
	}

    public delegate void Buff_Exit_Handler(BaseBuff _buff);
    public event Buff_Exit_Handler Buff_Exit_Event;

    public delegate void Buff_AddStack_Handler(BaseBuff _buff);
    public event Buff_AddStack_Handler Buff_AddStack_Event;
	
	public virtual void RefreshBuff(){
		Enter();
	}
	
	public virtual void TickExecute(){}

    public virtual void AddStack()
    {
        StackNum++;
        if (StackNum == 1)
        {
            Enter();
        }

        if (StackNum > MaxStackNum)
        {
            StackNum = MaxStackNum;
        }

        if (Buff_AddStack_Event != null)
            Buff_AddStack_Event(this);
    }

    public virtual void RemoveStack()
    {
        StackNum--;
        if (StackNum == 0)
        {
            Exit();
        }
    }
}
