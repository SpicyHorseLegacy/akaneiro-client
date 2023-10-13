using UnityEngine;
using System.Collections;

public class WhirlWindControl : MonoBehaviour {

    public Renderer[] WhirlWindVFXMeshRenderers = new Renderer[0];


    public void Go()
    {
        foreach (Renderer _child in WhirlWindVFXMeshRenderers)
        {
            TintColorFade.FadeFromTo(_child.gameObject, 0, 50, 0.2f, false);
            iTween.RotateAdd(_child.gameObject, iTween.Hash("amount", Vector3.up * 360, "speed", Random.Range(360 * 4, 360 * 5), "space", Space.World, "looptype", iTween.LoopType.loop, "easetype", iTween.EaseType.linear));
        }
    }

    public void GoToHell()
    {
        foreach (Renderer _child in WhirlWindVFXMeshRenderers)
        {
            _child.transform.parent = null;
            TintColorFade.FadeFromTo(_child.gameObject, 50, 0, 0.5f, true);
            iTween.ScaleAdd(_child.gameObject, iTween.Hash("amount", Vector3.one * 2, "time", 0.5f));
        }

        DestructAfterTime.DestructGameObjectNow(gameObject);
    }
}
