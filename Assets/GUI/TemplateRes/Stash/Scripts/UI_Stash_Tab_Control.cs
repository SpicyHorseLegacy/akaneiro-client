using UnityEngine;
using System.Collections;

public class UI_Stash_Tab_Control : MonoBehaviour {
	
	public int IDX;
	public UI_Stash_TabsManager Manager;
	
    UISprite TabIcon;

    void Awake() { TabIcon = GetComponent<UISprite>(); }
	
	public void Init(UI_Stash_TabsManager _manager, int _idx)
	{
		Manager = _manager;
		IDX = _idx;
	}
	
    public void Reset()
    {
        TabIcon.spriteName = "Box_3";
    }

    public void SetBoxState(bool _isOpen)
    {
        if (_isOpen)
            TabIcon.spriteName = "Box_2";
        else
            TabIcon.spriteName = "Box_1";
    }
	
	void BTN_Clicked()
	{
        Manager.TabBeClicked(IDX);
	}
}
