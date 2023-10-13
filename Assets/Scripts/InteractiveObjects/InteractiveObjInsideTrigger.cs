using UnityEngine;
using System.Collections;

public class InteractiveObjInsideTrigger : MonoBehaviour {

    public Transform LinkedObj;

    void OnTriggerEnter(Collider other)
    {
        if (Player.Instance && other.transform == Player.Instance.transform)
        {
            if (LinkedObj.collider)
            {
                LinkedObj.collider.isTrigger = true;
                if (!Player.Instance.MovementController.togetherObjs.Contains(LinkedObj))
                    Player.Instance.MovementController.togetherObjs.Add(LinkedObj);
            }
        }
    }
}
