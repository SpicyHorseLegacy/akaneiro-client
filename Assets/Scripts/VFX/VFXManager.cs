using UnityEngine;
using System.Collections;

public class VFXManager : MonoBehaviour {
	
	static public VFXManager Instance = null;
	
	public Transform CommoneImpactPrefab;
	public Transform FlameImpactPrefab;
	public Transform FrostImpactPrefab;
	public Transform ExplosionImpactPrefab;
    public Transform StomeImpactPrefab;
    public Transform StormImpactPrefab;

    public Transform Enemy_CommoneImpactPrefab;
    public Transform Enemy_FlameImpactPrefab;
    public Transform Enemy_FrostImpactPrefab;
    public Transform Enemy_ExplosionImpactPrefab;
    public Transform Enemy_StomeImpactPrefab;

	void Awake()
	{
		Instance = this;
	}
	
	public void PlayImpactEffectAtTransform(EStatusElementType element, Transform owner)
	{
		Transform vfx = null;

        #region Player Or Ally
        if (owner.GetComponent<Player>() || owner.GetComponent<AllyNpc>())
        {
            switch (element.Get())
            {
                case EStatusElementType.StatusElement_Invalid:
                    vfx = Instantiate(Enemy_CommoneImpactPrefab) as Transform;
                    break;

                case EStatusElementType.StatusElement_Flame:
                    vfx = Instantiate(Enemy_FlameImpactPrefab) as Transform;
                    break;

                case EStatusElementType.StatusElement_Frost:
                    vfx = Instantiate(Enemy_FrostImpactPrefab) as Transform;
                    break;

                case EStatusElementType.StatusElement_Explosion:
                    vfx = Instantiate(Enemy_ExplosionImpactPrefab) as Transform;
                    break;

                case EStatusElementType.StatusElement_Storm:
                    vfx = Instantiate(Enemy_StomeImpactPrefab) as Transform;
                    break;
            }
        }
        #endregion
        else
        #region Enemy
        {
            switch (element.Get())
            {
                case EStatusElementType.StatusElement_Invalid:
                    vfx = Instantiate(CommoneImpactPrefab) as Transform;
                    break;

                case EStatusElementType.StatusElement_Flame:
                    vfx = Instantiate(FlameImpactPrefab) as Transform;
                    break;

                case EStatusElementType.StatusElement_Frost:
                    vfx = Instantiate(FrostImpactPrefab) as Transform;
                    break;

                case EStatusElementType.StatusElement_Explosion:
                    vfx = Instantiate(ExplosionImpactPrefab) as Transform;
                    break;

                case EStatusElementType.StatusElement_Storm:
                    vfx = Instantiate(StormImpactPrefab) as Transform;
                    break;
            }
        }
        #endregion

        if (vfx)
			vfx.position = owner.position + Vector3.up * 0.5f;
	}
}
