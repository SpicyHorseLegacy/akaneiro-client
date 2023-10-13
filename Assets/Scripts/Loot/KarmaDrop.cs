using UnityEngine;
using System.Collections;

public class KarmaDrop : MonoBehaviour {

	// Use this for initialization
	
	public int typeID = 1;
	public float HealingAmount = 0f;
	public float SeekSpeed;
	public float SeekRange;
	public Vector3 RotateSpeed;
	public float SpawnDuration;
    public Vector3 SpawnOffset;
	
	public Vector3 minScale = new Vector3(1,1,1);
	
	public Vector3 maxScale = new Vector3(1,1,1);
	
	Vector3 LocalScale = new Vector3(1,1,1);
	
	public int Karma;
	public Vector2 xRange = new Vector2(-100,100);
	public Vector2 yRange = new Vector2(0,100);
	
	public Transform PickUpKarmaSound;
	
	public float grav = 10f;
	public float maxHoverImpulseRange = 10f;
	
	public float MoveDelaySeconds = 1f;
	
	float fJumSpeed = 3f;
	
	Transform PickUpSoundInstance;
	
	float Realtime;
	
    [HideInInspector]
	public bool mbMotion = false;
	
	bool mbMayMove = false;
	
	bool mbDestroyed = false;
	
	[HideInInspector]
	public int id = 0;
	
	
		
	Vector3 mRandomVector = new Vector3(0,0,0);
	
	
	void Start () {
		
	   Realtime = SpawnDuration;
	  
	   transform.position += SpawnOffset;
	   transform.Rotate( 90, 0,0);
	   
	   LocalScale.x = Random.Range(minScale.x, maxScale.x);
	   LocalScale.y = Random.Range(minScale.y, maxScale.y);
	   LocalScale.z = Random.Range(minScale.z, maxScale.z);
			
	   LocalScale.x *= transform.localScale.x;
	   LocalScale.y *= transform.localScale.y;
	   LocalScale.z *= transform.localScale.z;
		
	   transform.localScale = LocalScale;
		
	   if( gameObject.rigidbody != null)
	  {
		  mRandomVector.x = Random.Range(xRange.x, xRange.y);
		  mRandomVector.y = 0f;//Random.Range(yRange.x, yRange.y);
		  mRandomVector.z = 0f;
			
		  gameObject.rigidbody.AddForce(mRandomVector);
			
		  gameObject.rigidbody.useGravity = false;
	   }
		
	   if( PickUpKarmaSound)
		{
			PickUpSoundInstance = Instantiate(PickUpKarmaSound) as Transform;
			
			PickUpSoundInstance.parent = transform;
			
		}
		
		Vector3 tempPos = transform.position;
			
	    tempPos.y = 100f;
			
	    RaycastHit hit;
			
		int layer = 1<<LayerMask.NameToLayer("Walkable");
				
	    if(Physics.Raycast(tempPos,Vector3.down,out hit,100f,layer))
		{
		   tempPos.y = hit.point.y;
	    }
		
		transform.position = tempPos;
		
		StartCoroutine("PermitMove");
		
		//transform.Rotate(new Vector3(0,0,0));
	
	}
	
	IEnumerator PermitMove()
	{
		yield return new WaitForSeconds(MoveDelaySeconds);
		
		mbMayMove = true;
		
	}
	
	// Update is called once per frame
	void Update () {
		
		ProcessMotion();
	}
	
	public void StartMotion()
	{
		 if(mbMotion)
			return;
		
	     if( mbMayMove)
	     {
	        mbMotion = true;
			
	        foreach(Transform it in CS_SceneInfo.Instance.KarmaMap[id].KarmaInstanceList)
		    {
		        it.GetComponent<KarmaDrop>().mbMotion = true;	   
		    }
//		    _UI_CS_MissionLogic.Instance.MissionKarma += CS_SceneInfo.Instance.KarmaMap[id].KarmaInfo.money;
			
		}
	    else
		{
			mbMotion = false;
			
		}
	}
	
