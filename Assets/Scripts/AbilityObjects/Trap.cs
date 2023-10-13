using UnityEngine;
using System.Collections;

public class Trap : AbilityObject {

	public Transform ExplodePrefab;
    public Transform ExplodeSoundPrefab;
	
	public virtual void Explorsion()
	{
        print("trap explode!");
	}

    public virtual void GoToHell()
    {
        if (GetComponent<DestroyAfterTimeWithSpeed>())
        {
            GetComponent<DestroyAfterTimeWithSpeed>().StartWithTime(1);
        }
    }
}
