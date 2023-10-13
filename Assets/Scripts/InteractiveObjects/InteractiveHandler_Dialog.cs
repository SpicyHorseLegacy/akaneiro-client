using UnityEngine;
using System.Collections;

public class InteractiveHandler_Dialog : InteractiveHandler {

	public override void GoToHell()
    {
		base.GoToHell();
		
		if(GetComponent<PopUpDialog>())
			GetComponent<PopUpDialog>().PopMenu();
	}
}
