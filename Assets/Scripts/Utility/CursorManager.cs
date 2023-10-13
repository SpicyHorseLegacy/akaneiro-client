using UnityEngine;
using System.Collections;

public class CursorManager : MonoBehaviour {

    static public CursorManager Instance = null;

	public Texture2D NormalCursor ;  // Your cursor texture
    public int cursorSizeX  = 16;  // Your cursor size x
    public int cursorSizeY  = 16;  // Your cursor size y

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        //Screen.showCursor = false;
    }

    void OnGUI()
    {
        GUI.DrawTexture(new Rect(Event.current.mousePosition.x, Event.current.mousePosition.y, cursorSizeX, cursorSizeY), NormalCursor);
    }
}
