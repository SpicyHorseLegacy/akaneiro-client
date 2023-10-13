using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerMovementAbilityBaseState : PlayerAbilityBaseState {

    protected List<Collider> throughableWalls = new List<Collider>();

    /// <summary>
	/// Checks if there is a wall in the front of player.
	/// </summary>
	/// <returns>
	/// The if wall.
	/// </returns>
	/// <param name='dis'>
	/// Dis.
	/// </param>
	protected Vector3 checkIfWall(Vector3 targetPos)
	{
        float dis = (float)Vector3.Distance(targetPos, Owner.position) + 1;
        if (dis > Info.EndDistance)
            dis = Info.EndDistance;

		Vector3 originPos = Owner.position + Vector3.up * 0.5f;
		Vector3 originaldir = Owner.GetComponent<PlayerMovement>().PlayerObj.forward.normalized;
		int layer =   1<<LayerMask.NameToLayer("Collision")
					| 1 << LayerMask.NameToLayer("InteractiveOBJ")
					| 1 << LayerMask.NameToLayer("Default");
		RaycastHit hit;


        for (int i = 0; i < dis + 1; i++)
        {
            Vector3 point1 = Player.Instance.GetComponent<PlayerMovement>().pointOnTheGround(originPos + originaldir * i) + Vector3.up * 0.35f;
            Vector3 point2 = Player.Instance.GetComponent<PlayerMovement>().pointOnTheGround(originPos + originaldir * (i+1)) + Vector3.up * 0.35f;
            Vector3 dir = point2 - point1;

            Debug.DrawLine(point1, point2, new Color(Random.RandomRange(0, 1.0f), Random.RandomRange(0, 1.0f), (1 / 20.0f) * i), 3);

            if (Physics.CapsuleCast(point1, point1 + Vector3.up, Player.Instance.GetComponent<CharacterController>().radius, dir, out hit, 1, layer))
            {
                Debug.DrawRay(hit.point, hit.normal, Color.red, 3);
                if (i == 0)
                {
                    return Owner.position;
                }
                return Player.Instance.GetComponent<PlayerMovement>().pointOnTheGround(point1);
            }

            // because if point2 is much higher than point1, capsulecast doesn't work fine. so recheck if there is a wall.
            if (Mathf.Abs(point1.y - point2.y) > 0.5f)
            {
                point1.y = point2.y;
                dir = point2 - point1;
                if (Physics.CapsuleCast(point1, point1 + Vector3.up, Player.Instance.GetComponent<CharacterController>().radius, dir, out hit, 1, layer))
                {
                    Debug.DrawRay(hit.point, hit.normal, Color.red, 3);
                    if (i == 0)
                    {
                        return Owner.position;
                    }
                    return Player.Instance.GetComponent<PlayerMovement>().pointOnTheGround(point1);
                }
            }
        }

        return Player.Instance.GetComponent<PlayerMovement>().pointOnTheGround(originPos + originaldir * dis);
	}

    /// <summary>
    /// Check if there is through walls in front of player within distance
    /// </summary>
    /// <returns>
    /// The if through wall.
    /// </returns>
    /// <param name='dis'>
    /// Dis.
    /// </param>
    public Vector3 checkIfThroughWall(Vector3 targetPos)
    {
        float dis = (float)Vector3.Distance(targetPos, Owner.position);
        if (dis > 0)
        {
            Vector3 originPos = Owner.position + Vector3.up * 0.5f;
            Vector3 dir = Owner.GetComponent<PlayerMovement>().PlayerObj.forward;
            Vector3 tarPos = Player.Instance.GetComponent<PlayerMovement>().pointOnTheGround(Owner.position + dir * dis);

            int layer = 1 << LayerMask.NameToLayer("DashThroughWall")
                    | 1 << LayerMask.NameToLayer("Collision")
                    | 1 << LayerMask.NameToLayer("InteractiveOBJ")
                    | 1 << LayerMask.NameToLayer("Breakable")
                    | 1 << LayerMask.NameToLayer("Default");

            Collider[] collidersAtTargetPos = Physics.OverlapSphere(tarPos, 1, layer);
            if (collidersAtTargetPos.Length > 0)
            {
                foreach (Collider collider in collidersAtTargetPos)
                {
                    ;// Debug.LogWarning(collider.transform.name);
                }
                // if there is any collider cover character at target position, character can't go through any wall.
                RaycastHit hit;
                if (Physics.Raycast(originPos, dir, out hit, dis, layer))
                {
                    //Debug.Log("hit wall : " + hit.transform.name);
                    //Debug.Log("dis : " + Vector3.Distance(originPos, hit.point));
                    if (Vector3.Distance(originPos, hit.point) < 1)
                        return Owner.position;
                    else
                        return Player.Instance.GetComponent<PlayerMovement>().pointOnTheGround(originPos + dir * (Vector3.Distance(originPos, hit.point) - 1));
                }
            }
        }
        return targetPos;
    }
}
