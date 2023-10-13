using UnityEngine;
using System.Collections;

public class WorldMapArrow : MonoBehaviour {
	
	public enum	WorldMapArrowType {
		Up = 1,
		Down,
		Left,
		Right,
		Max
	}
	
	[SerializeField]
	private WorldMapArrowType type;
	
	[SerializeField]
	private Transform WorldMap;
	
	private UIButton m_Button;
	// Use this for initialization
	void Start () {
		m_Button = GetComponent<UIButton>();
		m_Button.AddInputDelegate(ButtonDelegate);
		wideHalf = WorldMap.GetComponent<UIButton>().width/2;
		heightHalf = WorldMap.GetComponent<UIButton>().height/2;
	}
	
	private bool m_IsActive = false;
	[SerializeField]
	private float m_Speed = 8f;
	// Update is called once per frame
	void Update () {
		if (m_IsActive) {
			Move();
			CheckOffest(WorldMap);
		}
	}
	
	private void Move() {
		switch(type) {
		case WorldMapArrowType.Up:
			WorldMap.position = new Vector3(WorldMap.transform.position.x,WorldMap.transform.position.y-(m_Speed * Time.deltaTime),WorldMap.transform.position.z);
			break;
		case WorldMapArrowType.Down:
			WorldMap.position = new Vector3(WorldMap.transform.position.x,WorldMap.transform.position.y+(m_Speed * Time.deltaTime),WorldMap.transform.position.z);
			break;
		case WorldMapArrowType.Left:
			WorldMap.position = new Vector3(WorldMap.transform.position.x+(m_Speed * Time.deltaTime),WorldMap.transform.position.y,WorldMap.transform.position.z);
			break;
		case WorldMapArrowType.Right:
			WorldMap.position = new Vector3(WorldMap.transform.position.x-(m_Speed * Time.deltaTime),WorldMap.transform.position.y,WorldMap.transform.position.z);
			
			break;
		default:
			break;
		}
	}
	
	private void ButtonDelegate (ref POINTER_INFO ptr)
	{
		switch(ptr.evt)
		{
			case POINTER_INFO.INPUT_EVENT.RELEASE:
				m_IsActive = false;
			break;
			case POINTER_INFO.INPUT_EVENT.RELEASE_OFF:
				m_IsActive = false;;
			break;
			case POINTER_INFO.INPUT_EVENT.TAP:
				m_IsActive = false;
			break;
			case POINTER_INFO.INPUT_EVENT.PRESS:
				m_IsActive = true;
			break;
		}
	}
	
	private float wideHalf = 0;
	private float heightHalf = 0;
	[SerializeField]
	private Transform lt;
	[SerializeField]
	private Transform br;
	private void CheckOffest(Transform obj) {
		float x = obj.localPosition.x;
		float y = obj.localPosition.y;
		float z = 0;
		if(obj.localPosition.x - wideHalf > lt.localPosition.x) {
			x = lt.localPosition.x + wideHalf;
		}
		if(obj.localPosition.x + wideHalf < br.localPosition.x) {
			x = br.localPosition.x - wideHalf;
		}
		if(obj.localPosition.y + heightHalf < lt.localPosition.y) {
			y = lt.localPosition.y - heightHalf;
		}
		if(obj.localPosition.y - heightHalf > br.localPosition.y) {
			y = br.localPosition.y + heightHalf;
		}
		obj.localPosition = new Vector3(x,y,z);
	}
}
