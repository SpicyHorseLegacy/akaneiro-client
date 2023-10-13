using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CraftingAnimPanel : MonoBehaviour 
{
	[SerializeField]
	float animationTime = 0;
	[SerializeField]
	SpriteText dtProgressInfo;
	[SerializeField]
	Animation hammerAnim;
	[SerializeField]
	string startAnim = "";
	[SerializeField]
	string succeedAnim = "";
	[SerializeField]
	string failedAnim = "";
	[SerializeField]
	UIButton continueBtn = null;
	[SerializeField]
	UIProgressBar progressBar = null;
	[SerializeField]
	UIButton workingbar = null;
	[SerializeField]
	UIButton failedbar = null;
	[SerializeField]
	UIButton addedPic = null;
	[SerializeField]
	UIPanel myPanel = null;
	[SerializeField]
	CraftingStarsControler starController = null;
	[SerializeField]
	Transform craftingSfx = null;
	[SerializeField]
	Transform failedSfx = null;
	[SerializeField]
	Transform succeedSfx = null;
	
	public int attrLevel = 0;
	bool isSucceed = false;
	
	void Start()
	{
		addedPic.Hide(true);
		continueBtn.AddInputDelegate(OnContinue);
	}
	
	public void StartAnim(bool succeed)
	{
		isSucceed = succeed;
		failedbar.Hide(true);
		workingbar.Hide(false);
		addedPic.Hide(true);
		StartCoroutine(CraftingProgress(succeed));
	}
	
	IEnumerator CraftingProgress(bool succeed)
	{
		myPanel.BringIn();
		yield return null;
		CraftingDetailPanel.Instance.craftingControlPanel.Dismiss();
		
		CraftingDetailPanel.Instance.craftText.Hide(true);
		
		starController.prepearForAnim();
		
		continueBtn.controlIsEnabled = false;
		dtProgressInfo.Text = "Working...";
		hammerAnim.Play(startAnim);
		
		SoundCue.PlayPrefabAndDestroy(craftingSfx);
		
		progressBar.Value = 0;
		progressBar.Color = Color.yellow;
		
		yield return new WaitForSeconds(animationTime);
		
		progressBar.Value = 1f;
		if(succeed)
		{
			addedPic.Hide(false);
			hammerAnim.Play(succeedAnim);
			progressBar.Color = Color.green;
			dtProgressInfo.Text = "Success!!";
			
			SoundCue.PlayPrefabAndDestroy(succeedSfx);
			
			StartCoroutine( starController.startAnim() );
		}
		else
		{
			workingbar.Hide(true);
			failedbar.Hide(false);
			hammerAnim.Play(failedAnim);
			progressBar.Color = Color.red;
			dtProgressInfo.Text = "Failed...Sorry!";
			
			SoundCue.PlayPrefabAndDestroy(failedSfx);
		}
		
		continueBtn.controlIsEnabled = true;
	}
	
	void Update()
	{
		if(progressBar.Value < 1f)
		{
			progressBar.Value += 1f / animationTime * Time.deltaTime;
		}
	}
	
	void OnContinue(ref POINTER_INFO ptr){
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.TAP:
			if(isSucceed)
			{
				CraftingDetailPanel.Instance.Upgraded();
				CraftingWeaponPanel.Instance.UpdateItemInfo();
				isSucceed = false;
			}
			else
			{
				//starController.SetupLevel();
				CraftingDetailPanel.Instance.ReSetup();
			}
			CraftingDetailPanel.Instance.craftText.Hide(false);
			
			break;
		}	
	}
}
