using UnityEngine;
using System.Collections;

public class CraftingAnimFx : MonoBehaviour 
{
	[SerializeField]	GameObject hammerHitFxPrefab = null;
	[SerializeField]	GameObject craftSucceedPrefab = null;
	[SerializeField]	Transform leftHitPosition = null;
	[SerializeField]	Transform rightHitPosition = null;
	[SerializeField]	Transform succeedVfxPosition = null;
	
	// Animation callbacks
	public void AnimCallback_LeftHit()
	{
#if NGUI
		GameObject hammerhitVFX = Instantiate(hammerHitFxPrefab, leftHitPosition.position, leftHitPosition.rotation) as GameObject;
		ChangeLayerTONGUI(hammerhitVFX);
#else
		Instantiate(hammerHitFxPrefab, leftHitPosition.position, leftHitPosition.rotation);
#endif
	}
	
	public void AnimCallback_rightHit()
	{
#if NGUI
		GameObject hammerhitVFX = Instantiate(hammerHitFxPrefab, rightHitPosition.position, rightHitPosition.rotation) as GameObject;
		ChangeLayerTONGUI(hammerhitVFX);
#else
		Instantiate(hammerHitFxPrefab, rightHitPosition.position, rightHitPosition.rotation);
#endif
	}
	
	public void AnimCallback_Succeed()
	{
		#if NGUI
		GameObject successVFX = Instantiate(craftSucceedPrefab, succeedVfxPosition.position, succeedVfxPosition.rotation) as GameObject;
		ChangeLayerTONGUI(successVFX);
		successVFX = Instantiate(craftSucceedPrefab, succeedVfxPosition.position, succeedVfxPosition.rotation) as GameObject;
		ChangeLayerTONGUI(successVFX);
		successVFX = Instantiate(craftSucceedPrefab, succeedVfxPosition.position, succeedVfxPosition.rotation) as GameObject;
		ChangeLayerTONGUI(successVFX);
#else
		Instantiate(craftSucceedPrefab, succeedVfxPosition.position, succeedVfxPosition.rotation);
		Instantiate(craftSucceedPrefab, succeedVfxPosition.position, succeedVfxPosition.rotation);
		Instantiate(craftSucceedPrefab, succeedVfxPosition.position, succeedVfxPosition.rotation);
#endif
	}
	
	void ChangeLayerTONGUI(GameObject _target)
	{
		foreach(Transform _t in _target.GetComponentsInChildren<Transform>())
		{
			_t.gameObject.layer = LayerMask.NameToLayer("NGUI");;
		}
	}
}
