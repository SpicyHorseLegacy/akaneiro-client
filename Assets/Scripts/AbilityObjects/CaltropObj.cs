using UnityEngine;
using System.Collections;

public class CaltropObj : Trap {
	
	protected override void Awake() {
		base.Awake();
		Transform[] nails = transform.GetComponentsInChildren<Transform>();
        foreach (Transform nail in nails)
        {
            if (nail == transform) continue;
            if (nail.name == "PF_Caltrop")
                nail.gameObject.AddComponent<NailObj>();
        }
	}
	
	public override void Init()
	{
		base.Init();
		
		NailObj[] nails = transform.GetComponentsInChildren<NailObj>();
		foreach(NailObj nail in nails)
		{
			nail.ResetPosition();
			float x = Random.Range(-0.5f,0.5f);
			float z = Random.Range(-0.5f,0.5f);
			nail.transform.localPosition += new Vector3(x, 0, z);
			
			RaycastHit hitInfo;
			if(Physics.Raycast(nail.transform.position + Vector3.up*5f,Vector3.down,out hitInfo,10f,1 << LayerMask.NameToLayer("Walkable")))
			{
				nail.transform.position = hitInfo.point + Vector3.up * 0.1f;
			}
			
			x = Random.Range(0,360f);
			float y = Random.Range(0, 360f);
			z = Random.Range(0, 360f);
			nail.transform.eulerAngles = new Vector3(x, y, z);
		}
	}

    public override void Explorsion()
    {
        base.Explorsion();

        if (ExplodeSoundPrefab)
        {
            Transform explodeSound = Instantiate(ExplodeSoundPrefab) as Transform;
            explodeSound.position = transform.position;
            explodeSound.gameObject.AddComponent<DestructAfterTime>();
            explodeSound.audio.time = 0;
            explodeSound.GetComponent<DestructAfterTime>().time = explodeSound.audio.clip.length;
        }
    }


}
