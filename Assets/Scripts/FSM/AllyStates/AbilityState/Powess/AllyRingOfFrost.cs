using UnityEngine;
using System.Collections;

public class AllyRingOfFrost : AllyCurPosAbilityBaseState
{
    public Transform IceMeshPrefab;
    Transform iceMesh;

    public override void Enter()
    {
        base.Enter();
        Owner.GetComponent<AllyMovement>().StopMove(false);
        if (!iceMesh && IceMeshPrefab)
        {
            iceMesh = CS_Main.Instance.SpawnObject(IceMeshPrefab);
            iceMesh.position = Vector3.one * 1000.0f;
        }
    }

    public override void ProcessImpactAtPos(Vector3 _pos)
    {
        base.ProcessImpactAtPos(_pos);
        
        if (iceMesh)
        {
            iceMesh.position = _pos;
            iceMesh.GetComponent<IceObj>().Go();
        }
    }
}
