using UnityEngine;
using System.Collections;

public class PingManager : MonoBehaviour {
	
	public static PingManager Instance;
	
	void Awake() {
		Instance = this;
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	[SerializeField]
	private UISprite bg;
	[SerializeField]
	private UILabel content;
	public void UpdatePing(int _val) {
		ChangeColor(_val);
		content.text = _val.ToString();
	}
	
	private void ChangeColor(int _val) {
		if(_val < levels[0]) {
			bg.color = colors[0];
			content.color = colors[0];
		}else if(_val < levels[1]) {
			bg.color = colors[1];
			content.color = colors[1];
		}else if(_val < levels[2]) {
			bg.color = colors[2];
			content.color = colors[2];
		}else {
			bg.color = colors[3];
			content.color = colors[3];
		}
	}
	
	[SerializeField]
	private Color [] colors;
	[SerializeField]
	private int [] levels;
}
