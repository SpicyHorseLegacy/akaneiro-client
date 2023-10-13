using UnityEngine;
using System.Collections;

public class PlayerStingingShot : PlayerAbilityShot {

    public override void Execute()
    {
        base.Execute();

        if (step == PrepareStep.WaitForMouseDown || step == PrepareStep.WaitForMouseUp || step == PrepareStep.WaitForReleaseKey)
        {
            // if not shoot, character always look at mousepoint on ShootLayer
            Owner.GetComponent<PlayerMovement>().LookAtShootDir();

            if (step == PrepareStep.WaitForMouseDown)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    tempMousePos = Input.mousePosition;
                    eventAfterMounting = EventAfterMount.Shoot;
                    Shoot();
                }
            }
        }
    }


	public override void AcitveAbilityWithMousePos (Vector3 mousePos)
	{
		base.AcitveAbilityWithMousePos (mousePos);
	    
		tempMousePos = mousePos;
		Owner.GetComponent<PlayerMovement>().LookAtTargetMouseInShootMode(tempMousePos);
        step = PrepareStep.WaitForServerCallback;
	}
}
