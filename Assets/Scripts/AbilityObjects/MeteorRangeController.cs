using UnityEngine;
using System.Collections;

public class MeteorRangeController : MonoBehaviour {

    public float Factor = 19 / 5.6f;
    public ParticleEmitter[] Circles;

    public void ResizeCircle(float _size)
    {
        foreach (ParticleEmitter circle in Circles)
        {
            circle.minSize = _size * Factor;
            circle.maxSize = _size * Factor;
        }
    }

}
