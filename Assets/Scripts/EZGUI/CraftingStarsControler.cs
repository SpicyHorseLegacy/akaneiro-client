using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CraftingStarsControler : MonoBehaviour 
{
	[SerializeField]
	List<UIButton> stars = new List<UIButton>();
	[SerializeField]
	List<UIButton> starSlots = new List<UIButton>();
	[SerializeField]
	float starAnimTime = 0.3f;
	[SerializeField]
	Transform starSFX = null;
	int currentLevel = 1;
	
	public void HideSlots(bool hide)
	{
		foreach(UIButton star in starSlots)
			star.Hide(hide);
	}
	
	public void SetupLevel()
	{
		SetupLevel(currentLevel);
	}
	
	public void SetupLevel(int level)
	{
		currentLevel = level;
		for(int i = 0; i < stars.Count; ++i)
		{
			stars[i].renderer.enabled = i < currentLevel;
			stars[i].controlIsEnabled = true;
		}
	}
	
	public void prepearForAnim()
	{
		for(int i = 0; i < stars.Count; ++i)
		{
			stars[i].controlIsEnabled = false;
			stars[i].renderer.enabled = true;
		}
	}
	
	public IEnumerator startAnim()
	{
		for(int i = 0; i < currentLevel; ++i)
		{
			stars[i].controlIsEnabled = true;
			SoundCue.PlayPrefabAndDestroy(starSFX);
			yield return new WaitForSeconds(starAnimTime);
		}
	}
}
