using UnityEngine;
using System.Collections;


//[ExecuteInEditMode]
public class CameraAdapting : MonoBehaviour {
	
	//public Transform CameraEffect1;
	//public GameObject CameraEffect2;
	//public GameObject CameraEffect3;
	//public GameObject CameraEffect4;
	/*
	public string[]  s1EffectList;
	public string[]  s2EffectList;
	public string[]  s3EffectList;
	public string[]  s4EffectList;
	*/
	//private Object s1Effect;
	//private Object s2Effect;
	//private Object s3Effect;
	//private Object s4Effect;
	
	// Use this for initialization
	void Start () {
		//this.enabled
		
		//s1Effect = Instantiate(CameraEffect1);
	   
	}
	
	// Update is called once per frame
	void Update () {
		
		if (!Application.isPlaying )
		{
			
		}
	    
		else
		{
		   if(GlobalGameState.state  == "s1")
		   {
				
			 // typeof
				/*
				if( this.gameObject.GetComponent<GrayscaleEffect>())
				{
					 this.gameObject.GetComponent<GrayscaleEffect>().enabled = true;
				}
			    if(this.gameObject.GetComponent<BlurEffect>())
				{
					 this.gameObject.GetComponent<BlurEffect>().enabled = false;
				}
				*/
				if( this.gameObject.GetComponent<Lightning>())
				{
					 if(this.gameObject.GetComponent<Lightning>().enabled)
					 this.gameObject.GetComponent<Lightning>().enabled = false;
				}
				
				
			   
			     
				
				
						
		   }
	       else if(GlobalGameState.state  == "s2")
		   {
					if( this.gameObject.GetComponent<Lightning>())
				{
					 if(this.gameObject.GetComponent<Lightning>().enabled)
					 this.gameObject.GetComponent<Lightning>().enabled = false;
				}
				
		   }
		   else if(GlobalGameState.state  == "s3")
		   {
			  	if( this.gameObject.GetComponent<Lightning>())
				{
					 if(this.gameObject.GetComponent<Lightning>().enabled)
					 this.gameObject.GetComponent<Lightning>().enabled = false;
				}
		   }
		   else if(GlobalGameState.state  == "s4")
		   {
				//Lightning
				
				if( this.gameObject.GetComponent<Lightning>())
				{
					 if(!this.gameObject.GetComponent<Lightning>().enabled)
					 this.gameObject.GetComponent<Lightning>().enabled = true;
				}
				
		   }
		}
		
	} 
	
	void ToggleFunction(string FuncName,bool enable)
	{
		/*
		if( this.gameObject.GetComponent(FuncName))
		{
			if(this.gameObject.GetComponent(FuncName).GetComponent<Behaviour>())
			{
				bool bvalue= this.gameObject.GetComponent(FuncName).GetComponent<Behaviour>().enabled;
				if(bvalue != enable)
				{
					this.gameObject.GetComponent(FuncName).GetComponent<Behaviour>().enabled = enable;
				}
			}
		}
		*/
	}
}
