using UnityEngine;
using System.Collections;

public class UI_Hud_BaseDragItem : MonoBehaviour {
	
	[SerializeField]  private UISprite DragItemIcon;
	
	public string SpriteName {get{return DragItemIcon.spriteName;}}
	
	protected virtual void Update () {
		
		Vector3 _temppos = transform.position;
		Vector3 _mousepos = UICamera.mainCamera.ScreenToWorldPoint(Input.mousePosition);
		_temppos.x = _mousepos.x;
		_temppos.y = _mousepos.y;
		transform.position = _temppos;
		
		if(Input.GetMouseButtonUp(0))
		{
			DragFinished();
		}
	}
	
	public void UpdateIcon(string _iconName)
	{
		DragItemIcon.spriteName = _iconName;
	}
	
	protected virtual void DragFinished()
	{
		Destroy(gameObject);
	}

}