	void ProcessMotion()
	{
		if(mbDestroyed)
			return;

		if(mbMotion)
		{
			
			Vector3 tempTargetPos = Player.Instance.transform.position;
			
			tempTargetPos.y += 1f;
			
			
			Vector3 diffPos =  tempTargetPos - transform.position;
			
			float dis = SeekSpeed * Time.deltaTime;
			
			if( diffPos.magnitude <= dis)
			{
				transform.position = tempTargetPos;
				mbMotion = false;
				//pickup
                Player.GetKarma();
				
				if(PickUpSoundInstance)
		        {
			       PickUpSoundInstance.position = transform.position;
			       PickUpSoundInstance.rotation = transform.rotation;
			       SoundCue.Play(PickUpSoundInstance.gameObject);
		        }
				
		        ///gameObject.SetActiveRecursively(false);
			     Renderer[] rds  = transform.GetComponentsInChildren<Renderer>();
				
				 foreach(Renderer rd in rds)
				   rd.enabled = false;	
				mbDestroyed = true;
				StartCoroutine("DestroyResources");
			
			}
			else
			{
				
			    diffPos.Normalize();
				transform.position += diffPos * dis;
			}
																																																																																																																																																																																				
		}
		else
		{
			Vector3 tempTargetPos = Player.Instance.transform.position;
			
			tempTargetPos.y += 1f;
			
			//tempTargetPos.z -= 1f;
			
			
			Vector3 diffPos = tempTargetPos - transform.position;
			
			if( diffPos.magnitude <= SeekRange && !Player.Instance.FSM.IsInState(Player.Instance.DS))
			{
			     StartMotion();
			}
			
		    transform.Rotate(RotateSpeed * Time.deltaTime);
			// transform.Rotate( transform.eulerAngles.x, transform.eulerAngles.y,90);
			
	
			if(SpawnDuration != 0)
			{
			  Realtime -= Time.deltaTime;
			  if( Realtime <= 0)
			  {
				  //Destroy(PickUpSoundInstance);
				  Destroy(gameObject);
			  }
			}
			
			Vector3 tempPos = transform.position;
			
	        tempPos.y = 100f;
			
	        RaycastHit hit;
			
		    int layer = 1<<LayerMask.NameToLayer("Walkable");
				
			if(Physics.Raycast(tempPos,Vector3.down,out hit,100f,layer))
			{
				tempPos.y = hit.point.y + 1f;
			}
		
			if (transform.position.y < tempPos.y && fJumSpeed > 0)
			{
				transform.position = tempPos;
				float rseed = Random.Range(0,2);
				fJumSpeed = -(3f + rseed);
				return;
			}
			
			if(transform.position.y > tempPos.y + 1f && fJumSpeed < 0)
			{
				tempPos.y += 1f;
				transform.position = tempPos;
				float rseed = Random.Range(0,2);
				fJumSpeed = 3f + rseed;
				return;
			}
			
			
			transform.position += Vector3.down * fJumSpeed * Time.deltaTime;
	
			
			
		}
		
		
	}
	
	void OnTriggerStay(Collider other) 
	{
		/*
		if (other.gameObject.layer == LayerMask.NameToLayer("Walkable") )
		{
			
			Vector3 tempPos = transform.position;
			
	        tempPos.y = 100f;
			
	        RaycastHit hit;
			
		    int layer = 1<<LayerMask.NameToLayer("Walkable");
				
			if(Physics.Raycast(tempPos,Vector3.down,out hit,100f,layer))
			{
				tempPos.y = hit.point.y + 2f;
			}
		
			float distanceToGround = this.transform.position.y - tempPos.y; //other.ClosestPointOnBounds(this.transform.position).y;
		
			if (this.transform.position.y < other.ClosestPointOnBounds(this.transform.position).y)
			{
				return;
			}
			//Debug.Log("Distance: " + distanceToGround);
			
	        distanceToGround = Mathf.Min(maxHoverImpulseRange, distanceToGround);
			if (distanceToGround < 0.05f)
			{
				distanceToGround = .15f;
			}
	        float halfMaxHoverImpulseRange = maxHoverImpulseRange/2;
			
	        if (distanceToGround > maxHoverImpulseRange)//maxHoverImpulseRange/2) 
			{
	            transform.rigidbody.AddForce(-Vector3.up * ((distanceToGround - halfMaxHoverImpulseRange) / halfMaxHoverImpulseRange) * grav);
	        }
			else
			{
	            transform.rigidbody.AddForce(Vector3.up * (halfMaxHoverImpulseRange / distanceToGround) * grav);
				
	        }
		}
		*/
	}
	
	IEnumerator DestroyResources()
	{
		yield return new WaitForSeconds(1.5f);
		//Destroy(PickUpSoundInstance);
		Destroy(gameObject);
	}
	
	
}
