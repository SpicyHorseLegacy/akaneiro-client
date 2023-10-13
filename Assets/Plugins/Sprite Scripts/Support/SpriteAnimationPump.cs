//-----------------------------------------------------------------
//  Copyright 2009 Brady Wright and Above and Beyond Software
//	All rights reserved
//-----------------------------------------------------------------


#define PUMP_ALWAYS_RUNNING
#define PUMP_EVERY_FRAME
// #define USE_DELTA_TIME	// Base animation on Time.deltaTime instead of Time.realtimeSinceStartup


using UnityEngine;
using System.Collections;


/// <remarks>
/// Drives all sprite animation using a coroutine.
/// A SpriteAnimationPump instance will be automatically 
/// created by the first sprite created in the scene.
/// </remarks>
public class SpriteAnimationPump : MonoBehaviour
{
	static SpriteAnimationPump instance = null;

	// The first element in our sprite list:
	protected static ISpriteAnimatable head;

	// Used to iterate through the sprite list:
	protected static ISpriteAnimatable cur;


	// Pump state vars:
	static float startTime;
	static float time;
	static float timePaused;
	static bool isPaused = false;

	// Indicates whether the animation pump is
	// currently running. The pump can also be
	// turned off by setting this value to false.
	protected static bool pumpIsRunning = false;

	// Tells us when the coroutine has run its course.
	protected static bool pumpIsDone = true;

	/// <summary>
	/// Returns whether or not the pump is currently running.
	/// </summary>
	public bool IsRunning
	{
		get { return pumpIsRunning; }
	}

	/// <summary>
	/// The interval between animation coroutine updates.
	/// Defaults to 0.03333f (30 frames per second).
	/// </summary>
	public static float animationPumpInterval = 0.03333f;


	void Awake()
	{
		instance = this;
		DontDestroyOnLoad(this);
	}

	// Makes sure we account for realtime passage while paused
	// due to loss of focus, etc:
	void OnApplicationPause(bool paused)
	{
#if !USE_DELTA_TIME
		if (paused && !isPaused)
		{
			timePaused = Time.realtimeSinceStartup;
		}
		else if (!paused && isPaused)
		{
			// See how long we were paused:
			float pauseDuration = Time.realtimeSinceStartup - timePaused;
			startTime += pauseDuration;
		}

		isPaused = paused;
#endif
	}

	/// <summary>
	/// Starts the animation pump coroutine.
	/// Normally, there is no need to call 
	/// this directly.  Only use this if you
	/// have manually stopped the pump to pause
	/// all animations or something.
	/// </summary>
	public void StartAnimationPump()
	{
		if (!pumpIsRunning)
		{
			pumpIsRunning = true;
			StartCoroutine(PumpStarter());
		}
	}

	// Coroutine that gets the pump started:
	protected IEnumerator PumpStarter()
	{
		while (!pumpIsDone)
			yield return null;

		StartCoroutine(AnimationPump());
	}

	/// <summary>
	/// Stops the animation pump from running.
	/// Normally, there is no need to call 
	/// this directly.  Only use this if you
	/// want to pause all animations or something.
	/// </summary>
	public static void StopAnimationPump()
	{
#if !PUMP_ALWAYS_RUNNING
		pumpIsRunning = false;
		//Instance.StopAllCoroutines();
#endif
	}

/*
	void Update()
	{
		if(!pumpIsRunning)
			return;	// Abort

		cur = head;
		
		while(cur != null)
		{
			cur.StepAnim(Time.deltaTime);
			cur = cur.next;
		}
	}
*/

	// The coroutine that drives animation:
	protected static IEnumerator AnimationPump()
	{
#if !USE_DELTA_TIME
		startTime = Time.realtimeSinceStartup;
#else
		startTime = Time.time;
#endif
		float elapsed;
		ISpriteAnimatable next;

		pumpIsDone = false;

		while (pumpIsRunning)
		{
			// Check for pause:
			if ((!isPaused && Time.timeScale == 0) ||
				(isPaused && Time.timeScale != 0))
				instance.OnApplicationPause(Time.timeScale == 0);

#if !PUMP_EVERY_FRAME
			yield return new WaitForSeconds(animationPumpInterval);
#else
			yield return null;
#endif


#if USE_DELTA_TIME
			time = Time.time;
            elapsed = time - startTime;
            startTime = time;
#else
			time = Time.realtimeSinceStartup;
			elapsed = time - startTime;
			startTime = time;
#endif

			// Start at the beginning:
			cur = head;

			while( cur != null )
			{
				next = (ISpriteAnimatable)cur.next;
				cur.StepAnim(elapsed);
				cur = next;
			}
		}

		pumpIsDone = true;
	}

	public static SpriteAnimationPump Instance
	{
		get 
		{
			if(instance == null)
			{
				GameObject go = new GameObject("SpriteAnimationPump");
				instance = (SpriteAnimationPump)go.AddComponent(typeof(SpriteAnimationPump));
			}

			return instance;
		}
	}
	
	public void OnDestroy()
	{
		instance = null;
	}


	// Adds the specified sprite to the animation list.
	// s: A reference to the sprite to be animated.
	public static void Add(ISpriteAnimatable s)
	{
		if(head != null)
		{
			s.next = head;
			head.prev = s;
			head = s;
		}
		else
		{
			head = s;

			// We've got our first item, so
			// we need to start the pump:
			Instance.StartAnimationPump();
		}
	}

	// Removes the specified sprite from the animation list,
	// thereby not receiving animation updates.
	// s: A reference to the sprite to be removed.
	public static void Remove(ISpriteAnimatable s)
	{
		if(head == s)
		{
			head = (ISpriteAnimatable)s.next;
			
			// See if we need to stop the pump:
			if(head == null)
			{
				StopAnimationPump();
			}
		}
		else
		{
			if(s.next != null)
			{	// Connect either side:
				s.prev.next = s.next;
				s.next.prev = s.prev;
			}
			else if(s.prev != null)
			{
				// Removing the last item:
				s.prev.next = null;
			}
		}
		s.next = null;
		s.prev = null;
	}
}