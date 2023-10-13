using UnityEngine;
using System.Collections;

public class NormalTrap : Trap {

    public Transform VFX;
    Transform vfx;

    bool isUsed = false;

    public override void Init()
    {
        base.Init();

        if (VFX)
        {
            vfx = Instantiate(VFX, transform.position + Vector3.down * 0.1f, VFX.rotation) as Transform;
            vfx.parent = transform;
            vfx.gameObject.AddComponent<Rigidbody>();
            vfx.rigidbody.useGravity = false;
            vfx.rigidbody.angularVelocity = new Vector3(0, Random.Range(0.5f, 1f), 0);
        }

        isUsed = false;
    }

    public override void Explorsion()
    {
        base.Explorsion();

        if (isUsed) return;

        isUsed = true;

        if (ExplodePrefab)
        {
            Transform explorsionVFX = Instantiate(ExplodePrefab) as Transform;
            explorsionVFX.position = transform.position;
        }

        if (ExplodeSoundPrefab)
        {
            Transform explodeSound = Instantiate(ExplodeSoundPrefab) as Transform;
            explodeSound.position = transform.position;
            explodeSound.gameObject.AddComponent<DestructAfterTime>();
            explodeSound.audio.time = 0;
            explodeSound.GetComponent<DestructAfterTime>().time = explodeSound.audio.clip.length;

        }
    }

    public override void GoToHell()
    {
        base.GoToHell();

        if (vfx)
        {
            vfx.parent = null;
            if (!vfx.GetComponent<DestructAfterTime>())
                vfx.gameObject.AddComponent<DestructAfterTime>();
            vfx.GetComponent<DestructAfterTime>().DestructNow();
            vfx = null;
        }
    }
}
