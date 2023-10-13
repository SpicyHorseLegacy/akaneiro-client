using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif
	
public class BSOD : MonoBehaviour {
	
	public bool haltOnErrors = true;
	
	private bool bsodActive = false;
	private string bsodMessage;
	private GUIStyle bsodStyle;
	
	static private BSOD instance = null;
	
	void Awake() {
		if (instance) {
			instance.Message("BSOD instance is already exists");
			return;
		}
		
		// prepare bsod style
		bsodStyle = new GUIStyle();
		bsodStyle.normal.textColor = Color.white;
		bsodStyle.normal.background = new Texture2D(1, 1);
		bsodStyle.normal.background.SetPixel(0, 0, Color.blue);
		bsodStyle.normal.background.Apply();
		bsodStyle.alignment = TextAnchor.MiddleCenter;
		bsodStyle.wordWrap = true;
		bsodStyle.font = Resources.Load("Fonts/BSOD", typeof(Font)) as Font;
		
		instance = this;
	}
	
	public static void Fatal(string message) {
		instance.Message(message);
	}
	
	public static void Error(string message) {
		if (instance.haltOnErrors) {
			instance.Message(message);
		} else {
#if NGUI
			PopUpBox.PopUpErr(message);
#else
			_UI_CS_PopupBoxCtrl.PopUpError(message);
#endif
		}
	}
	
	#if UNITY_EDITOR
	void Update() {
		if (EditorApplication.isCompiling) {
			Debug.Log("Unity is recompiling scripts, play session ended.");
			EditorApplication.ExecuteMenuItem("Edit/Play");
			return; 
		}
	}
	#endif

	
	private void Message(string message) {
		if (bsodActive)
			return;
		
		bsodMessage = message;
		bsodActive = true;
	}
	
	// Use this for initialization
	void Start () {
//		if (instance) {
//			instance.Message("BSOD instance is already exists");
//			return;
//		}
//		
//		// prepare bsod style
//		bsodStyle = new GUIStyle();
//		bsodStyle.normal.textColor = Color.white;
//		bsodStyle.normal.background = new Texture2D(1, 1);
//		bsodStyle.normal.background.SetPixel(0, 0, new Color(0f, 0.03921568627f, 0.568627451f, 1f));
//		bsodStyle.normal.background.Apply();
//		bsodStyle.alignment = TextAnchor.MiddleCenter;
//		bsodStyle.wordWrap = true;
//		bsodStyle.font = Resources.Load("Fonts/BSOD", typeof(Font)) as Font;
//		
//		instance = this;
	}
	
	private void OnGUI() {
		if (bsodActive) {
			GUI.Label(new Rect (0,0,Screen.width,Screen.height), bsodMessage, bsodStyle);
			if (Input.GetKeyDown(KeyCode.Space)) {
				Application.Quit();
			}
		}
	}
}
