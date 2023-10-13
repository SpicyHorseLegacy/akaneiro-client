using UnityEngine;
using System.Collections;

public class StateMachine  {
	
	public Transform Owner;
	
	public StateMachine(){}
	public StateMachine(Transform o){Owner=o;}
	
	State CurrentState = null;
	State PreviousState = null;
	State GlobalState = null;
	
	public virtual void SetCurrentState(State s){CurrentState = s;}
	public virtual void SetPreviousState(State s){PreviousState = s;}
	public virtual void SetGlobalState(State s){GlobalState = s;}
	
	public State GetCurrentState()	{return CurrentState;}
	public State GetPreviousState()	{return PreviousState;}
	public State GetGlobalState()	{return GlobalState;}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	public void Update () 
	{
	    //if a global state exists, call its execute method, else do nothing
	    if(GlobalState!=null) GlobalState.Execute();
	
	    //same for the current state
	    if (CurrentState!=null) CurrentState.Execute();		
	}
	
	public virtual void ChangeState(State newState)
	{
		if(newState==null) return;
		
		//keep a record of the previous state
		PreviousState = CurrentState;
		
		//call the exit method of the existing state
		if(CurrentState != null)
			CurrentState.Exit();
		
		//change state to the new state
		CurrentState = newState;
		
		//call the entry method of the new state
		CurrentState.Enter();
	}
	
	public void ChangeStateInRunstate(AbilityBaseState newState)
	{
		if(newState == null) return;
		
		PreviousState = CurrentState;
		
		CurrentState.Exit();
		
		CurrentState = newState;
		
		newState.EnterInRunState();
	}
	
	public void RevertToPreviousState()
	{
		ChangeState(PreviousState);
	}
	
	public bool IsInState(State st)
	{
		if(CurrentState==null || st == null)
			return false;
		
		return st.GetType() == CurrentState.GetType();
	}
	
	public bool IsLastState(State st)
	{
		if(PreviousState == null || st == null)
			return false;
		
		return st.GetType() == PreviousState.GetType();
	}

}
